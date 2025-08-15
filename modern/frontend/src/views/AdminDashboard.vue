<template>
  <div>
    <h1 class="text-2xl font-bold mb-6">Dashboard</h1>
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <!-- Welcome Card -->
      <div class="bg-white p-6 rounded-lg shadow-md">
        <h2 class="text-lg font-semibold">Welcome!</h2>
        <p class="text-gray-600 mt-2">
          This is your central hub for managing Twilio communications.
        </p>
      </div>

      <!-- Quick Links Card -->
      <div class="bg-white p-6 rounded-lg shadow-md">
        <h2 class="text-lg font-semibold">Quick Links</h2>
        <ul class="mt-2 space-y-2">
          <li><router-link to="/voice" class="text-blue-600 hover:underline">Manage Voice Messages</router-link></li>
          <li><router-link to="/sms" class="text-blue-600 hover:underline">Send SMS</router-link></li>
          <li><router-link to="/files" class="text-blue-600 hover:underline">Manage Files</router-link></li>
          <li><router-link to="/logs" class="text-blue-600 hover:underline">View Activity Logs</router-link></li>
        </ul>
      </div>

      <!-- Stats Card (dummy) -->
      <div class="bg-white p-6 rounded-lg shadow-md">
        <h2 class="text-lg font-semibold">Usage Stats</h2>
        <p class="text-gray-600 mt-2">
          (Statistics will be displayed here in a future update.)
        </p>
      </div>
    </div>
  </div>
</template>

<script setup>
// No specific logic needed for this simple dashboard yet.
</script>
