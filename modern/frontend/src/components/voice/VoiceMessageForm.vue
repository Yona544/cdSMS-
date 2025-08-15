<template>
  <TransitionRoot appear :show="isOpen" as="template">
    <Dialog as="div" @close="closeModal" class="relative z-10">
      <TransitionChild
        as="template"
        enter="duration-300 ease-out"
        enter-from="opacity-0"
        enter-to="opacity-100"
        leave="duration-200 ease-in"
        leave-from="opacity-100"
        leave-to="opacity-0"
      >
        <div class="fixed inset-0 bg-black bg-opacity-25" />
      </TransitionChild>

      <div class="fixed inset-0 overflow-y-auto">
        <div class="flex min-h-full items-center justify-center p-4 text-center">
          <TransitionChild
            as="template"
            enter="duration-300 ease-out"
            enter-from="opacity-0 scale-95"
            enter-to="opacity-100 scale-100"
            leave="duration-200 ease-in"
            leave-from="opacity-100 scale-100"
            leave-to="opacity-0 scale-95"
          >
            <DialogPanel class="w-full max-w-md transform overflow-hidden rounded-2xl bg-white p-6 text-left align-middle shadow-xl transition-all">
              <DialogTitle as="h3" class="text-lg font-medium leading-6 text-gray-900">
                {{ isEditing ? 'Edit' : 'Create' }} Voice Message
              </DialogTitle>
              <form @submit.prevent="submitForm" class="mt-4 space-y-4">
                <div>
                  <label for="name" class="block text-sm font-medium text-gray-700">Name</label>
                  <input v-model="form.name" type="text" id="name" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" required>
                </div>
                <div>
                  <label for="voice_text" class="block text-sm font-medium text-gray-700">Text</label>
                  <textarea v-model="form.voice_text" id="voice_text" rows="4" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" required></textarea>
                </div>
                <!-- Add other fields like gender, rate etc. later -->
                <div class="mt-6 flex justify-end space-x-2">
                  <BaseButton type="button" color="secondary" @click="closeModal">Cancel</BaseButton>
                  <BaseButton type="submit" color="primary">Save</BaseButton>
                </div>
              </form>
            </DialogPanel>
          </TransitionChild>
        </div>
      </div>
    </Dialog>
  </TransitionRoot>
</template>

<script setup>
import { ref, watch } from 'vue'
import {
  TransitionRoot,
  TransitionChild,
  Dialog,
  DialogPanel,
  DialogTitle,
} from '@headlessui/vue'
import BaseButton from '../common/BaseButton.vue'

const props = defineProps({
  isOpen: Boolean,
  message: Object, // The message to edit, or null for creation
})

const emit = defineEmits(['close', 'save'])

const isEditing = ref(false)
const form = ref({
  name: '',
  voice_text: '',
})

watch(() => props.message, (newMessage) => {
  if (newMessage) {
    isEditing.value = true
    form.value = { ...newMessage }
  } else {
    isEditing.value = false
    form.value = { name: '', voice_text: '' }
  }
}, { immediate: true })

function closeModal() {
  emit('close')
}

function submitForm() {
  emit('save', form.value)
  closeModal()
}
</script>
