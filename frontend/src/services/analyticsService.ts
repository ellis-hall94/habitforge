import api from './api'
import type {
    StreakResponse,
    HabitStreakResponse,
    HabitTrendResponse,
    AnalyticsSummaryResponse,
} from './types'

export async function fetchAllStreaks(): Promise<HabitStreakResponse[]> {
    const { data } = await api.get<HabitStreakResponse[]>('/analytics/streaks')
    return data
}

export async function fetchHabitStreaks(habitId: string): Promise<StreakResponse> {
    const { data } = await api.get<StreakResponse>(`/analytics/streaks/${habitId}/streaks`)
    return data
}

export async function fetchHabitTrend(habitId: string, days: number = 30): Promise<HabitTrendResponse> {
    const { data } = await api.get<HabitTrendResponse>(`/analytics/habit/${habitId}/trends`, {
        params: { days },
    })
    return data
}

export async function fetchAnalyticsSummary(): Promise<AnalyticsSummaryResponse> {
    const { data } = await api.get<AnalyticsSummaryResponse>('/analytics/summary')
    return data
}