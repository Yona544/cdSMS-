import axios from 'axios'

export const api = axios.create({
  baseURL: '/api',
})

api.interceptors.request.use((config) => {
  const key = localStorage.getItem('apiKey')
  if (key) {
    config.headers['X-API-Key'] = key
  }
  return config
})

export default api
