<template>
  <div class="legacy-scope">
    <h1>Users</h1>

    <!-- Actions + Search -->
    <div class="mb-4 flex items-center justify-between">
      <form class="flex items-center gap-3" @submit.prevent="onSearch">
        <input class="inp-form" v-model="q" placeholder="Search users..." />
        <button type="submit" class="h-[31px] flex items-center">
          <img src="/legacy/admin/images/top_search_btn.gif" alt="Search" />
        </button>
      </form>
      <router-link to="/users/new" class="legacy-btn" title="New" style="display:inline-flex;align-items:center;border:1px solid #dbdbdb;background:#fff;padding:4px 8px;">
        <img src="/legacy/admin/images/Submit.gif" alt="New" class="mr-1" style="margin-right:4px;" />
        <span>New</span>
      </router-link>
    </div>

    <div class="gridtable">
      <div class="tblheader px-3 flex items-center gap-6">
        <span class="cursor-pointer" @click="toggleSort('username')">
          Username
          <img v-if="sort==='username' && dir==='asc'" src="/legacy/admin/images/table_icon_3.gif" alt="Ascending" class="ml-1 align-middle" />
          <img v-else-if="sort==='username' && dir==='desc'" src="/legacy/admin/images/table_icon_4.gif" alt="Descending" class="ml-1 align-middle" />
        </span>
        <span class="cursor-pointer" @click="toggleSort('display_name')">
          Display Name
          <img v-if="sort==='display_name' && dir==='asc'" src="/legacy/admin/images/table_icon_3.gif" alt="Ascending" class="ml-1 align-middle" />
          <img v-else-if="sort==='display_name' && dir==='desc'" src="/legacy/admin/images/table_icon_4.gif" alt="Descending" class="ml-1 align-middle" />
        </span>
        <span class="cursor-pointer" @click="toggleSort('email')">
          Email
          <img v-if="sort==='email' && dir==='asc'" src="/legacy/admin/images/table_icon_3.gif" alt="Ascending" class="ml-1 align-middle" />
          <img v-else-if="sort==='email' && dir==='desc'" src="/legacy/admin/images/table_icon_4.gif" alt="Descending" class="ml-1 align-middle" />
        </span>
      </div>
      <table class="w-full">
        <thead>
          <tr class="tblrow">
            <th class="text-left p-2">Username</th>
            <th class="text-left p-2">Display Name</th>
            <th class="text-left p-2">Email</th>
            <th class="text-left p-2">Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="u in visibleUsers" :key="u.id" class="tblrow">
            <td class="p-2">{{ u.username }}</td>
            <td class="p-2">{{ u.display_name }}</td>
            <td class="p-2">{{ u.email }}</td>
            <td class="p-2">
              <router-link :to="`/users/${u.id}`" class="mr-3">
                <img src="/legacy/admin/images/action_edit.gif" alt="Edit" class="inline-block mr-1" /> Edit
              </router-link>
              <button type="button" @click="onDelete(u.id)" class="inline-flex items-center" title="Delete">
                <img src="/legacy/admin/images/action_delete.gif" alt="Delete" class="inline-block mr-1" /> Delete
              </button>
            </td>
          </tr>
        </tbody>
      </table>

      <div class="flex items-center gap-2 mt-4" v-if="totalPages > 1">
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
      <div v-if="errorMessage" class="message-red mt-2">{{ errorMessage }}</div>
      <div v-if="successMessage" class="message-green mt-2">{{ successMessage }}</div>
    </div>
  </div>
</template>
<script setup>
import { ref, onMounted, computed, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { api } from '@/utils/api'

const route = useRoute()
const router = useRouter()

const users = ref([])
const q = ref(typeof route.query.q === 'string' ? route.query.q : '')
const sort = ref(typeof route.query.sort === 'string' ? route.query.sort : 'username')
const dir = ref(typeof route.query.dir === 'string' ? route.query.dir : 'asc')
const page = ref(Number.parseInt(route.query.page) || 1)
const pageSize = 20
const successMessage = ref('')
const errorMessage = ref('')

onMounted(async () => {
  await fetchUsers()
  // Show post-save success banner and clear the query flag
  if (route.query.saved === '1') {
    successMessage.value = 'User saved.'
    const nextQuery = { ...route.query }
    delete nextQuery.saved
    router.replace({ path: route.path, query: nextQuery })
  }
})

async function fetchUsers() {
  try {
    const res = await api.get('/users')
    users.value = res.data?.data || []
  } catch (e) {
    users.value = []
    errorMessage.value = 'Failed to load users.'
  }
}

function includesCI(hay, needle) {
  if (!needle) return true
  if (!hay) return false
  return String(hay).toLowerCase().includes(String(needle).toLowerCase())
}

const filtered = computed(() => {
  const arr = users.value || []
  return arr.filter(u => {
    return includesCI(u?.username, q.value)
      || includesCI(u?.display_name, q.value)
      || includesCI(u?.email, q.value)
  })
})

const sorted = computed(() => {
  const k = sort.value
  const direction = dir.value === 'desc' ? -1 : 1
  const arr = [...filtered.value]
  function val(u) {
    if (k === 'display_name') return String(u?.display_name || '').toLowerCase()
    if (k === 'email') return String(u?.email || '').toLowerCase()
    return String(u?.username || '').toLowerCase()
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
const visibleUsers = computed(() => {
  const p = Math.min(Math.max(1, page.value), totalPages.value)
  const start = (p - 1) * pageSize
  return sorted.value.slice(start, start + pageSize)
})

watch(totalPages, (n) => {
  if (page.value > n) page.value = n
})

function updateQuery() {
  router.replace({
    path: route.path,
    query: {
      q: q.value || '',
      sort: sort.value || 'username',
      dir: dir.value || 'asc',
      page: String(page.value || 1)
    }
  })
}
watch(() => route.query, () => {
  const rq = route.query
  q.value = typeof rq.q === 'string' ? rq.q : q.value
  sort.value = typeof rq.sort === 'string' ? rq.sort : sort.value
  dir.value = typeof rq.dir === 'string' ? rq.dir : dir.value
  page.value = Number.parseInt(rq.page) || page.value
})

function onSearch() {
  page.value = 1
  updateQuery()
}
function toggleSort(key) {
  if (sort.value === key) {
    dir.value = dir.value === 'asc' ? 'desc' : 'asc'
  } else {
    sort.value = key
    dir.value = 'asc'
  }
  updateQuery()
}
function goFirst() { if (page.value > 1) { page.value = 1; updateQuery() } }
function goPrev() { if (page.value > 1) { page.value -= 1; updateQuery() } }
function goNext() { if (page.value < totalPages.value) { page.value += 1; updateQuery() } }
function goLast() { if (page.value < totalPages.value) { page.value = totalPages.value; updateQuery() } }

async function onDelete(id) {
  if (!confirm('Delete this user?')) return
  try {
    await api.delete(`/users/${id}`)
    users.value = users.value.filter(u => u.id !== id)
    successMessage.value = 'User deleted.'
    if (visibleUsers.value.length === 0 && page.value > 1) {
      page.value = page.value - 1
      updateQuery()
    }
  } catch (e) {
    errorMessage.value = 'Failed to delete user.'
  }
}
</script>