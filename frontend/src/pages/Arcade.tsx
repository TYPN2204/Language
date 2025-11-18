import { useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { GameplayApi } from '../api/gameplay';
import type { AuthResponse } from '../types/auth';
import type { StudentStatusResponse } from '../types/gameplay';
import { StatusCard } from '../components/StatusCard';
import { SlotMachine } from '../components/SlotMachine';

interface ArcadeProps {
  auth: AuthResponse;
}

export function Arcade({ auth }: ArcadeProps) {
  const navigate = useNavigate();
  const [status, setStatus] = useState<StudentStatusResponse | null>(null);
  const [feedback, setFeedback] = useState<string | null>(null);
  const [isLoadingStatus, setIsLoadingStatus] = useState(false);
  const [isSpinning, setIsSpinning] = useState(false);
  const [tickets, setTickets] = useState(0);

  const hocSinhId = auth.hocSinhId;

  useEffect(() => {
    const loadStatus = async () => {
      setIsLoadingStatus(true);
      try {
        const statusResponse = await GameplayApi.getStatus(hocSinhId);
        setStatus(statusResponse);
        setTickets(statusResponse.soVeChoiGame);
        setFeedback(statusResponse.message ?? null);
      } catch (error) {
        console.error(error);
        setFeedback('Không thể tải dữ liệu. Vui lòng kiểm tra API.');
      } finally {
        setIsLoadingStatus(false);
      }
    };

    loadStatus();
  }, [hocSinhId]);

  const refreshStatus = async () => {
    setIsLoadingStatus(true);
    try {
      const latest = await GameplayApi.getStatus(hocSinhId);
      setStatus(latest);
      setTickets(latest.soVeChoiGame);
      setFeedback(latest.message ?? 'Đã cập nhật trạng thái mới nhất.');
    } catch (error) {
      console.error(error);
      setFeedback('Không thể đồng bộ trạng thái.');
    } finally {
      setIsLoadingStatus(false);
    }
  };

  const handleSpinSlot = async () => {
    if (tickets < 1) {
      setFeedback('Bạn không có vé chơi game. Hãy mua vé tại cửa hàng!');
      return;
    }

    setIsLoadingStatus(true);
    try {
      // Sử dụng vé
      const ticketResponse = await GameplayApi.useTicket({ hocSinhId });
      setTickets(ticketResponse.soVeChoiGame);
      
      // Cập nhật status
      const updatedStatus = await GameplayApi.getStatus(hocSinhId);
      setStatus(updatedStatus);
      
      // Bắt đầu quay slot machine
      setIsSpinning(true);
      setFeedback(null);
    } catch (error: any) {
      console.error(error);
      setFeedback(error.response?.data?.message || 'Không thể sử dụng vé. Vui lòng thử lại.');
      setIsLoadingStatus(false);
    }
  };

  const handleSelectGame = (gameName: string) => {
    // Chuyển đến trang game tương ứng
    if (gameName === 'Matching Cards') {
      navigate('/games/matching-cards');
    } else {
      // Các game khác sẽ được implement sau
      setFeedback(`Game "${gameName}" đang được phát triển. Vui lòng thử lại sau!`);
      setIsSpinning(false);
    }
  };

  const handleCancelSlot = () => {
    setIsSpinning(false);
    setFeedback('Đã hủy quay số.');
  };

  return (
    <div className="page-container">
      <header className="page-header">
        <button className="back-button" onClick={() => navigate('/town')}>
          ← Về thị trấn
        </button>
        <h1>🎮 Sân chơi Arcade</h1>
      </header>

      <div className="page-content">
        <div className="city-hud">
          <StatusCard status={status} onRefresh={refreshStatus} isLoading={isLoadingStatus} />
          {feedback && <div className="panel info-panel">{feedback}</div>}
        </div>

        <div className="zone-content">
          {isSpinning ? (
            <div className="panel">
              <SlotMachine onSelectGame={handleSelectGame} onCancel={handleCancelSlot} />
            </div>
          ) : (
            <div className="panel arcade-panel">
              <header>
                <div>
                  <p className="eyebrow">Sân chơi Arcade</p>
                  <h2>Quay số chọn game! 🎰</h2>
                  <p className="muted small">
                    Sử dụng vé chơi game để quay số và chọn một mini-game ngẫu nhiên!
                  </p>
                </div>
              </header>

              <div className="arcade-info">
                <div className="ticket-display">
                  <span className="ticket-icon">🎫</span>
                  <span className="ticket-count">Bạn đang có: <strong>{tickets}</strong> vé chơi game</span>
                </div>
                {tickets < 1 && (
                  <p className="muted" style={{ color: '#fca5a5', marginTop: '1rem' }}>
                    ⚠️ Bạn không có vé. Hãy mua vé tại cửa hàng (50 💎 = 1 vé)!
                  </p>
                )}
              </div>

              <div className="arcade-actions">
                <button
                  className={`primary ${tickets < 1 ? 'disabled' : ''}`}
                  onClick={handleSpinSlot}
                  disabled={tickets < 1 || isLoadingStatus}
                >
                  {isLoadingStatus ? 'Đang xử lý...' : 'Sử dụng 1 vé và Quay số! 🎰'}
                </button>
                <button
                  className="secondary"
                  onClick={() => navigate('/shop')}
                  style={{ marginTop: '1rem' }}
                >
                  Mua vé tại cửa hàng →
                </button>
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
