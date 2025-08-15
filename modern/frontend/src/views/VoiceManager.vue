<template>
  <div>
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-2xl font-bold">Voice Messages</h1>
      <BaseButton @click="openCreateModal">Create New</BaseButton>
    </div>

    <div v-if="voiceStore.loading" class="text-center">Loading...</div>
    <div v-if="voiceStore.error" class="text-center text-red-500">{{ voiceStore.error }}</div>

    <div v-if="!voiceStore.loading && voiceStore.messages.length" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <VoiceMessageCard
        v-for="message in voiceStore.messages"
        :key="message.id"
        :message="message"
        @edit="openEditModal"
        @duplicate="handleDuplicate"
        @delete="handleDelete"
      />
    </div>

    <div v-if="!voiceStore.loading && !voiceStore.messages.length" class="text-center text-gray-500">
      No voice messages found.
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
import { ref, onMounted } from 'vue'
import { useVoiceStore } from '@/stores/voice'
import VoiceMessageCard from '@/components/voice/VoiceMessageCard.vue'
import VoiceMessageForm from '@/components/voice/VoiceMessageForm.vue'
import BaseButton from '@/components/common/BaseButton.vue'

const voiceStore = useVoiceStore()

const isModalOpen = ref(false)
const currentMessage = ref(null)

onMounted(() => {
  // We need to make sure the tenant info is loaded first
  // This would be handled by a main app setup logic, for now we assume it's there
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
    // Update existing message
    await voiceStore.updateMessage(messageData.id, messageData)
  } else {
    // Create new message
    await voiceStore.createMessage(messageData)
  }
}

async function handleDelete(messageId) {
  if (confirm('Are you sure you want to delete this message?')) {
    await voiceStore.deleteMessage(messageId)
  }
}

async function handleDuplicate(messageId) {
  await voiceStore.duplicateMessage(messageId)
}
</script>
