<template>
  <div class="app-layout">
    <nav class="navbar">
      <div class="navbar-left">
        <RouterLink to="/dashboard" class="navbar-brand">HabitForge</RouterLink>
        <div class="navbar-links">
          <RouterLink to="/dashboard" class="nav-link">Dashboard</RouterLink>
          <RouterLink to="/analytics" class="nav-link">Analytics</RouterLink>
        </div>
      </div>
      <button v-if="auth.isAuthenticated" class="btn btn-logout" @click="handleLogout">
        Logout
      </button>
    </nav>
    <main class="main-content">
      <slot />
    </main>
  </div>
</template>

<script setup lang="ts">
import { RouterLink, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const auth = useAuthStore()
const router = useRouter()

async function handleLogout() {
  auth.logout()
  await router.push({ name: 'login' })
}
</script>

<style scoped>
.navbar {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0.75rem 1.5rem;
  background: var(--color-surface);
  border-bottom: 1px solid var(--color-border);
}

.navbar-left {
  display: flex;
  align-items: center;
  gap: 2rem;
}

.navbar-brand {
  font-size: 1.25rem;
  font-weight: 700;
  color: var(--color-primary);
}

.navbar-links {
  display: flex;
  gap: 1rem;
}

.nav-link {
  font-size: 0.875rem;
  color: var(--color-text-muted);
  text-decoration: none;
  transition: color 0.15s;
}

.nav-link:hover,
.nav-link.router-link-active {
  color: var(--color-primary);
}

.main-content {
  max-width: 800px;
  margin: 0 auto;
  padding: 2rem 1.5rem;
}

.btn-logout {
  padding: 0.4rem 1rem;
  border: 1px solid var(--color-border);
  border-radius: var(--radius);
  background: transparent;
  color: var(--color-text-muted);
  font-size: 0.875rem;
  transition: color 0.15s, border-color 0.15s;
}

.btn-logout:hover {
  color: var(--color-danger);
  border-color: var(--color-danger);
}
</style>
