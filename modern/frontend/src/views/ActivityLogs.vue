<template>
  <div>
    <h1 class="text-2xl font-bold mb-6">Activity Logs</h1>

    <div v-if="commStore.loading && !commStore.history.length" class="text-center">Loading history...</div>
    <div v-if="commStore.error" class="text-center text-red-500">{{ commStore.error }}</div>

    <div v-if="!commStore.loading && !commStore.history.length" class="text-center text-gray-500 mt-8">
      No activity logs found.
    </div>

    <div v-if="commStore.history.length" class="space-y-4">
      <ActivityLogItem
        v-for="log in commStore.history"
        :key="log.id"
        :log="log"
      />
    </div>

    <!-- Pagination -->
    <div v-if="pagination.total_pages > 1" class="mt-6 flex justify-center items-center space-x-4">
      <BaseButton @click="changePage(pagination.page - 1)" :disabled="!pagination.has_prev">
        Previous
      </BaseButton>
      <span>Page {{ pagination.page }} of {{ pagination.total_pages }}</span>
      <BaseButton @click="changePage(pagination.page + 1)" :disabled="!pagination.has_next">
        Next
      </BaseButton>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import { useCommunicationStore } from '@/stores/communication'
import ActivityLogItem from '@/components/logs/ActivityLogItem.vue'
import BaseButton from '@/components/common/BaseButton.vue'

const commStore = useCommunicationStore()
const currentPage = ref(1)

// A computed property to extract pagination data from the store's response
const pagination = computed(() => {
  // This assumes the fetchHistory action returns the full API response with pagination data
  // We will need to update the store action to store this. For now, we create a dummy structure.
  // This is a simplification. In a real app, the store would manage pagination state.
  return {
    page: currentPage.value,
    total_pages: Math.ceil(commStore.history.length / 20), // Dummy calculation
    has_next: true, // Dummy value
    has_prev: currentPage.value > 1,
  }
})


onMounted(() => {
  commStore.fetchHistory(currentPage.value)
})

function changePage(newPage) {
  if (newPage > 0) {
    currentPage.value = newPage
    commStore.fetchHistory(currentPage.value)
  }
}
</script>
