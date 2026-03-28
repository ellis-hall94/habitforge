import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import {
  fetchHabits as fetchHabitsApi,
  createHabit as createHabitApi,
  updateHabit as updateHabitApi,
  deleteHabit as deleteHabitApi,
  toggleCompletion as toggleCompletionApi,
  fetchCompletions as fetchCompletionsApi,
} from '@/services/habitService'
import type { CreateHabitRequest, UpdateHabitRequest, HabitResponse } from '@/services/types'

export const useHabitStore = defineStore('habits', () => {
  const habits = ref<HabitResponse[]>([])
  const completedToday = ref<Set<string>>(new Set())
  const isLoading = ref(false)
  const error = ref<string | null>(null)

  const activeHabits = computed(() => habits.value.filter((h) => !h.isArchived))
  const archivedHabits = computed(() => habits.value.filter((h) => h.isArchived))

  function isCompletedToday(habitId: string): boolean {
    return completedToday.value.has(habitId)
  }

  async function loadHabits() {
    isLoading.value = true
    error.value = null
    try {
      habits.value = await fetchHabitsApi()
      await loadTodayCompletions()
    } catch (e: unknown) {
      const message = e instanceof Error ? e.message : 'Failed to load habits'
      error.value = message
      throw e
    } finally {
      isLoading.value = false
    }
  }

  async function loadTodayCompletions() {
    const today = new Date().toISOString().split('T')[0]
    const newSet = new Set<string>()
    for (const habit of habits.value) {
      const completions = await fetchCompletionsApi(habit.id, today, today)
      if (completions.length > 0) {
        newSet.add(habit.id)
      }
    }
    completedToday.value = newSet
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
    completedToday.value.delete(id)
  }

  async function toggleCheckin(habitId: string) {
    const result = await toggleCompletionApi(habitId)
    if (result.completed) {
      completedToday.value.add(habitId)
    } else {
      completedToday.value.delete(habitId)
    }
    completedToday.value = new Set(completedToday.value)
    return result.completed
  }

  return { habits, completedToday, isLoading, error, activeHabits, archivedHabits, isCompletedToday, loadHabits, addHabit, editHabit, removeHabit, toggleCheckin }
})
