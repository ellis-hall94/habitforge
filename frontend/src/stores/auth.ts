import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { login as loginApi, register as registerApi } from '@/services/authService'
import type { LoginRequest, RegisterRequest } from '@/services/types'

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string | null>(localStorage.getItem('token'))
  const isLoading = ref(false)
  const error = ref<string | null>(null)

  const isAuthenticated = computed(() => !!token.value)

  function setToken(newToken: string) {
    token.value = newToken
    localStorage.setItem('token', newToken)
  }

  function clearToken() {
    token.value = null
    localStorage.removeItem('token')
  }

  async function login(request: LoginRequest) {
    isLoading.value = true
    error.value = null
    try {
      const response = await loginApi(request)
      setToken(response.token)
    } catch (e: unknown) {
      const message = e instanceof Error ? e.message : 'Login failed'
      error.value = message
      throw e
    } finally {
      isLoading.value = false
    }
  }

  async function register(request: RegisterRequest) {
    isLoading.value = true
    error.value = null
    try {
      const response = await registerApi(request)
      setToken(response.token)
    } catch (e: unknown) {
      const message = e instanceof Error ? e.message : 'Registration failed'
      error.value = message
      throw e
    } finally {
      isLoading.value = false
    }
  }

  function logout() {
    clearToken()
  }

  return { token, isLoading, error, isAuthenticated, login, register, logout }
})
