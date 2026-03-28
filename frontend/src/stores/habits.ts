import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import {
  fetchHabits as fetchHabitsApi,
  createHabit as createHabitApi,
  updateHabit as updateHabitApi,
  deleteHabit as deleteHabitApi,
} from '@/services/habitService'
import type { CreateHabitRequest, UpdateHabitRequest, HabitResponse } from '@/services/types'

export const useHabitStore = defineStore('habits', () => {
  const habits = ref<HabitResponse[]>([])
  const isLoading = ref(false)
  const error = ref<string | null>(null)

  const activeHabits = computed(() => habits.value.filter((h) => !h.isArchived))
  const archivedHabits = computed(() => habits.value.filter((h) => h.isArchived))

  async function loadHabits() {
    isLoading.value = true
    error.value = null
    try {
      habits.value = await fetchHabitsApi()
    } catch (e: unknown) {
      const message = e instanceof Error ? e.message : 'Failed to load habits'
      error.value = message
      throw e
    } finally {
      isLoading.value = false
    }
  }

  async function addHabit(request: CreateHabitRequest) {
    const habit = await createHabitApi(request)
    habits.value.push(habit)
    return habit
  }

  async function editHabit(id: string, request: UpdateHabitRequest) {
    const updated = await updateHabitApi(id, request)
    const index = habits.value.findIndex((h) => h.id === id)
    if (index !== -1) {
      habits.value[index] = updated
    }
    return updated
  }

  async function removeHabit(id: string) {
    await deleteHabitApi(id)
    habits.value = habits.value.filter((h) => h.id !== id)
  }

  return { habits, isLoading, error, activeHabits, archivedHabits, loadHabits, addHabit, editHabit, removeHabit }
})
