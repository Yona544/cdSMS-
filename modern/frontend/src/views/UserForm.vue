<template>
  <div class="legacy-scope">
    <h1>User</h1>
    <div class="gridtable p-4">
      <form @submit.prevent="save">
        <div class="mb-3">
          <label class="mr-2">Username</label>
          <input v-model="form.username" class="inp-form" />
        </div>
        <div class="mb-3">
          <label class="mr-2">Display Name</label>
          <input v-model="form.display_name" class="inp-form" />
        </div>
        <div class="mb-3">
          <label class="mr-2">Email</label>
          <input v-model="form.email" class="inp-form" />
        </div>
        <div class="mb-3">
          <label class="mr-2">Admin</label>
          <input type="checkbox" v-model="form.is_admin" />
        </div>
        <div class="mb-3">
          <label class="mr-2">Tags</label>
          <!-- Tokenized TagInput (page-local) -->
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
          <div class="text-xs text-gray-500 mt-1">Press Enter or comma to add. Saved as comma-delimited.</div>
        </div>
        <button class="submit-login" type="submit">Save</button>
      </form>
    </div>
  </div>
</template>

<script setup>
import { reactive, onMounted, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { api } from '@/utils/api'

const route = useRoute()
const router = useRouter()

const form = reactive({ username: '', display_name: '', email: '', is_admin: false, tags: '' })
const tagTokens = ref([])
const tagDraft = ref('')
const tagInp = ref(null)
function syncTokensFromForm() {
  const arr = (form.tags || '')
    .split(',')
    .map(s => s.trim())
    .filter(Boolean)
  tagTokens.value = arr
}
function syncFormFromTokens() {
  form.tags = tagTokens.value.join(', ')
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

onMounted(async () => {
  if (route.params.id) {
    try {
      const res = await api.get(`/users/${route.params.id}`)
      const d = res.data?.data || {}
      form.username = d.username || ''
      form.display_name = d.display_name || ''
      form.email = d.email || ''
      form.is_admin = d.is_admin ?? false
      form.tags = Array.isArray(d.tags) ? d.tags.join(', ') : (d.tags ?? '')
      syncTokensFromForm()
    } catch {
      // ignore for stub
    }
  }
})

async function save() {
  try {
    if (route.params.id) {
      await api.put(`/users/${route.params.id}`, form)
    } else {
      await api.post(`/users`, form)
    }
    // redirect with success flag for banner on list page
    router.push({ path: '/users', query: { saved: '1' } })
  } catch {
    // ignore for stub
    router.push('/users')
  }
}
</script>