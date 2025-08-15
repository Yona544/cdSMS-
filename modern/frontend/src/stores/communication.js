import { defineStore } from 'pinia'
import { ref } from 'vue'
import apiClient from '@/utils/api'
import { useTenantStore } from './tenant'

export const useCommunicationStore = defineStore('communication', () => {
  const tenantStore = useTenantStore()

  // State
  const history = ref([])
  const loading = ref(false)
  const error = ref(null)

  const getHeaders = () => {
    return { 'X-API-Key': tenantStore.apiKey }
  }

  // Actions
  const fetchHistory = async (page = 1, per_page = 20) => {
    loading.value = true
    error.value = null
    try {
      const response = await apiClient.get('/communication/history', {
        params: { page, per_page },
        headers: getHeaders(),
      })
      history.value = response.data.data
      return response.data
    } catch (err) {
      error.value = err.response?.data?.error?.message || 'Failed to fetch history'
    } finally {
      loading.value = false
    }
  }

  const sendSms = async (smsData) => {
    loading.value = true
    try {
      await apiClient.post('/communication/sms/send', smsData, { headers: getHeaders() })
      // Optionally refetch history or show a success notification
    } catch (err) {
      error.value = err.response?.data?.error?.message || 'Failed to send SMS'
      throw err
    } finally {
      loading.value = false
    }
  }

  const sendVoiceCall = async (callData) => {
    loading.value = true
    try {
      await apiClient.post('/communication/voice/send', callData, { headers: getHeaders() })
      // Optionally refetch history or show a success notification
    } catch (err) {
      error.value = err.response?.data?.error?.message || 'Failed to initiate call'
      throw err
    } finally {
      loading.value = false
    }
  }

  return {
    history,
    loading,
    error,
    fetchHistory,
    sendSms,
    sendVoiceCall,
  }
})
