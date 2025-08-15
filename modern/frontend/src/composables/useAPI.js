import { ref } from 'vue'
import apiClient from '@/utils/api'

export function useAPI() {
  const loading = ref(false)
  const error = ref(null)

  // This composable will be expanded later.
  // For now, it just provides access to the api client.

  return {
    loading,
    error,
    api: apiClient,
  }
}
