<template>
  <form @submit.prevent="handleSubmit" class="space-y-4">
    <div>
      <label for="to_number" class="block text-sm font-medium text-gray-700">To Number</label>
      <input v-model="form.to_number" type="text" id="to_number" placeholder="+1234567890" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" required>
    </div>
    <div>
      <label for="message" class="block text-sm font-medium text-gray-700">Message</label>
      <textarea v-model="form.message" id="message" rows="4" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" required></textarea>
      <p class="mt-2 text-sm text-gray-500">You can use variables like {name} or {appointment_date}.</p>
    </div>
    <div class="flex justify-end">
      <BaseButton type="submit" :disabled="loading">
        {{ loading ? 'Sending...' : 'Send SMS' }}
      </BaseButton>
    </div>
  </form>
</template>

<script setup>
import { ref } from 'vue'
import BaseButton from '../common/BaseButton.vue'

defineProps({
  loading: Boolean,
})

const emit = defineEmits(['submit'])

const form = ref({
  to_number: '',
  message: '',
})

function handleSubmit() {
  emit('submit', { ...form.value })
}
</script>
