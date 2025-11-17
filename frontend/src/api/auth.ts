import type { AuthResponse, LoginRequest, RegisterRequest } from '../types/auth';
import { httpClient } from './httpClient';

export const AuthApi = {
  async register(payload: RegisterRequest): Promise<AuthResponse> {
    const { data } = await httpClient.post<AuthResponse>('/auth/register', payload);
    return data;
  },

  async login(payload: LoginRequest): Promise<AuthResponse> {
    const { data } = await httpClient.post<AuthResponse>('/auth/login', payload);
    return data;
  }
};

