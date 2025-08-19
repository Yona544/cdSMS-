<template>
  <div class="legacy-scope">
    <h1>Voice XML Editor</h1>
    <div class="gridtable p-4">
      <form @submit.prevent="save">
        <div class="mb-3">
          <label class="mr-2">Filename</label>
          <input v-model="form.xml_filename" class="inp-form" />
        </div>
        <div class="mb-3">
          <label class="mr-2">Content</label>
          <textarea v-model="form.xml_content" class="inp-form" style="height:120px;width:380px;"></textarea>
        </div>
        <div class="mb-3">
          <label class="mr-2">Active</label>
          <input type="checkbox" v-model="form.is_active" />
        </div>
        <button class="submit-login" type="submit">Save</button>
        <button type="button" class="ml-3" @click="preview">Preview TwiML</button>
        <div v-if="previewError" class="message-red mt-2">{{ previewError }}</div>
      </form>
    </div>

    <!-- Preview Modal -->
    <div v-if="showPreview" style="position:fixed; inset:0; background:rgba(0,0,0,0.5); z-index:1000; display:flex; align-items:center; justify-content:center;">
      <div style="background:#fff; width:600px; max-width:90%; border:1px solid #dbdbdb;">
        <div class="tblheader" style="padding:6px 10px;">TwiML Preview</div>
        <div style="padding:10px;">
          <pre style="max-height:60vh; overflow:auto; background:#f8f8f8; border:1px solid #ececec; padding:8px; font-size:12px; white-space:pre-wrap;">{{ previewTwiML }}</pre>
          <div style="text-align:right; margin-top:10px;">
            <button type="button" @click="showPreview = false" style="border:1px solid #dbdbdb; background:#fff; padding:4px 10px; cursor:pointer;">Close</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { reactive, onMounted, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { api } from '@/utils/api'

const route = useRoute()
const router = useRouter()
const form = reactive({ xml_filename: '', xml_content: '', is_active: true })

onMounted(async () => {
  if (route.params.id) {
    try {
      const res = await api.get(`/voice-xml/${route.params.id}`)
      const d = res.data?.data || {}
      form.xml_filename = d.xml_filename ?? ''
      form.xml_content = d.xml_content ?? ''
      form.is_active = d.is_active ?? true
    } catch {
      // ignore for stub mode
    }
  }
})

const showPreview = ref(false)
const previewTwiML = ref('')
const previewError = ref('')

async function preview() {
  previewError.value = ''
  try {
    // Try backend preview endpoint first (if available)
    const payload = {
      xml_filename: form.xml_filename,
      xml_content: form.xml_content,
      is_active: form.is_active
    }
    let twiml = ''
    try {
      const res = await api.post('/voice-xml/preview', payload)
      twiml = res?.data?.twiml || res?.data?.data?.twiml || (typeof res?.data === 'string' ? res.data : '')
    } catch {
      // Fallback to local content if endpoint is missing
      twiml = (form.xml_content || '').trim()
    }
    if (!twiml) {
      twiml = `<?xml version="1.0" encoding="UTF-8"?>
<Response>
  <Say voice="alice">This is a TwiML preview.</Say>
</Response>`
    }
    previewTwiML.value = twiml
    showPreview.value = true
  } catch (e) {
    previewError.value = 'Unable to render preview. Please check your XML content.'
  }
}

async function save() {
  try {
    if (route.params.id) {
      await api.put(`/voice-xml/${route.params.id}`, form)
    } else {
      await api.post(`/voice-xml`, form)
    }
  } catch {
    // ignore for stub mode
  } finally {
    router.push('/voice-xml')
  }
}
</script>