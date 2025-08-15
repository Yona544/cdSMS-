<template>
  <div class="bg-white rounded-lg shadow-md p-6 hover:shadow-lg transition-shadow">
    <div class="flex justify-between items-start mb-4">
      <h3 class="text-lg font-semibold text-gray-800">{{ message.name }}</h3>
      <div class="flex space-x-2">
        <button @click="$emit('edit', message)" class="text-sm text-blue-600 hover:text-blue-800">Edit</button>
        <button @click="$emit('duplicate', message.id)" class="text-sm text-green-600 hover:text-green-800">Duplicate</button>
        <button @click="$emit('delete', message.id)" class="text-sm text-red-600 hover:text-red-800">Delete</button>
      </div>
    </div>

    <p class="text-gray-600 mb-4">{{ truncatedText }}</p>

    <div class="flex justify-between items-center text-sm text-gray-500">
      <span>Voice: {{ message.voice_gender }}</span>
      <span>Rate: {{ message.voice_rate }}</span>
      <span>{{ formattedDate }}</span>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  message: {
    type: Object,
    required: true
  }
})

defineEmits(['edit', 'duplicate', 'delete'])

const truncatedText = computed(() => {
  const text = props.message.voice_text || ''
  return text.length > 100 ? text.substring(0, 100) + '...' : text
})

const formattedDate = computed(() => {
  if (!props.message.created_at) return ''
  return new Date(props.message.created_at).toLocaleDateString()
})
</script>
