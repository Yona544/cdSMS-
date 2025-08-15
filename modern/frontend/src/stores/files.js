import { defineStore } from 'pinia'
import { ref } from 'vue'
import apiClient from '@/utils/api'
import { useTenantStore } from './tenant'

export const useFileStore = defineStore('files', () => {
  const tenantStore = useTenantStore()

  // State
  const files = ref([])
  const loading = ref(false)
  const error = ref(null)

  const getHeaders = (isFormData = false) => {
    const headers = { 'X-API-Key': tenantStore.apiKey }
    if (!isFormData) {
      headers['Content-Type'] = 'application/json'
    }
    return headers
  }

  // Actions
  const fetchFiles = async (page = 1, per_page = 20) => {
    loading.value = true
    error.value = null
    try {
      const response = await apiClient.get('/files', {
        params: { page, per_page },
        headers: getHeaders(),
      })
      files.value = response.data.data
      return response.data
    } catch (err) {
      error.value = err.response?.data?.error?.message || 'Failed to fetch files'
    } finally {
      loading.value = false
    }
  }

  const uploadFile = async (fileData) => {
    loading.value = true
    const formData = new FormData()
    formData.append('file', fileData)

    try {
      await apiClient.post('/files/upload', formData, {
        headers: getHeaders(true),
      })
      await fetchFiles() // Refetch to include the new file
    } catch (err) {
      error.value = err.response?.data?.error?.message || 'Failed to upload file'
      throw err
    } finally {
      loading.value = false
    }
  }

  const updateFile = async (id, fileMetadata) => {
    loading.value = true
    try {
      await apiClient.put(`/files/${id}`, fileMetadata, { headers: getHeaders() })
      await fetchFiles() // Refetch to get updated data
    } catch (err) {
      error.value = err.response?.data?.error?.message || 'Failed to update file metadata'
      throw err
    } finally {
      loading.value = false
    }
  }

  const deleteFile = async (id) => {
    loading.value = true
    try {
      await apiClient.delete(`/files/${id}`, { headers: getHeaders() })
      files.value = files.value.filter(file => file.id !== id)
    } catch (err) {
      error.value = err.response?.data?.error?.message || 'Failed to delete file'
      throw err
    } finally {
      loading.value = false
    }
  }

  return {
    files,
    loading,
    error,
    fetchFiles,
    uploadFile,
    updateFile,
    deleteFile,
  }
})
