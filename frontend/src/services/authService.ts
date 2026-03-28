import api from './api'
import type { LoginRequest, RegisterRequest, AuthenticationResponse } from './types'

export async function login(request: LoginRequest): Promise<AuthenticationResponse> {
  const { data } = await api.post<AuthenticationResponse>('/authentication/login', request)
  return data
}

export async function register(request: RegisterRequest): Promise<AuthenticationResponse> {
  const { data } = await api.post<AuthenticationResponse>('/authentication/register', request)
  return data
}
