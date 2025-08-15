<template>
  <div class="bg-white p-4 rounded-lg shadow flex items-center justify-between">
    <div class="flex items-center">
      <!-- Icon can go here -->
      <div class="ml-4">
        <p class="text-sm font-medium text-gray-900">{{ file.filename }}</p>
        <p class="text-sm text-gray-500">{{ formattedSize }} - {{ file.file_type }}</p>
      </div>
    </div>
    <div class="flex space-x-2">
      <a :href="downloadUrl" target="_blank" class="text-sm text-blue-600 hover:text-blue-800">Download</a>
      <button @click="$emit('delete', file.id)" class="text-sm text-red-600 hover:text-red-800">Delete</button>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  file: {
    type: Object,
    required: true,
  },
})

defineEmits(['delete'])

const formattedSize = computed(() => {
  const size = props.file.file_size
  if (size < 1024) return `${size} Bytes`
  if (size < 1024 * 1024) return `${(size / 1024).toFixed(2)} KB`
  return `${(size / (1024 * 1024)).toFixed(2)} MB`
})

const downloadUrl = computed(() => {
  // This assumes the API is served from the same domain
  return `/api/files/${props.file.id}/download`
})
</script>
