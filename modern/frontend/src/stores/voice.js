import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import apiClient from '@/utils/api'
import { useTenantStore } from './tenant'

export const useVoiceStore = defineStore('voice', () => {
  const tenantStore = useTenantStore()

  // State
  const messages = ref([])
  const loading = ref(false)
  const error = ref(null)

  // Getters
  const activeMessages = computed(() =>
    messages.value.filter(msg => msg.is_active)
  )
  const messageCount = computed(() => messages.value.length)

  const getHeaders = () => {
    return { 'X-API-Key': tenantStore.apiKey }
  }

  // Actions
  const fetchMessages = async (page = 1, per_page = 20) => {
    loading.value = true
    error.value = null
    try {
      const response = await apiClient.get('/voice-messages', {
        params: { page, per_page },
        headers: getHeaders(),
      })
      messages.value = response.data.data
      // In a real app, you'd also store the pagination info
      return response.data
    } catch (err) {
      error.value = err.response?.data?.error?.message || 'Failed to fetch messages'
    } finally {
      loading.value = false
    }
  }

  const createMessage = async (messageData) => {
    loading.value = true
    try {
      const response = await apiClient.post('/voice-messages', messageData, { headers: getHeaders() })
      await fetchMessages() // Refetch the list to include the new one
      return response.data
    } catch (err) {
      error.value = err.response?.data?.error?.message || 'Failed to create message'
      throw err
    } finally {
      loading.value = false
    }
  }

  const updateMessage = async (id, messageData) => {
    loading.value = true
    try {
      const response = await apiClient.put(`/voice-messages/${id}`, messageData, { headers: getHeaders() })
      const index = messages.value.findIndex(msg => msg.id === id)
      if (index !== -1) {
        messages.value[index] = response.data.data
      }
      return response.data
    } catch (err) {
      error.value = err.response?.data?.error?.message || 'Failed to update message'
      throw err
    } finally {
      loading.value = false
    }
  }

  const deleteMessage = async (id) => {
    loading.value = true
    try {
      await apiClient.delete(`/voice-messages/${id}`, { headers: getHeaders() })
      messages.value = messages.value.filter(msg => msg.id !== id)
    } catch (err) {
      error.value = err.response?.data?.error?.message || 'Failed to delete message'
      throw err
    } finally {
      loading.value = false
    }
  }

  const duplicateMessage = async (id) => {
    loading.value = true
    try {
      await apiClient.post(`/voice-messages/${id}/duplicate`, {}, { headers: getHeaders() })
      await fetchMessages() // Refetch to show the duplicated message
    } catch (err) {
      error.value = err.response?.data?.error?.message || 'Failed to duplicate message'
      throw err
    } finally {
      loading.value = false
    }
  }

  return {
    messages,
    loading,
    error,
    activeMessages,
    messageCount,
    fetchMessages,
    createMessage,
    updateMessage,
    deleteMessage,
    duplicateMessage,
  }
})
