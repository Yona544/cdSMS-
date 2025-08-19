<template>
  <div class="legacy-scope">
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-2xl font-bold">Voice Messages</h1>
      <BaseButton @click="openCreateModal">Create New</BaseButton>
    </div>

    <!-- Legacy-style Search -->
    <div class="mb-4">
      <form class="flex items-center gap-3" @submit.prevent="onSearch">
        <input class="inp-form" v-model="q" placeholder="Search voice messages..." />
        <button type="submit" class="h-[31px] flex items-center">
          <img src="/legacy/admin/images/top_search_btn.gif" alt="Search" />
        </button>
      </form>
    </div>

    <div v-if="voiceStore.loading" class="text-center">Loading...</div>
    <div v-if="voiceStore.error" class="text-center message-red">{{ voiceStore.error }}</div>
    <div v-if="successMessage" class="text-center message-green">{{ successMessage }}</div>

    <div v-if="!voiceStore.loading && visibleMessages.length" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <VoiceMessageCard
        v-for="message in visibleMessages"
        :key="message.id"
        :message="message"
        @edit="openEditModal"
        @duplicate="handleDuplicate"
        @delete="handleDelete"
      />
    </div>
    <div v-if="totalPages > 1" class="flex items-center gap-2 mt-4">
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

    <div v-if="!voiceStore.loading && !visibleMessages.length" class="text-center text-gray-500">
      No voice messages found<span v-if="q"> for current search</span>.
    </div>

    <VoiceMessageForm
      :is-open="isModalOpen"
      :message="currentMessage"
      @close="closeModal"
      @save="handleSave"
    />
  </div>
</template>

<script setup>
import { ref, onMounted, computed, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useVoiceStore } from '@/stores/voice'
import VoiceMessageCard from '@/components/voice/VoiceMessageCard.vue'
import VoiceMessageForm from '@/components/voice/VoiceMessageForm.vue'
import BaseButton from '@/components/common/BaseButton.vue'

const voiceStore = useVoiceStore()

const isModalOpen = ref(false)
const currentMessage = ref(null)
const successMessage = ref('')

const route = useRoute()
const router = useRouter()

// Search + pagination state
const q = ref(typeof route.query.q === 'string' ? route.query.q : '')
const page = ref(Number.parseInt(route.query.page) || 1)
const pageSize = 12

onMounted(() => {
  voiceStore.fetchMessages()
})

function openCreateModal() {
  currentMessage.value = null
  isModalOpen.value = true
}

function openEditModal(message) {
  currentMessage.value = message
  isModalOpen.value = true
}

function closeModal() {
  isModalOpen.value = false
  currentMessage.value = null
}

async function handleSave(messageData) {
  if (messageData.id) {
    await voiceStore.updateMessage(messageData.id, messageData)
  } else {
    await voiceStore.createMessage(messageData)
  }
  successMessage.value = 'Message saved.'
  isModalOpen.value = false
}

async function handleDelete(messageId) {
  if (confirm('Are you sure you want to delete this message?')) {
    await voiceStore.deleteMessage(messageId)
    successMessage.value = 'Message deleted.'
    if (visibleMessages.value.length === 0 && page.value > 1) {
      page.value = page.value - 1
      updateQuery()
    }
  }
}

async function handleDuplicate(messageId) {
  await voiceStore.duplicateMessage(messageId)
  successMessage.value = 'Message duplicated.'
}

// Helpers
function includesCI(hay, needle) {
  if (!needle) return true
  if (!hay) return false
  return String(hay).toLowerCase().includes(String(needle).toLowerCase())
}

const filtered = computed(() => {
  const arr = voiceStore.messages || []
  return arr.filter(m => {
    const name = m?.name || m?.title || m?.messageName || ''
    return includesCI(name, q.value)
  })
})

const totalPages = computed(() => Math.max(1, Math.ceil(filtered.value.length / pageSize)))
const visibleMessages = computed(() => {
  const p = Math.min(Math.max(1, page.value), totalPages.value)
  const start = (p - 1) * pageSize
  return filtered.value.slice(start, start + pageSize)
})

// Keep page within bounds
watch(totalPages, (n) => {
  if (page.value > n) page.value = n
})

// Route sync
function updateQuery() {
  router.replace({
    path: route.path,
    query: {
      q: q.value || '',
      page: String(page.value || 1)
    }
  })
}
function syncFromRoute() {
  const rq = route.query
  q.value = typeof rq.q === 'string' ? rq.q : q.value
  page.value = Number.parseInt(rq.page) || page.value
}
watch(() => route.query, () => syncFromRoute())

// UI actions
function onSearch() {
  page.value = 1
  updateQuery()
}
function goFirst() { if (page.value > 1) { page.value = 1; updateQuery() } }
function goPrev() { if (page.value > 1) { page.value -= 1; updateQuery() } }
function goNext() { if (page.value < totalPages.value) { page.value += 1; updateQuery() } }
function goLast() { if (page.value < totalPages.value) { page.value = totalPages.value; updateQuery() } }
</script>
