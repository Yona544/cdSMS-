<template>
  <button
    :type="type"
    :class="[
      'px-4 py-2 rounded-md font-semibold text-sm focus:outline-none focus:ring-2 focus:ring-offset-2',
      'transition ease-in-out duration-150',
      {
        'bg-blue-600 text-white hover:bg-blue-700 focus:ring-blue-500': color === 'primary',
        'bg-gray-200 text-gray-800 hover:bg-gray-300 focus:ring-gray-500': color === 'secondary',
        'bg-red-600 text-white hover:bg-red-700 focus:ring-red-500': color === 'danger',
      },
      { 'opacity-50 cursor-not-allowed': disabled }
    ]"
    :disabled="disabled"
  >
    <slot />
  </button>
</template>

<script setup>
defineProps({
  type: {
    type: String,
    default: 'button',
  },
  color: {
    type: String,
    default: 'primary', // primary, secondary, danger
  },
  disabled: {
    type: Boolean,
    default: false,
  },
})
</script>
