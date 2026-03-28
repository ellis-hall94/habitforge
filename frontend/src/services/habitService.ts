import api from './api'
import type { CreateHabitRequest, UpdateHabitRequest, HabitResponse } from './types'

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
