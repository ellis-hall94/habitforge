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
