import { useState } from 'react';
import { AuthCard } from './components/AuthCard';
import { GameDashboard } from './components/GameDashboard';
import { LoginPage } from './pages/Login';
import { RegisterPage } from './pages/Register';
import type { AuthMode, AuthResponse } from './types/auth';

export default function App() {
  const [mode, setMode] = useState<AuthMode>('login');
  const [authResult, setAuthResult] = useState<AuthResponse | null>(null);

  const handleSuccess = (response: AuthResponse) => {
    setAuthResult(response);
  };

  const subtitle =
    mode === 'login'
      ? 'Sạc lại năng lượng và tiếp tục hành trình học tập tại Thị Trấn Học Thuật.'
      : 'Thu thập kiến thức, nâng cấp năng lượng và khám phá khu Arcade đầy phần thưởng!';

  if (authResult) {
    return (
      <div className="app-shell dashboard-mode">
        <GameDashboard auth={authResult} />
      </div>
    );
  }

  return (
    <div className="app-shell">
      <AuthCard title="Game RPG Học Tập" subtitle={subtitle}>
        {mode === 'login' ? (
          <LoginPage onSuccess={handleSuccess} switchToRegister={() => setMode('register')} />
        ) : (
          <RegisterPage onSuccess={handleSuccess} switchToLogin={() => setMode('login')} />
        )}
      </AuthCard>
    </div>
  );
}

