import { useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { GameplayApi } from '../api/gameplay';
import type { AuthResponse } from '../types/auth';
import type { RewardDto, StudentStatusResponse } from '../types/gameplay';
import { StatusCard } from '../components/StatusCard';
import { ShopPanel } from '../components/ShopPanel';

interface ShopProps {
  auth: AuthResponse;
}

export function Shop({ auth }: ShopProps) {
  const navigate = useNavigate();
  const [status, setStatus] = useState<StudentStatusResponse | null>(null);
  const [rewards, setRewards] = useState<RewardDto[]>([]);
  const [feedback, setFeedback] = useState<string | null>(null);
  const [isLoadingStatus, setIsLoadingStatus] = useState(false);
  const [isPurchasing, setIsPurchasing] = useState(false);

  const hocSinhId = auth.hocSinhId;

  useEffect(() => {
    const loadInitial = async () => {
      setIsLoadingStatus(true);
      try {
        const [statusResponse, rewardResponse] = await Promise.all([
          GameplayApi.getStatus(hocSinhId),
          GameplayApi.getRewards()
        ]);
        setStatus(statusResponse);
        setRewards(rewardResponse);
        setFeedback(statusResponse.message ?? null);
      } catch (error) {
        console.error(error);
        setFeedback('Không thể tải dữ liệu. Vui lòng kiểm tra API.');
      } finally {
        setIsLoadingStatus(false);
      }
    };

    loadInitial();
  }, [hocSinhId]);

  const refreshStatus = async () => {
    setIsLoadingStatus(true);
    try {
      const latest = await GameplayApi.getStatus(hocSinhId);
      setStatus(latest);
      setFeedback(latest.message ?? 'Đã cập nhật trạng thái mới nhất.');
    } catch (error) {
      console.error(error);
      setFeedback('Không thể đồng bộ trạng thái.');
    } finally {
      setIsLoadingStatus(false);
    }
  };

  const handlePurchase = async (rewardId: number) => {
    setIsPurchasing(true);
    try {
      const updatedStatus = await GameplayApi.purchaseReward({
        hocSinhId,
        phanThuongId: rewardId
      });
      setStatus(updatedStatus);
      setFeedback(updatedStatus.message ?? 'Đã mua vật phẩm!');
    } catch (error) {
      console.error(error);
      setFeedback('Mua vật phẩm thất bại. Kiểm tra số 💎 của bạn.');
    } finally {
      setIsPurchasing(false);
    }
  };

  const handleBuyTicket = async (quantity: number = 1) => {
    setIsPurchasing(true);
    try {
      const updatedStatus = await GameplayApi.buyTicket({
        hocSinhId,
        quantity
      });
      setStatus(updatedStatus);
      setFeedback(updatedStatus.message ?? 'Đã mua vé thành công!');
    } catch (error: any) {
      console.error(error);
      setFeedback(error.response?.data?.message || 'Mua vé thất bại. Kiểm tra số 💎 của bạn.');
    } finally {
      setIsPurchasing(false);
    }
  };

  return (
    <div className="page-container">
      <header className="page-header">
        <button className="back-button" onClick={() => navigate('/town')}>
          ← Về thị trấn
        </button>
        <h1>🛒 Cửa hàng</h1>
      </header>

      <div className="page-content">
        <div className="city-hud">
          <StatusCard status={status} onRefresh={refreshStatus} isLoading={isLoadingStatus} />
          {feedback && <div className="panel info-panel">{feedback}</div>}
        </div>

        <div className="zone-content">
          {/* Section mua vé */}
          <div className="panel" style={{ marginBottom: '1.5rem' }}>
            <header>
              <div>
                <p className="eyebrow">Đặc biệt</p>
                <h2>🎫 Vé Chơi Game</h2>
                <p className="muted small">
                  Mua vé để quay số và chọn mini-game tại Arcade! (50 💎 = 1 vé)
                </p>
              </div>
            </header>
            <div style={{ marginTop: '1rem' }}>
              <p className="muted">
                Bạn đang có: <strong>{status?.soVeChoiGame ?? 0} 🎫</strong> vé
              </p>
              <div style={{ display: 'flex', gap: '0.5rem', marginTop: '1rem' }}>
                <button
                  className="primary"
                  onClick={() => handleBuyTicket(1)}
                  disabled={isPurchasing || (status?.tongDiem ?? 0) < 50}
                >
                  Mua 1 vé (50 💎)
                </button>
                <button
                  className="secondary"
                  onClick={() => handleBuyTicket(2)}
                  disabled={isPurchasing || (status?.tongDiem ?? 0) < 100}
                >
                  Mua 2 vé (100 💎)
                </button>
                <button
                  className="secondary"
                  onClick={() => handleBuyTicket(5)}
                  disabled={isPurchasing || (status?.tongDiem ?? 0) < 250}
                >
                  Mua 5 vé (250 💎)
                </button>
              </div>
              {(status?.tongDiem ?? 0) < 50 && (
                <p className="muted" style={{ color: '#fca5a5', marginTop: '0.5rem' }}>
                  ⚠️ Bạn cần ít nhất 50 💎 để mua 1 vé
                </p>
              )}
            </div>
          </div>

          <ShopPanel
            rewards={rewards}
            owned={status?.inventory ?? []}
            onPurchase={handlePurchase}
            isPurchasing={isPurchasing}
          />
        </div>
      </div>
    </div>
  );
}

