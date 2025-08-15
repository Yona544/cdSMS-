<template>
  <div class="w-64 bg-gray-800 text-white flex flex-col">
    <div class="p-4 border-b border-gray-700">
      <h1 class="text-xl font-bold">Twilio Manager</h1>
    </div>
    <nav class="flex-1 px-2 py-4 space-y-2">
      <router-link
        to="/"
        class="flex items-center px-4 py-2 text-gray-300 hover:bg-gray-700 hover:text-white rounded-md"
        active-class="bg-gray-900 text-white"
      >
        Dashboard
      </router-link>
      <router-link
        to="/voice"
        class="flex items-center px-4 py-2 text-gray-300 hover:bg-gray-700 hover:text-white rounded-md"
        active-class="bg-gray-900 text-white"
      >
        Voice Messages
      </router-link>
      <router-link
        to="/sms"
        class="flex items-center px-4 py-2 text-gray-300 hover:bg-gray-700 hover:text-white rounded-md"
        active-class="bg-gray-900 text-white"
      >
        SMS
      </router-link>
      <router-link
        to="/files"
        class="flex items-center px-4 py-2 text-gray-300 hover:bg-gray-700 hover:text-white rounded-md"
        active-class="bg-gray-900 text-white"
      >
        Files
      </router-link>
      <router-link
        to="/logs"
        class="flex items-center px-4 py-2 text-gray-300 hover:bg-gray-700 hover:text-white rounded-md"
        active-class="bg-gray-900 text-white"
      >
        Activity Logs
      </router-link>
    </nav>
  </div>
</template>

<script setup>
// No script needed for this simple component
</script>
