export type AuthMode = 'login' | 'register';

export interface AuthResponse {
  hocSinhId: number;
  tenDangNhap: string;
  email?: string | null;
  tongDiem: number;
  nangLuongGioChoi: number;
  accessToken: string;
}

export interface RegisterRequest {
  tenDangNhap: string;
  email: string;
  password: string;
}

export interface LoginRequest {
  tenDangNhap: string;
  password: string;
}

