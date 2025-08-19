<template>
  <div class="legacy-scope">
    <!-- Legacy-style top nav strip -->
    <div class="nav-outer-repeat">
      <div class="nav-outer">
        <div id="nav-right">
          <div class="clear">&nbsp;</div>
        </div>
        <div class="nav">
          <div class="table">
            <ul class="nav select">
              <li v-for="item in navItems" :key="item.path" :class="{ current: isActive(item) }">
                <router-link :to="item.path">
                  <template v-if="item.key === 'logout'">
                    <img src="/legacy/admin/images/nav_logout.gif" alt="Logout" />
                  </template>
                  <template v-else>
                    {{ item.label }}
                  </template>
                </router-link>
              </li>
            </ul>
          </div>
        </div>
        <div class="clear"></div>
      </div>
    </div>

    <!-- Existing modern shell preserved -->
    <div class="flex h-screen bg-gray-100">
      <!-- Sidebar -->
      <Sidebar />

      <!-- Main content -->
      <div class="flex-1 flex flex-col overflow-hidden">
        <!-- Header -->
        <Header />

        <!-- Main content area -->
        <main class="flex-1 overflow-x-hidden overflow-y-auto bg-gray-200">
          <div class="container mx-auto px-6 py-8">
            <router-view />
          </div>
        </main>
      </div>
    </div>

    <!-- Legacy-style footer -->
    <div id="footer">
      <div id="footer-left"><i>&nbsp;</i>&nbsp;</div>
      <div class="clear">&nbsp;</div>
    </div>
  </div>
</template>

<script setup>
import Sidebar from './Sidebar.vue'
import Header from './Header.vue'
import { useRoute } from 'vue-router'

const route = useRoute()

// JSON-driven top navigation items derived from router definitions
const navItems = [
  { key: 'dashboard', label: 'Dashboard', path: '/' },
  { key: 'voice', label: 'Voice', path: '/voice' },
  { key: 'sms', label: 'SMS', path: '/sms' },
  { key: 'files', label: 'Files', path: '/files' },
  { key: 'users', label: 'Users', path: '/users' },
  { key: 'voice-xml', label: 'Voice XML', path: '/voice-xml' },
  { key: 'logs', label: 'Logs', path: '/logs' },
  { key: 'logout', label: 'Logout', path: '/logout' },
]

// Legacy "current" state helper
function isActive(item) {
  if (item.path === '/') {
    // Exact match for dashboard so "/" doesn't match everything
    return route.path === item.path
  }
  return route.path.startsWith(item.path)
}
</script>

<style scoped>
/* Active nav highlighting to mimic legacy "current" state */
.legacy-scope .nav .select li.current a,
.legacy-scope .nav .select li.current a:hover {
  background: url(/legacy/admin/images/pro_line_2.gif);
  color: #fff;
  padding-left: 10px;
}
</style>
