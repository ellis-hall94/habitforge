<template>
  <div class="auth-page">
    <div class="auth-card">
      <h1>Login</h1>
      <form @submit.prevent="handleLogin">
        <div class="form-group">
          <label for="email">Email</label>
          <input
            id="email"
            v-model="email"
            type="email"
            required
            autocomplete="email"
            placeholder="you@example.com"
          />
        </div>
        <div class="form-group">
          <label for="password">Password</label>
          <input
            id="password"
            v-model="password"
            type="password"
            required
            autocomplete="current-password"
            placeholder="Your password"
          />
        </div>
        <p v-if="auth.error" class="error-message">{{ auth.error }}</p>
        <button type="submit" class="btn btn-primary" :disabled="auth.isLoading">
          {{ auth.isLoading ? 'Logging in…' : 'Login' }}
        </button>
      </form>
      <p class="auth-link">
        Don't have an account? <RouterLink to="/register">Register</RouterLink>
      </p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { RouterLink, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { AxiosError } from 'axios'

const auth = useAuthStore()
const router = useRouter()

const email = ref('')
const password = ref('')

async function handleLogin() {
  try {
    await auth.login({ email: email.value, password: password.value })
    await router.push({ name: 'dashboard' })
  } catch (e: unknown) {
    if (e instanceof AxiosError && e.response?.status === 401) {
      auth.error = 'Invalid email or password'
    }
  }
}
</script>

<style scoped>
.auth-page {
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  padding: 1rem;
}

.auth-card {
  width: 100%;
  max-width: 400px;
  padding: 2rem;
  background: var(--color-surface);
  border: 1px solid var(--color-border);
  border-radius: var(--radius);
}

.auth-card h1 {
  margin-bottom: 1.5rem;
  font-size: 1.5rem;
}

.form-group {
  margin-bottom: 1rem;
}

.form-group label {
  display: block;
  margin-bottom: 0.25rem;
  font-size: 0.875rem;
  font-weight: 500;
  color: var(--color-text);
}

.form-group input {
  width: 100%;
  padding: 0.5rem 0.75rem;
  border: 1px solid var(--color-border);
  border-radius: var(--radius);
  font-size: 0.875rem;
  transition: border-color 0.15s;
}

.form-group input:focus {
  outline: none;
  border-color: var(--color-primary);
  box-shadow: 0 0 0 3px rgba(79, 70, 229, 0.1);
}

.error-message {
  margin-bottom: 1rem;
  padding: 0.5rem 0.75rem;
  border-radius: var(--radius);
  background: #fef2f2;
  color: var(--color-danger);
  font-size: 0.875rem;
}

.btn-primary {
  width: 100%;
  padding: 0.6rem;
  border: none;
  border-radius: var(--radius);
  background: var(--color-primary);
  color: white;
  font-size: 0.875rem;
  font-weight: 500;
  transition: background 0.15s;
}

.btn-primary:hover:not(:disabled) {
  background: var(--color-primary-hover);
}

.btn-primary:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.auth-link {
  margin-top: 1.5rem;
  text-align: center;
  font-size: 0.875rem;
  color: var(--color-text-muted);
}
</style>
