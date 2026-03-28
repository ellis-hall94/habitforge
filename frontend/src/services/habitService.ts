import api from './api'
import type { CreateHabitRequest, UpdateHabitRequest, HabitResponse, ToggleCompletionResponse, HabitCompletionResponse } from './types'

export async function fetchHabits(): Promise<HabitResponse[]> {
  const { data } = await api.get<HabitResponse[]>('/habits')
  return data
}

export async function fetchHabit(id: string): Promise<HabitResponse> {
  const { data } = await api.get<HabitResponse>(`/habits/${id}`)
  return data
}

export async function createHabit(request: CreateHabitRequest): Promise<HabitResponse> {
  const { data } = await api.post<HabitResponse>('/habits', request)
  return data
}

export async function updateHabit(id: string, request: UpdateHabitRequest): Promise<HabitResponse> {
  const { data } = await api.put<HabitResponse>(`/habits/${id}`, request)
  return data
}

export async function deleteHabit(id: string): Promise<void> {
  await api.delete(`/habits/${id}`)
}

export async function toggleCompletion(habitId: string, date?: string): Promise<ToggleCompletionResponse> {
  const params = date ? { date } : {}
  const { data } = await api.post<ToggleCompletionResponse>(`/habits/${habitId}/completions`, null, { params })
  return data
}

export async function fetchCompletions(habitId: string, from?: string, to?: string): Promise<HabitCompletionResponse[]> {
  const params: Record<string, string> = {}
  if (from) params.from = from
  if (to) params.to = to
  const { data } = await api.get<HabitCompletionResponse[]>(`/habits/${habitId}/completions`, { params })
  return data
}
