<template>
  <div class="dashboard">
    <div class="dashboard-header">
      <h1>Your Habits</h1>
      <button v-if="!showForm" class="btn btn-primary" @click="openCreateForm">
        + New Habit
      </button>
    </div>

    <HabitForm
      v-if="showForm"
      :habit="editingHabit ?? undefined"
      @submit="handleFormSubmit"
      @cancel="closeForm"
    />

    <p v-if="habitStore.error" class="error-message">{{ habitStore.error }}</p>

    <div v-if="habitStore.isLoading" class="loading">Loading habits…</div>

    <div v-else-if="habitStore.activeHabits.length === 0 && !showForm" class="empty-state">
      <p>No habits yet. Create your first one!</p>
    </div>

    <div v-else class="habit-list">
      <HabitCard
        v-for="habit in habitStore.activeHabits"
        :key="habit.id"
        :habit="habit"
        @edit="openEditForm"
        @delete="handleDelete"
      />
    </div>

    <template v-if="habitStore.archivedHabits.length > 0">
      <h2 class="section-title">Archived</h2>
      <div class="habit-list">
        <HabitCard
          v-for="habit in habitStore.archivedHabits"
          :key="habit.id"
          :habit="habit"
          @edit="openEditForm"
          @delete="handleDelete"
        />
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useHabitStore } from '@/stores/habits'
import HabitCard from '@/components/HabitCard.vue'
import HabitForm from '@/components/HabitForm.vue'
import type { HabitResponse } from '@/services/types'

const habitStore = useHabitStore()

const showForm = ref(false)
const editingHabit = ref<HabitResponse | null>(null)

onMounted(() => {
  habitStore.loadHabits()
})

function openCreateForm() {
  editingHabit.value = null
  showForm.value = true
}

function openEditForm(habit: HabitResponse) {
  editingHabit.value = habit
  showForm.value = true
}

function closeForm() {
  showForm.value = false
  editingHabit.value = null
}

async function handleFormSubmit(payload: { name: string; description?: string; isArchived: boolean }) {
  if (editingHabit.value) {
    await habitStore.editHabit(editingHabit.value.id, payload)
  } else {
    await habitStore.addHabit({ name: payload.name, description: payload.description })
  }
  closeForm()
}

async function handleDelete(id: string) {
  if (!confirm('Delete this habit?')) return
  await habitStore.removeHabit(id)
}
</script>

<style scoped>
.dashboard-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 1.5rem;
}

.dashboard-header h1 {
  font-size: 1.5rem;
}

.btn-primary {
  padding: 0.5rem 1rem;
  border: none;
  border-radius: var(--radius);
  background: var(--color-primary);
  color: white;
  font-size: 0.875rem;
  font-weight: 500;
  transition: background 0.15s;
}

.btn-primary:hover {
  background: var(--color-primary-hover);
}

.error-message {
  margin-bottom: 1rem;
  padding: 0.5rem 0.75rem;
  border-radius: var(--radius);
  background: #fef2f2;
  color: var(--color-danger);
  font-size: 0.875rem;
}

.loading {
  text-align: center;
  padding: 2rem;
  color: var(--color-text-muted);
}

.empty-state {
  text-align: center;
  padding: 3rem 1rem;
  color: var(--color-text-muted);
}

.habit-list {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.section-title {
  font-size: 1rem;
  font-weight: 600;
  color: var(--color-text-muted);
  margin-top: 2rem;
  margin-bottom: 0.75rem;
}
</style>
