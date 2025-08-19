<template>
  <div class="legacy-scope">
    <div id="content">
      <div class="gridtable p-4">
        <div class="tblheader px-3">Logging out...</div>
        <p class="p-3">You are being redirected to the login page.</p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { onMounted } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()

onMounted(() => {
  try {
    localStorage.removeItem('apiKey')
  } finally {
    router.replace({ path: '/login' })
  }
})
</script>