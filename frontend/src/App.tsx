import { useState, useEffect } from 'react';
import { BrowserRouter, Routes, Route, Navigate, useNavigate } from 'react-router-dom';
import { AuthCard } from './components/AuthCard';
import { LoginPage } from './pages/Login';
import { RegisterPage } from './pages/Register';
import { Town } from './pages/Town';
import { School } from './pages/School';
import { Arcade } from './pages/Arcade';
import { Shop } from './pages/Shop';
import { Leaderboard } from './pages/Leaderboard';
import { Chatbot } from './pages/Chatbot';
import { MatchingCardsGamePage } from './pages/MatchingCardsGamePage';
import type { AuthMode, AuthResponse } from './types/auth';

function ProtectedRoute({ children, auth }: { children: React.ReactNode; auth: AuthResponse | null }) {
  if (!auth) {
    return <Navigate to="/" replace />;
  }
  return <>{children}</>;
}

function AuthRedirect({ auth }: { auth: AuthResponse | null }) {
  const navigate = useNavigate();
  
  useEffect(() => {
    if (auth) {
      navigate('/town', { replace: true });
    }
  }, [auth, navigate]);
  
  return null;
}

function AppContent() {
  const [mode, setMode] = useState<AuthMode>('login');
  const [authResult, setAuthResult] = useState<AuthResponse | null>(() => {
    // Lấy auth từ localStorage nếu có
    try {
      const stored = localStorage.getItem('auth');
      return stored ? JSON.parse(stored) : null;
    } catch {
      return null;
    }
  });

  const handleSuccess = (response: AuthResponse) => {
    setAuthResult(response);
    localStorage.setItem('auth', JSON.stringify(response));
    // React Router sẽ tự động redirect về /town khi authResult thay đổi
  };

  const subtitle =
    mode === 'login'
      ? 'Sạc lại năng lượng và tiếp tục hành trình học tập tại Thị Trấn Học Thuật.'
      : 'Thu thập kiến thức, nâng cấp năng lượng và khám phá khu Arcade đầy phần thưởng!';

  if (authResult) {
    return (
      <Routes>
        <Route
          path="/town"
          element={
            <ProtectedRoute auth={authResult}>
              <Town auth={authResult} />
            </ProtectedRoute>
          }
        />
        <Route
          path="/school"
          element={
            <ProtectedRoute auth={authResult}>
              <School auth={authResult} />
            </ProtectedRoute>
          }
        />
        <Route
          path="/arcade"
          element={
            <ProtectedRoute auth={authResult}>
              <Arcade auth={authResult} />
            </ProtectedRoute>
          }
        />
        <Route
          path="/shop"
          element={
            <ProtectedRoute auth={authResult}>
              <Shop auth={authResult} />
            </ProtectedRoute>
          }
        />
        <Route
          path="/leaderboard"
          element={
            <ProtectedRoute auth={authResult}>
              <Leaderboard auth={authResult} />
            </ProtectedRoute>
          }
        />
        <Route
          path="/chatbot"
          element={
            <ProtectedRoute auth={authResult}>
              <Chatbot auth={authResult} />
            </ProtectedRoute>
          }
        />
        <Route
          path="/games/matching-cards"
          element={
            <ProtectedRoute auth={authResult}>
              <MatchingCardsGamePage auth={authResult} />
            </ProtectedRoute>
          }
        />
        <Route path="/" element={<Navigate to="/town" replace />} />
      </Routes>
    );
  }

  return (
    <Routes>
      <Route
        path="/"
        element={
          <>
            <AuthRedirect auth={authResult} />
            <div className="app-shell">
              <AuthCard title="Game RPG Học Tập" subtitle={subtitle}>
                {mode === 'login' ? (
                  <LoginPage onSuccess={handleSuccess} switchToRegister={() => setMode('register')} />
                ) : (
                  <RegisterPage onSuccess={handleSuccess} switchToLogin={() => setMode('login')} />
                )}
              </AuthCard>
            </div>
          </>
        }
      />
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}

export default function App() {
  return (
    <BrowserRouter>
      <AppContent />
    </BrowserRouter>
  );
}

