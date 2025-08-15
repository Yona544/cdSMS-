import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { useAuth } from '@/composables/useAuth'
import apiClient from '@/utils/api'

export const useTenantStore = defineStore('tenant', () => {
  // State from URL/auth
  const { apiKey, tenantId } = useAuth()

  // State from API
  const tenantProfile = ref(null)
  const loading = ref(false)
  const error = ref(null)

  // Getters
  const isAuthenticated = computed(() => !!apiKey.value)

  // Actions
  async function fetchTenantProfile() {
    if (!apiKey.value) {
      error.value = 'API Key is missing.'
      return
    }

    loading.value = true
    error.value = null
    try {
      const response = await apiClient.get('/tenant/profile', {
        headers: {
          'X-API-Key': apiKey.value,
        },
      })
      tenantProfile.value = response.data.data
    } catch (err) {
      console.error('Failed to fetch tenant profile:', err)
      error.value = err.response?.data?.error?.message || 'Failed to fetch tenant profile'
      tenantProfile.value = null
    } finally {
      loading.value = false
    }
  }

  return {
    apiKey,
    tenantId,
    tenantProfile,
    loading,
    error,
    isAuthenticated,
    fetchTenantProfile,
  }
})
