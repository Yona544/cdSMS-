<template>
  <div class="legacy-scope">
    <h1 class="text-2xl font-bold mb-6">Activity Logs</h1>

    <!-- Filters -->
    <div class="mb-4">
      <form class="flex flex-wrap items-center gap-3" @submit.prevent="onFilter">
        <input class="inp-form" v-model="q" placeholder="Search text..." />
        <label class="text-sm text-gray-700">Type</label>
        <select class="border border-gray-300 h-[31px] px-2 text-sm" v-model="network">
          <option value="both">Both</option>
          <option value="call">Call</option>
          <option value="sms">SMS</option>
        </select>
        <label class="text-sm text-gray-700">From</label>
        <input type="date" class="border border-gray-300 h-[31px] px-2 text-sm" v-model="fromDate" />
        <label class="text-sm text-gray-700">To</label>
        <input type="date" class="border border-gray-300 h-[31px] px-2 text-sm" v-model="toDate" />
        <button type="submit" class="h-[31px] flex items-center">
          <img src="/legacy/admin/images/top_search_btn.gif" alt="Search" />
        </button>
      </form>
    </div>

    <div v-if="commStore.loading && !commStore.history.length" class="text-center">Loading history...</div>
    <div v-if="commStore.error" class="text-center message-red">{{ commStore.error }}</div>

    <div v-if="!commStore.loading && !visibleLogs.length" class="text-center text-gray-500 mt-8">
      No activity logs found<span v-if="q || network !== 'both' || fromDate || toDate"> for current filters</span>.
    </div>

    <div v-if="visibleLogs.length" class="gridtable p-4">
      <div class="tblheader px-3 flex items-center gap-6">
        <span class="cursor-pointer" @click="toggleSort('date')">
          Date
          <img v-if="sort === 'date' && dir === 'asc'" src="/legacy/admin/images/table_icon_3.gif" alt="Ascending" class="ml-1 align-middle" />
          <img v-else-if="sort === 'date' && dir === 'desc'" src="/legacy/admin/images/table_icon_4.gif" alt="Descending" class="ml-1 align-middle" />
        </span>
        <span class="cursor-pointer" @click="toggleSort('type')">
          Type
          <img v-if="sort === 'type' && dir === 'asc'" src="/legacy/admin/images/table_icon_3.gif" alt="Ascending" class="ml-1 align-middle" />
          <img v-else-if="sort === 'type' && dir === 'desc'" src="/legacy/admin/images/table_icon_4.gif" alt="Descending" class="ml-1 align-middle" />
        </span>
        <span class="cursor-pointer" @click="toggleSort('to')">
          To
          <img v-if="sort === 'to' && dir === 'asc'" src="/legacy/admin/images/table_icon_3.gif" alt="Ascending" class="ml-1 align-middle" />
          <img v-else-if="sort === 'to' && dir === 'desc'" src="/legacy/admin/images/table_icon_4.gif" alt="Descending" class="ml-1 align-middle" />
        </span>
        <span class="cursor-pointer" @click="toggleSort('from')">
          From
          <img v-if="sort === 'from' && dir === 'asc'" src="/legacy/admin/images/table_icon_3.gif" alt="Ascending" class="ml-1 align-middle" />
          <img v-else-if="sort === 'from' && dir === 'desc'" src="/legacy/admin/images/table_icon_4.gif" alt="Descending" class="ml-1 align-middle" />
        </span>
        <span class="cursor-pointer" @click="toggleSort('status')">
          Status
          <img v-if="sort === 'status' && dir === 'asc'" src="/legacy/admin/images/table_icon_3.gif" alt="Ascending" class="ml-1 align-middle" />
          <img v-else-if="sort === 'status' && dir === 'desc'" src="/legacy/admin/images/table_icon_4.gif" alt="Descending" class="ml-1 align-middle" />
        </span>
      </div>
      <div class="space-y-4">
        <ActivityLogItem
          v-for="log in visibleLogs"
          :key="log.id"
          :log="log"
        />
      </div>
      <div class="flex items-center gap-2 mt-4">
        <button :disabled="page <= 1" @click="goFirst" :class="{'opacity-40 cursor-not-allowed': page <= 1}">
          <img src="/legacy/admin/images/paging_far_left.gif" alt="First" />
        </button>
        <button :disabled="page <= 1" @click="goPrev" :class="{'opacity-40 cursor-not-allowed': page <= 1}">
          <img src="/legacy/admin/images/paging_left.gif" alt="Prev" />
        </button>
        <span class="text-sm text-gray-700">Page {{ page }} / {{ totalPages }}</span>
        <button :disabled="page >= totalPages" @click="goNext" :class="{'opacity-40 cursor-not-allowed': page >= totalPages}">
          <img src="/legacy/admin/images/paging_right.gif" alt="Next" />
        </button>
        <button :disabled="page >= totalPages" @click="goLast" :class="{'opacity-40 cursor-not-allowed': page >= totalPages}">
          <img src="/legacy/admin/images/paging_far_right.gif" alt="Last" />
        </button>
      </div>
    </div>

    <!-- Legacy paging rendered within the list block -->
  </div>
</template>

<script setup>
import { ref, onMounted, computed, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useCommunicationStore } from '@/stores/communication'
import ActivityLogItem from '@/components/logs/ActivityLogItem.vue'

const commStore = useCommunicationStore()
const route = useRoute()
const router = useRouter()

// Filter/sort/paging state
const q = ref(typeof route.query.q === 'string' ? route.query.q : '')
const network = ref(typeof route.query.network === 'string' ? route.query.network : 'both') // 'both' | 'call' | 'sms'
const fromDate = ref(typeof route.query.from === 'string' ? route.query.from : '')
const toDate = ref(typeof route.query.to === 'string' ? route.query.to : '')
const sort = ref(typeof route.query.sort === 'string' ? route.query.sort : 'date') // 'date'|'type'|'to'|'from'|'status'
const dir = ref(typeof route.query.dir === 'string' ? route.query.dir : 'desc')
const page = ref(Number.parseInt(route.query.page) || 1)
const pageSize = 20

onMounted(() => {
  commStore.fetchHistory(page.value)
})

function includesCI(hay, needle) {
  if (!needle) return true
  if (!hay) return false
  return String(hay).toLowerCase().includes(String(needle).toLowerCase())
}
function dateInRange(dt, from, to) {
  if (!from && !to) return true
  const t = new Date(dt)
  if (isNaN(t)) return false
  const start = from ? new Date(from + 'T00:00:00') : null
  const end = to ? new Date(to + 'T23:59:59') : null
  if (start && t < start) return false
  if (end && t > end) return false
  return true
}

const filtered = computed(() => {
  const arr = commStore.history || []
  return arr.filter(log => {
    const textOk = includesCI(log?.message_content, q.value)
      || includesCI(log?.to_number, q.value)
      || includesCI(log?.from_number, q.value)
    const networkOk = network.value === 'both' ? true : (String(log?.network_type || '').toLowerCase() === network.value)
    const dateOk = dateInRange(log?.created_at, fromDate.value, toDate.value)
    return textOk && networkOk && dateOk
  })
})

const sorted = computed(() => {
  const k = sort.value
  const direction = dir.value === 'desc' ? -1 : 1
  const arr = [...filtered.value]
  function val(log) {
    switch (k) {
      case 'date': {
        const ts = new Date(log?.created_at || 0).getTime()
        return isNaN(ts) ? 0 : ts
      }
      case 'type': return String(log?.network_type || '').toLowerCase()
      case 'to': return String(log?.to_number || '').toLowerCase()
      case 'from': return String(log?.from_number || '').toLowerCase()
      case 'status': return String(log?.status || '').toLowerCase()
      default: return 0
    }
  }
  arr.sort((a, b) => {
    const va = val(a); const vb = val(b)
    if (va < vb) return -1 * direction
    if (va > vb) return 1 * direction
    return 0
  })
  return arr
})

const totalPages = computed(() => Math.max(1, Math.ceil(sorted.value.length / pageSize)))
const visibleLogs = computed(() => {
  const p = Math.min(Math.max(1, page.value), totalPages.value)
  const start = (p - 1) * pageSize
  return sorted.value.slice(start, start + pageSize)
})

// Keep page within bounds if filtered length changes
watch(totalPages, (n) => {
  if (page.value > n) page.value = n
})

// Route sync
function updateQuery() {
  router.replace({
    path: route.path,
    query: {
      q: q.value || '',
      network: network.value || 'both',
      from: fromDate.value || '',
      to: toDate.value || '',
      sort: sort.value || 'date',
      dir: dir.value || 'desc',
      page: String(page.value || 1)
    }
  })
}
function syncFromRoute() {
  const rq = route.query
  q.value = typeof rq.q === 'string' ? rq.q : q.value
  network.value = typeof rq.network === 'string' ? rq.network : network.value
  fromDate.value = typeof rq.from === 'string' ? rq.from : fromDate.value
  toDate.value = typeof rq.to === 'string' ? rq.to : toDate.value
  sort.value = typeof rq.sort === 'string' ? rq.sort : sort.value
  dir.value = typeof rq.dir === 'string' ? rq.dir : dir.value
  page.value = Number.parseInt(rq.page) || page.value
}
watch(() => route.query, () => syncFromRoute())

// UI actions
function onFilter() {
  page.value = 1
  updateQuery()
}
function toggleSort(key) {
  if (sort.value === key) {
    dir.value = dir.value === 'asc' ? 'desc' : 'asc'
  } else {
    sort.value = key
    dir.value = key === 'date' ? 'desc' : 'asc'
  }
  updateQuery()
}
function goFirst() { if (page.value > 1) { page.value = 1; updateQuery() } }
function goPrev() { if (page.value > 1) { page.value -= 1; updateQuery() } }
function goNext() { if (page.value < totalPages.value) { page.value += 1; updateQuery() } }
function goLast() { if (page.value < totalPages.value) { page.value = totalPages.value; updateQuery() } }
</script>
