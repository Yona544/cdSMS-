import { createRouter, createWebHistory } from 'vue-router'
import AppLayout from '../components/layout/AppLayout.vue'
import AdminDashboard from '../views/AdminDashboard.vue'
import VoiceManager from '../views/VoiceManager.vue'
import SMSManager from '../views/SMSManager.vue'
import FileManager from '../views/FileManager.vue'
import ActivityLogs from '../views/ActivityLogs.vue'

const routes = [
  { path: '/login', name: 'Login', component: () => import('@/views/LoginView.vue') },
  { path: '/logout', name: 'Logout', component: () => import('@/views/LogoutView.vue') },
  {
    path: '/',
    component: AppLayout,
    children: [
      { path: '', name: 'Dashboard', component: AdminDashboard },
      { path: 'voice', name: 'Voice', component: VoiceManager },
      { path: 'sms', name: 'SMS', component: SMSManager },
      { path: 'files', name: 'Files', component: FileManager },
      { path: 'logs', name: 'Logs', component: ActivityLogs },
      { path: 'users', name: 'Users', component: () => import('@/views/UsersList.vue') },
      { path: 'users/new', name: 'UserNew', component: () => import('@/views/UserForm.vue') },
      { path: 'users/:id', name: 'UserEdit', component: () => import('@/views/UserForm.vue') },
      { path: 'voice-xml', name: 'VoiceXml', component: () => import('@/views/VoiceXmlList.vue') },
      { path: 'voice-xml/new', name: 'VoiceXmlNew', component: () => import('@/views/VoiceXmlForm.vue') },
      { path: 'voice-xml/:id', name: 'VoiceXmlEdit', component: () => import('@/views/VoiceXmlForm.vue') },
    ]
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
})

// Auth guard; enforce login for all except /login; handle /logout by clearing apiKey then redirect to /login
router.beforeEach((to, from, next) => {
  if (to.path === '/logout') {
    try { localStorage.removeItem('apiKey') } catch (e) {}
    return next({ path: '/login' })
  }
  const isPublic = to.path === '/login'
  const apiKey = localStorage.getItem('apiKey')
  if (!isPublic && !apiKey) {
    return next({ path: '/login', query: { nosession: '1' } })
  }
  next()
})

export default router
