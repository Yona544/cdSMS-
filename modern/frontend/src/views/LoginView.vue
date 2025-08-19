<template>
  <div class="legacy-scope">
    <div class="legacy-login-bg" style="min-height: 100vh;">
      <div id="login-holder" class="mx-auto" style="width:508px; padding-top: 120px;">
        <div id="loginbox" class="legacy-login-box mx-auto">
          <div id="login-inner" class="mx-auto" style="width:310px; font-family: Tahoma; font-size:13px;">
            <div class="text-center" style="padding-bottom: 12px;">
              <span v-if="message" style="color:#fff">{{ message }}</span>
            </div>
            <form @submit.prevent="onSubmit">
              <table border="0" cellpadding="0" cellspacing="0" style="width:100%;">
                <tbody>
                  <tr>
                    <th style="text-align:left; width:95px;">API Key</th>
                    <td>
                      <input v-model="apiKey" class="login-inp" type="text" required />
                    </td>
                  </tr>
                  <tr>
                    <th></th>
                    <td>
                      <button class="submit-login" type="submit">Login</button>
                    </td>
                  </tr>
                </tbody>
              </table>
            </form>
          </div>
          <div class="clear"></div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter, useRoute } from 'vue-router'

const router = useRouter()
const route = useRoute()

const apiKey = ref(localStorage.getItem('apiKey') || '')
const message = ref(route.query.nosession ? 'Session expired, please login again.' : '')

function onSubmit() {
  localStorage.setItem('apiKey', apiKey.value.trim())
  router.push('/')
}
</script>