import { ref, onMounted } from 'vue'

export function useAuth() {
  const apiKey = ref(null)
  const tenantId = ref(null)

  const parseApiKey = () => {
    const urlParams = new URLSearchParams(window.location.search)
    const key = urlParams.get('apiKey')
    if (key) {
      apiKey.value = key
    }

    // The tenant ID might also be in the path, e.g. /admin/{tenant_id}
    const pathSegments = window.location.pathname.split('/')
    const adminIndex = pathSegments.indexOf('admin')
    if (adminIndex !== -1 && pathSegments.length > adminIndex + 1) {
      tenantId.value = pathSegments[adminIndex + 1]
    }
  }

  onMounted(() => {
    parseApiKey()
  })

  return {
    apiKey,
    tenantId,
  }
}
