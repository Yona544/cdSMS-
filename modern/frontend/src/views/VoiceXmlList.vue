<template>
  <div class="legacy-scope">
    <h1>Voice XML</h1>

    <!-- Actions + Search -->
    <div class="mb-4 flex items-center justify-between">
      <form class="flex items-center gap-3" @submit.prevent="onSearch">
        <input class="inp-form" v-model="q" placeholder="Search filename..." />
        <button type="submit" class="h-[31px] flex items-center">
          <img src="/legacy/admin/images/top_search_btn.gif" alt="Search" />
        </button>
      </form>
      <router-link to="/voice-xml/new" class="legacy-btn" title="New" style="display:inline-flex;align-items:center;border:1px solid #dbdbdb;background:#fff;padding:4px 8px;">
        <img src="/legacy/admin/images/Submit.gif" alt="New" class="mr-1" style="margin-right:4px;" />
        <span>New</span>
      </router-link>
    </div>

    <div class="gridtable">
      <div class="tblheader px-3">XML Entries</div>
      <table class="w-full">
        <thead>
          <tr class="tblrow">
            <th class="text-left p-2">ID</th>
            <th class="text-left p-2">Filename</th>
            <th class="text-left p-2">Active</th>
            <th class="text-left p-2">Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="x in visibleItems" :key="x.id" class="tblrow">
            <td class="p-2">{{ x.id }}</td>
            <td class="p-2">{{ x.xml_filename }}</td>
            <td class="p-2">{{ x.is_active ? 'Yes' : 'No' }}</td>
            <td class="p-2">
              <router-link :to="`/voice-xml/${x.id}`" class="mr-3">
                <img src="/legacy/admin/images/action_edit.gif" alt="Edit" class="inline-block mr-1" /> Edit
              </router-link>
              <button type="button" @click="onDelete(x.id)" class="inline-flex items-center" title="Delete">
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
const items = ref([])
const q = ref(typeof route.query.q === 'string' ? route.query.q : '')
const page = ref(Number.parseInt(route.query.page) || 1)
const pageSize = 20
const successMessage = ref('')
const errorMessage = ref('')

onMounted(async () => {
  try {
    const res = await api.get('/voice-xml')
    items.value = res.data?.data || []
  } catch {
    items.value = []
  }
})

const filtered = computed(() => {
  const arr = items.value || []
  return arr.filter(x => {
    const name = x?.xml_filename || x?.filename || ''
    return String(name).toLowerCase().includes(String(q.value).toLowerCase())
  })
})

const totalPages = computed(() => Math.max(1, Math.ceil(filtered.value.length / pageSize)))
const visibleItems = computed(() => {
  const p = Math.min(Math.max(1, page.value), totalPages.value)
  const start = (p - 1) * pageSize
  return filtered.value.slice(start, start + pageSize)
})

watch(totalPages, (n) => {
  if (page.value > n) page.value = n
})

function updateQuery() {
  router.replace({
    path: route.path,
    query: {
      q: q.value || '',
      page: String(page.value || 1)
    }
  })
}
watch(() => route.query, () => {
  const rq = route.query
  q.value = typeof rq.q === 'string' ? rq.q : q.value
  page.value = Number.parseInt(rq.page) || page.value
})

function onSearch() {
  page.value = 1
  updateQuery()
}

async function onDelete(id) {
  if (!confirm('Delete this XML entry?')) return
  try {
    await api.delete(`/voice-xml/${id}`)
    items.value = items.value.filter(i => i.id !== id)
    successMessage.value = 'Entry deleted.'
    if (visibleItems.value.length === 0 && page.value > 1) {
      page.value = page.value - 1
      updateQuery()
    }
  } catch (e) {
    errorMessage.value = 'Failed to delete entry.'
  }
}

function goFirst() { if (page.value > 1) { page.value = 1; updateQuery() } }
function goPrev() { if (page.value > 1) { page.value -= 1; updateQuery() } }
function goNext() { if (page.value < totalPages.value) { page.value += 1; updateQuery() } }
function goLast() { if (page.value < totalPages.value) { page.value = totalPages.value; updateQuery() } }
</script>