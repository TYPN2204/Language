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

