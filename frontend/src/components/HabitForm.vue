<template>
  <form class="habit-form" @submit.prevent="handleSubmit">
    <h2>{{ isEditing ? 'Edit Habit' : 'New Habit' }}</h2>
    <div class="form-group">
      <label for="habitName">Name</label>
      <input
        id="habitName"
        v-model="name"
        type="text"
        required
        maxlength="200"
        placeholder="e.g. Morning run"
      />
    </div>
    <div class="form-group">
      <label for="habitDescription">Description (optional)</label>
      <textarea
        id="habitDescription"
        v-model="description"
        maxlength="1000"
        rows="3"
        placeholder="Why this habit matters…"
      />
    </div>
    <div v-if="isEditing" class="form-group form-group--inline">
      <label for="habitArchived">
        <input id="habitArchived" v-model="isArchived" type="checkbox" />
        Archived
      </label>
    </div>
    <div class="form-actions">
      <button type="button" class="btn btn-secondary" @click="$emit('cancel')">Cancel</button>
      <button type="submit" class="btn btn-primary" :disabled="isSubmitting">
        {{ isSubmitting ? 'Saving…' : 'Save' }}
      </button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import type { HabitResponse } from '@/services/types'

const props = defineProps<{
  habit?: HabitResponse
}>()

const emit = defineEmits<{
  submit: [payload: { name: string; description?: string; isArchived: boolean }]
  cancel: []
}>()

const isEditing = !!props.habit
const isSubmitting = ref(false)

const name = ref(props.habit?.name ?? '')
const description = ref(props.habit?.description ?? '')
const isArchived = ref(props.habit?.isArchived ?? false)

async function handleSubmit() {
  isSubmitting.value = true
  try {
    emit('submit', {
      name: name.value,
      description: description.value || undefined,
      isArchived: isArchived.value,
    })
  } finally {
    isSubmitting.value = false
  }
}
</script>

<style scoped>
.habit-form {
  padding: 1.5rem;
  background: var(--color-surface);
  border: 1px solid var(--color-border);
  border-radius: var(--radius);
  margin-bottom: 1.5rem;
}

.habit-form h2 {
  font-size: 1.125rem;
  margin-bottom: 1rem;
}

.form-group {
  margin-bottom: 1rem;
}

.form-group label {
  display: block;
  margin-bottom: 0.25rem;
  font-size: 0.875rem;
  font-weight: 500;
}

.form-group input[type="text"],
.form-group textarea {
  width: 100%;
  padding: 0.5rem 0.75rem;
  border: 1px solid var(--color-border);
  border-radius: var(--radius);
  font-size: 0.875rem;
  transition: border-color 0.15s;
}

.form-group input[type="text"]:focus,
.form-group textarea:focus {
  outline: none;
  border-color: var(--color-primary);
  box-shadow: 0 0 0 3px rgba(79, 70, 229, 0.1);
}

.form-group textarea {
  resize: vertical;
}

.form-group--inline label {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  cursor: pointer;
}

.form-actions {
  display: flex;
  justify-content: flex-end;
  gap: 0.5rem;
  margin-top: 1rem;
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

.btn-primary:hover:not(:disabled) {
  background: var(--color-primary-hover);
}

.btn-primary:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-secondary {
  padding: 0.5rem 1rem;
  border: 1px solid var(--color-border);
  border-radius: var(--radius);
  background: transparent;
  color: var(--color-text-muted);
  font-size: 0.875rem;
  transition: border-color 0.15s, color 0.15s;
}

.btn-secondary:hover {
  border-color: var(--color-text-muted);
  color: var(--color-text);
}
</style>
