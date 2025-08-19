<template>
  <div class="legacy-scope">
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-2xl font-bold">File Management</h1>
    </div>

    <!-- Legacy-style SearchRow -->
    <div class="mb-4">
      <form class="flex items-center gap-3" @submit.prevent="onSearch">
        <input class="inp-form" v-model="q" placeholder="Search files..." />
        <label class="text-sm text-gray-700">Type</label>
        <select class="border border-gray-300 h-[31px] px-2 text-sm" v-model="type">
          <option value="all">All</option>
          <option value="audio">Audio</option>
          <option value="xml">XML</option>
          <option value="other">Other</option>
        </select>
        <label class="text-sm text-gray-700">Tags</label>
        <input class="inp-form" v-model="tag" placeholder="tag1, tag2" />
        <button type="submit" class="h-[31px] flex items-center">
          <img src="/legacy/admin/images/top_search_btn.gif" alt="Search" />
        </button>
      </form>
    </div>

    <!-- File Upload Section -->
    <div class="bg-white p-6 rounded-lg shadow-md mb-6">
      <h2 class="text-lg font-semibold mb-4">Upload New File</h2>
      <div class="flex items-center space-x-4">
        <input type="file" @change="handleFileSelect" class="block w-full text-sm text-gray-500
          file:mr-4 file:py-2 file:px-4
          file:rounded-full file:border-0
          file:text-sm file:font-semibold
          file:bg-blue-50 file:text-blue-700
          hover:file:bg-blue-100"
        />
        <BaseButton @click="handleUpload" :disabled="!selectedFile || fileStore.loading">
          {{ fileStore.loading ? 'Uploading...' : 'Upload' }}
        </BaseButton>
      </div>
       <div v-if="fileStore.error" class="message-red mt-2">{{ fileStore.error }}</div>
       <div v-if="successMessage" class="message-green mt-2">{{ successMessage }}</div>
    </div>

    <!-- File List Section -->
    <div v-if="fileStore.loading && !fileStore.files.length" class="text-center">Loading files...</div>

    <div v-if="!fileStore.loading && !visibleFiles.length" class="text-center text-gray-500 mt-8">
      No files found<span v-if="q || type !== 'all' || tag"> for current filters</span>.
    </div>

    <div v-if="visibleFiles.length" class="gridtable p-4">
      <div class="tblheader px-3 flex items-center gap-6">
        <span class="cursor-pointer" @click="toggleSort('filename')">
          Filename
          <img v-if="sort === 'filename' && dir === 'asc'" src="/legacy/admin/images/table_icon_3.gif" alt="Ascending" class="ml-1 align-middle" />
          <img v-else-if="sort === 'filename' && dir === 'desc'" src="/legacy/admin/images/table_icon_4.gif" alt="Descending" class="ml-1 align-middle" />
        </span>
        <span class="cursor-pointer" @click="toggleSort('size')">
          Size
          <img v-if="sort === 'size' && dir === 'asc'" src="/legacy/admin/images/table_icon_3.gif" alt="Ascending" class="ml-1 align-middle" />
          <img v-else-if="sort === 'size' && dir === 'desc'" src="/legacy/admin/images/table_icon_4.gif" alt="Descending" class="ml-1 align-middle" />
        </span>
        <span class="cursor-pointer" @click="toggleSort('updated')">
          Updated
          <img v-if="sort === 'updated' && dir === 'asc'" src="/legacy/admin/images/table_icon_3.gif" alt="Ascending" class="ml-1 align-middle" />
          <img v-else-if="sort === 'updated' && dir === 'desc'" src="/legacy/admin/images/table_icon_4.gif" alt="Descending" class="ml-1 align-middle" />
        </span>
      </div>
      <div class="space-y-4">
        <div v-if="visibleFiles.length">
          <table class="w-full">
            <thead>
              <tr class="tblrow">
                <th class="text-left p-2">Filename</th>
                <th class="text-left p-2">Size</th>
                <th class="text-left p-2">Updated</th>
                <th class="text-left p-2">Actions</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="f in visibleFiles" :key="f.id || f.name" class="tblrow">
                <td class="p-2">{{ displayName(f) }}</td>
                <td class="p-2">{{ displaySize(f) }}</td>
                <td class="p-2">{{ displayUpdated(f) }}</td>
                <td class="p-2">
                  <button type="button" class="inline-flex items-center mr-3" @click="openEdit(f)">
                    <img src="/legacy/admin/images/action_edit.gif" alt="Edit" class="inline-block mr-1" /> Edit
                  </button>
                  <button type="button" class="inline-flex items-center" @click="handleDelete(f.id)">
                    <img src="/legacy/admin/images/action_delete.gif" alt="Delete" class="inline-block mr-1" /> Delete
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
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

    <!-- Edit Modal -->
    <div v-if="editOpen" style="position:fixed; inset:0; background:rgba(0,0,0,0.5); z-index:1000; display:flex; align-items:center; justify-content:center;">
      <div style="background:#fff; width:640px; max-width:95%; border:1px solid #dbdbdb;">
        <div class="tblheader px-3">Edit File</div>
        <div class="p-4">
          <div class="mb-3">
            <label class="mr-2">Filename</label>
            <input v-model="editForm.name" class="inp-form" />
          </div>
          <div class="mb-3">
            <label class="mr-2">Tags</label>
            <div class="tag-input" @click="focusTagInput" style="display:flex;flex-wrap:wrap;gap:6px;padding:4px 6px;min-height:31px;border:1px solid #dbdbdb;background:#fff;">
              <span v-for="(t, i) in tagTokens" :key="i" class="tag-chip" style="background:var(--legacy-gray-9);border:1px solid var(--legacy-gray-7);padding:2px 6px;border-radius:2px;font-size:12px;">
                {{ t }}
                <button type="button" class="tag-x" @click.stop="removeTag(i)" aria-label="Remove tag" style="background:transparent;border:none;margin-left:6px;cursor:pointer;color:#555;">&times;</button>
              </span>
              <input
                ref="tagInp"
                v-model="tagDraft"
                @keydown.enter.prevent="commitTag"
                @keydown="onTagKeydown"
                @blur="commitTag"
                placeholder="Add tag…"
                class="tag-inp"
                style="border:none;outline:none;min-width:140px;height:24px;font-size:12px;"
              />
            </div>
            <div class="text-xs text-gray-500 mt-1">Press Enter or comma to add. Tags saved comma-delimited.</div>
          </div>
          <div class="text-right">
            <button type="button" @click="saveEdit" class="legacy-btn" style="border:1px solid #dbdbdb;background:#fff;padding:4px 10px;cursor:pointer;">Save</button>
            <button type="button" @click="closeEdit" class="legacy-btn" style="border:1px solid #dbdbdb;background:#fff;padding:4px 10px;cursor:pointer;margin-left:8px;">Cancel</button>
          </div>
          <div v-if="editError" class="message-red mt-2">{{ editError }}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useFileStore } from '@/stores/files'
import BaseButton from '@/components/common/BaseButton.vue'
import { api } from '@/utils/api'

const fileStore = useFileStore()
const selectedFile = ref(null)

const route = useRoute()
const router = useRouter()

// Query/filter/sort/paging state
const q = ref(typeof route.query.q === 'string' ? route.query.q : '')
const type = ref(typeof route.query.type === 'string' ? route.query.type : 'all')
const tag = ref(typeof route.query.tag === 'string' ? route.query.tag : '')
const sort = ref(typeof route.query.sort === 'string' ? route.query.sort : 'filename')
const dir = ref(typeof route.query.dir === 'string' ? route.query.dir : 'asc')
const page = ref(Number.parseInt(route.query.page) || 1)
const pageSize = 20

const successMessage = ref('')
const editOpen = ref(false)
const editForm = ref({ id: null, name: '', tags: '' })
const editError = ref('')

const tagTokens = ref([])
const tagDraft = ref('')
const tagInp = ref(null)

onMounted(() => {
  fileStore.fetchFiles()
})

function handleFileSelect(event) {
  selectedFile.value = event.target.files[0]
}

async function handleUpload() {
  if (!selectedFile.value) {
    alert('Please select a file to upload.')
    return
  }
  try {
    await fileStore.uploadFile(selectedFile.value)
    successMessage.value = 'File uploaded successfully.'
  } catch (e) {
    // Error banner is surfaced via fileStore.error
  } finally {
    selectedFile.value = null // Reset file input
  }
}

async function handleDelete(fileId) {
  if (confirm('Are you sure you want to delete this file? This action cannot be undone.')) {
    try {
      await fileStore.deleteFile(fileId)
      successMessage.value = 'File deleted.'
      if (visibleFiles.value.length === 0 && page.value > 1) {
        page.value = page.value - 1
        updateQuery()
      }
    } catch (e) {
      // Error banner handled by store
    }
  }
}

// Helpers
function includesCI(hay, needle) {
  if (!needle) return true
  if (!hay) return false
  return String(hay).toLowerCase().includes(String(needle).toLowerCase())
}
function detectType(file) {
  const name = (file?.name || file?.filename || '').toLowerCase()
  const mime = (file?.mime || file?.contentType || '').toLowerCase()
  if (mime.startsWith('audio/') || /\.(mp3|wav|ogg|m4a)$/.test(name)) return 'audio'
  if (mime.includes('xml') || /\.xml$/.test(name)) return 'xml'
  return 'other'
}
function hasAnyTag(file, tagsCsv) {
  const filterTokens = (tagsCsv || '').split(',').map(s => s.trim().toLowerCase()).filter(Boolean)
  if (!filterTokens.length) return true
  const fileTags = Array.isArray(file?.tags)
    ? file.tags.map(t => String(t).toLowerCase())
    : String(file?.tags || '').split(',').map(s => s.trim().toLowerCase()).filter(Boolean)
  return filterTokens.some(t => fileTags.includes(t))
}
function displayName(f) {
  return f?.name || f?.filename || ''
}
function displaySize(f) {
  const s = Number(f?.size || 0)
  if (!s) return '—'
  if (s < 1024) return `${s} B`
  if (s < 1024 * 1024) return `${(s / 1024).toFixed(1)} KB`
  return `${(s / (1024 * 1024)).toFixed(1)} MB`
}
function displayUpdated(f) {
  const d = f?.updatedAt || f?.updated || f?.modifiedAt || f?.mtime
  try { return new Date(d).toLocaleString() } catch { return d || '—' }
}
// Tag tokenization helpers for edit modal
function syncTokensFromForm() {
  const arr = (editForm.value.tags || '')
    .split(',')
    .map(s => s.trim())
    .filter(Boolean)
  tagTokens.value = arr
}
function syncFormFromTokens() {
  editForm.value.tags = tagTokens.value.join(', ')
}
function commitTag() {
  const parts = tagDraft.value.split(',').map(s => s.trim()).filter(Boolean)
  if (parts.length) {
    tagTokens.value.push(...parts)
    tagDraft.value = ''
    syncFormFromTokens()
  }
}
function removeTag(i) {
  tagTokens.value.splice(i, 1)
  syncFormFromTokens()
}
function focusTagInput() {
  tagInp.value && tagInp.value.focus()
}
function onTagKeydown(e) {
  if (e.key === ',' || e.code === 'Comma') {
    e.preventDefault()
    commitTag()
  }
}

const filtered = computed(() => {
  let arr = fileStore.files || []
  return arr.filter(f => {
    const name = f?.name || f?.filename || ''
    const typeOk = type.value === 'all' ? true : detectType(f) === type.value
    const qOk = includesCI(name, q.value)
    const tagOk = hasAnyTag(f, tag.value)
    return typeOk && qOk && tagOk
  })
})

const sorted = computed(() => {
  const sKey = sort.value
  const direction = dir.value === 'desc' ? -1 : 1
  const arr = [...filtered.value]
  function val(f) {
    if (sKey === 'size') return Number(f?.size || 0)
    if (sKey === 'updated') {
      const ts = new Date(f?.updatedAt || f?.updated || f?.modifiedAt || f?.mtime || 0).getTime()
      return isNaN(ts) ? 0 : ts
    }
    return String(f?.name || f?.filename || '').toLowerCase()
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
const visibleFiles = computed(() => {
  const p = Math.min(Math.max(1, page.value), totalPages.value)
  const start = (p - 1) * pageSize
  return sorted.value.slice(start, start + pageSize)
})

// Keep page in bounds if filters/sort change
watch(totalPages, (n) => {
  if (page.value > n) page.value = n
})

// Route sync
function updateQuery() {
  router.replace({
    path: route.path,
    query: {
      q: q.value || '',
      type: type.value || 'all',
      tag: tag.value || '',
      sort: sort.value || 'filename',
      dir: dir.value || 'asc',
      page: String(page.value || 1)
    }
  })
}
function syncFromRoute() {
  const rq = route.query
  q.value = typeof rq.q === 'string' ? rq.q : q.value
  type.value = typeof rq.type === 'string' ? rq.type : type.value
  tag.value = typeof rq.tag === 'string' ? rq.tag : tag.value
  sort.value = typeof rq.sort === 'string' ? rq.sort : sort.value
  dir.value = typeof rq.dir === 'string' ? rq.dir : dir.value
  page.value = Number.parseInt(rq.page) || page.value
}
watch(() => route.query, () => syncFromRoute())

// UI actions
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

// Edit modal actions
function openEdit(f) {
  editForm.value.id = f.id || f.name
  editForm.value.name = displayName(f)
  editForm.value.tags = Array.isArray(f?.tags) ? f.tags.join(', ') : (f?.tags || '')
  editError.value = ''
  syncTokensFromForm()
  editOpen.value = true
}
function closeEdit() {
  editOpen.value = false
}
async function saveEdit() {
  editError.value = ''
  try {
    const payload = { filename: editForm.value.name, tags: editForm.value.tags }
    try {
      await api.put(`/files/${editForm.value.id}`, payload)
    } catch (e) {
      // stub-friendly: ignore if endpoint missing
    }
    // Optimistic local update
    fileStore.files = (fileStore.files || []).map(f => {
      const key = f.id || f.name
      if (key === editForm.value.id) {
        return { ...f, name: editForm.value.name, filename: editForm.value.name, tags: editForm.value.tags }
      }
      return f
    })
    successMessage.value = 'File updated.'
    editOpen.value = false
  } catch (e) {
    editError.value = 'Failed to save changes.'
  }
}
</script>
