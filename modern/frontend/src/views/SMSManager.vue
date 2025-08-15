<template>
  <div>
    <h1 class="text-2xl font-bold mb-6">Send SMS</h1>
    <div class="bg-white p-6 rounded-lg shadow-md max-w-lg mx-auto">
      <SMSForm @submit="handleSendSms" :loading="commStore.loading" />
      <div v-if="commStore.error" class="mt-4 text-center text-red-500">
        {{ commStore.error }}
      </div>
      <div v-if="sent" class="mt-4 text-center text-green-500">
        SMS sent successfully!
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useCommunicationStore } from '@/stores/communication'
import SMSForm from '@/components/sms/SMSForm.vue'

const commStore = useCommunicationStore()
const sent = ref(false)

async function handleSendSms(smsData) {
  sent.value = false
  try {
    await commStore.sendSms(smsData)
    sent.value = true
    // Reset form or clear fields if SMSForm component emits an event for it
  } catch (e) {
    // Error is handled and displayed via the store's error state
  }
}
</script>
