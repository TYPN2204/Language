import { FormEvent, useState } from 'react';
import { AuthApi } from '../api/auth';
import type { AuthResponse } from '../types/auth';

interface LoginPageProps {
  onSuccess: (response: AuthResponse) => void;
  switchToRegister: () => void;
}

export function LoginPage({ onSuccess, switchToRegister }: LoginPageProps) {
  const [tenDangNhap, setTenDangNhap] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setError(null);
    setIsLoading(true);

    try {
      const response = await AuthApi.login({ tenDangNhap, password });
      onSuccess(response);
    } catch (err) {
      setError('Đăng nhập thất bại. Vui lòng kiểm tra thông tin và thử lại.');
      console.error(err);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <>
      <form className="auth-form" onSubmit={handleSubmit}>
        <label>
          Tên đăng nhập
          <input
            value={tenDangNhap}
            onChange={(event) => setTenDangNhap(event.target.value)}
            placeholder="anhhunghocvien"
            required
          />
        </label>

        <label>
          Mật khẩu
          <input
            type="password"
            value={password}
            onChange={(event) => setPassword(event.target.value)}
            placeholder="••••••"
            required
          />
        </label>

        {error && <p className="auth-form__error">{error}</p>}

        <button type="submit" disabled={isLoading}>
          {isLoading ? 'Đang xử lý...' : 'Đăng nhập'}
        </button>
      </form>

      <p className="auth-form__hint">
        Bạn chưa có tài khoản?{' '}
        <button type="button" className="link-button" onClick={switchToRegister}>
          Đăng ký ngay
        </button>
      </p>
    </>
  );
}

