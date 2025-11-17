import { FormEvent, useState } from 'react';
import { AuthApi } from '../api/auth';
import type { AuthResponse } from '../types/auth';

interface RegisterPageProps {
  onSuccess: (response: AuthResponse) => void;
  switchToLogin: () => void;
}

export function RegisterPage({ onSuccess, switchToLogin }: RegisterPageProps) {
  const [tenDangNhap, setTenDangNhap] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setError(null);
    setIsLoading(true);

    try {
      const response = await AuthApi.register({ tenDangNhap, email, password });
      onSuccess(response);
    } catch (err) {
      setError('Đăng ký thất bại. Tên đăng nhập hoặc email có thể đã tồn tại.');
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
          Email
          <input
            type="email"
            value={email}
            onChange={(event) => setEmail(event.target.value)}
            placeholder="ban@hoctap.vn"
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
          {isLoading ? 'Đang xử lý...' : 'Hoàn tất đăng ký'}
        </button>
      </form>

      <p className="auth-form__hint">
        Bạn đã có tài khoản?{' '}
        <button type="button" className="link-button" onClick={switchToLogin}>
          Đăng nhập
        </button>
      </p>
    </>
  );
}

