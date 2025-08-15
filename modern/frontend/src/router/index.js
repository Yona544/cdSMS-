import { createRouter, createWebHistory } from 'vue-router'
import AppLayout from '../components/layout/AppLayout.vue'
import AdminDashboard from '../views/AdminDashboard.vue'
import VoiceManager from '../views/VoiceManager.vue'
import SMSManager from '../views/SMSManager.vue'
import FileManager from '../views/FileManager.vue'
import ActivityLogs from '../views/ActivityLogs.vue'

const routes = [
  {
    path: '/',
    component: AppLayout,
    children: [
      { path: '', name: 'Dashboard', component: AdminDashboard },
      { path: 'voice', name: 'Voice', component: VoiceManager },
      { path: 'sms', name: 'SMS', component: SMSManager },
      { path: 'files', name: 'Files', component: FileManager },
      { path: 'logs', name: 'Logs', component: ActivityLogs },
    ]
  },
  // Add other routes here if they don't use the layout, e.g., a login page
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

export default router
