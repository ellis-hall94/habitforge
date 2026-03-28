<template>
  <div class="habit-card" :class="{ 'habit-card--archived': habit.isArchived }">
    <div class="habit-header">
      <h3 class="habit-name">{{ habit.name }}</h3>
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
}>()

defineEmits<{
  edit: [habit: HabitResponse]
  delete: [id: string]
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

.habit-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 0.5rem;
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
