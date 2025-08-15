import axios from 'axios'

const apiClient = axios.create({
  baseURL: '/api',
  headers: {
    'Content-Type': 'application/json',
  },
})

// We will add an interceptor here later to automatically add the API key.
// For now, we will have to add it manually to each request.

export default apiClient
