<template>
  <div class="habit-card" :class="{ 'habit-card--archived': habit.isArchived, 'habit-card--completed': completedToday }">
    <div class="habit-header">
      <div class="habit-header-left">
        <button
          v-if="!habit.isArchived"
          class="btn-checkin"
          :class="{ 'btn-checkin--done': completedToday }"
          :title="completedToday ? 'Undo check-in' : 'Mark complete'"
          @click="$emit('toggle', habit.id)"
        >
          {{ completedToday ? '✅' : '⬜' }}
        </button>
        <h3 class="habit-name">{{ habit.name }}</h3>
      </div>
      <div class="habit-actions">
        <button class="btn-icon" title="Edit" @click="$emit('edit', habit)">✏️</button>
        <button class="btn-icon" title="Delete" @click="$emit('delete', habit.id)">🗑️</button>
      </div>
    </div>
    <p v-if="habit.description" class="habit-description">{{ habit.description }}</p>
    <div class="habit-footer">
      <span class="habit-date">Created {{ formattedDate }}</span>
      <span v-if="habit.isArchived" class="habit-badge habit-badge--archived">Archived</span>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import type { HabitResponse } from '@/services/types'

const props = defineProps<{
  habit: HabitResponse
  completedToday: boolean
}>()

defineEmits<{
  edit: [habit: HabitResponse]
  delete: [id: string]
  toggle: [id: string]
}>()

const formattedDate = computed(() =>
  new Date(props.habit.createdAt).toLocaleDateString()
)
</script>

<style scoped>
.habit-card {
  padding: 1rem 1.25rem;
  background: var(--color-surface);
  border: 1px solid var(--color-border);
  border-radius: var(--radius);
  transition: box-shadow 0.15s;
}

.habit-card:hover {
  box-shadow: 0 1px 4px rgba(0, 0, 0, 0.06);
}

.habit-card--archived {
  opacity: 0.6;
}

.habit-card--completed {
  border-color: var(--color-success);
  background: #f0fdf4;
}

.habit-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 0.5rem;
}

.habit-header-left {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.btn-checkin {
  padding: 0.15rem;
  border: none;
  background: transparent;
  font-size: 1.25rem;
  line-height: 1;
  cursor: pointer;
  transition: transform 0.15s;
}

.btn-checkin:hover {
  transform: scale(1.15);
}

.habit-name {
  font-size: 1rem;
  font-weight: 600;
}

.habit-actions {
  display: flex;
  gap: 0.25rem;
}

.btn-icon {
  padding: 0.25rem 0.4rem;
  border: none;
  background: transparent;
  border-radius: var(--radius);
  font-size: 0.875rem;
  transition: background 0.15s;
}

.btn-icon:hover {
  background: var(--color-background);
}

.habit-description {
  margin-top: 0.5rem;
  font-size: 0.875rem;
  color: var(--color-text-muted);
}

.habit-footer {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-top: 0.75rem;
}

.habit-date {
  font-size: 0.75rem;
  color: var(--color-text-muted);
}

.habit-badge {
  font-size: 0.7rem;
  font-weight: 500;
  padding: 0.15rem 0.5rem;
  border-radius: 999px;
}

.habit-badge--archived {
  background: #f3f4f6;
  color: var(--color-text-muted);
}
</style>
