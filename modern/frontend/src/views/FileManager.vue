<template>
  <div>
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-2xl font-bold">File Management</h1>
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
       <div v-if="fileStore.error" class="text-red-500 mt-2">{{ fileStore.error }}</div>
    </div>

    <!-- File List Section -->
    <div v-if="fileStore.loading && !fileStore.files.length" class="text-center">Loading files...</div>

    <div v-if="!fileStore.loading && !fileStore.files.length" class="text-center text-gray-500 mt-8">
      No files found.
    </div>

    <div v-if="fileStore.files.length" class="space-y-4">
      <FileListItem
        v-for="file in fileStore.files"
        :key="file.id"
        :file="file"
        @delete="handleDelete"
      />
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useFileStore } from '@/stores/files'
import FileListItem from '@/components/files/FileListItem.vue'
import BaseButton from '@/components/common/BaseButton.vue'

const fileStore = useFileStore()
const selectedFile = ref(null)

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
  await fileStore.uploadFile(selectedFile.value)
  selectedFile.value = null // Reset file input
}

async function handleDelete(fileId) {
  if (confirm('Are you sure you want to delete this file? This action cannot be undone.')) {
    await fileStore.deleteFile(fileId)
  }
}
</script>
