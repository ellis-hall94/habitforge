export interface LoginRequest {
  email: string
  password: string
}

export interface RegisterRequest {
  email: string
  password: string
  displayName: string
}

export interface AuthenticationResponse {
  token: string
  expiresAt: string
}

export interface CreateHabitRequest {
  name: string
  description?: string
}

export interface UpdateHabitRequest {
  name: string
  description?: string
  isArchived: boolean
}

export interface HabitResponse {
  id: string
  name: string
  description: string | null
  createdAt: string
  updatedAt: string | null
  isArchived: boolean
  userId: string
}

export interface HabitCompletionResponse {
  id: string
  habitId: string
  completedDate: string
  createdAt: string
}

export interface ToggleCompletionResponse {
  completed: boolean
  completion?: HabitCompletionResponse
}

export interface StreakResponse {
  currentStreak: number
  longestStreak: number
  lastCompletedDate: string | null
}

export interface HabitStreakResponse {
  habitId: string
  habitName: string
  currentStreak: number
  longestStreak: number
  lastCompletedDate: string | null
}

export interface CompletionTrendPoint {
  date: string
  completed: boolean
}

export interface HabitTrendResponse {
  habitId: string
  habitName: string
  dataPoints: CompletionTrendPoint[]
}

export interface AnalyticsSummaryResponse {
  totalHabits: number
  totalCompletionsThisWeek: number
  overallCompletionRate: number
  habitStreaks: HabitStreakResponse[]
}
