import { useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { GameplayApi } from '../api/gameplay';
import type { AuthResponse } from '../types/auth';
import type { StudentStatusResponse } from '../types/gameplay';
import { StatusCard } from '../components/StatusCard';
import { MatchingCardsGame } from '../components/MatchingCardsGame';

interface ArcadeProps {
  auth: AuthResponse;
}

export function Arcade({ auth }: ArcadeProps) {
  const navigate = useNavigate();
  const [status, setStatus] = useState<StudentStatusResponse | null>(null);
  const [feedback, setFeedback] = useState<string | null>(null);
  const [isLoadingStatus, setIsLoadingStatus] = useState(false);
  const [isPlayingGame, setIsPlayingGame] = useState(false);
  const [energySpent, setEnergySpent] = useState(0);

  const hocSinhId = auth.hocSinhId;

  useEffect(() => {
    const loadStatus = async () => {
      setIsLoadingStatus(true);
      try {
        const statusResponse = await GameplayApi.getStatus(hocSinhId);
        setStatus(statusResponse);
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
      setFeedback(latest.message ?? 'Đã cập nhật trạng thái mới nhất.');
    } catch (error) {
      console.error(error);
      setFeedback('Không thể đồng bộ trạng thái.');
    } finally {
      setIsLoadingStatus(false);
    }
  };

  const handleStartGame = (energy: number) => {
    setEnergySpent(energy);
    setIsPlayingGame(true);
    setFeedback(null);
  };

  const handleGameWin = async (timeTaken: number, pairsMatched: number) => {
    try {
      const updatedStatus = await GameplayApi.matchingGameWin({
        hocSinhId,
        energySpent,
        timeTaken,
        pairsMatched
      });
      setStatus(updatedStatus);
      setFeedback(updatedStatus.message ?? '🎉 Chúc mừng! Bạn đã thắng Matching Game!');
      setIsPlayingGame(false);
      setEnergySpent(0);
    } catch (error) {
      console.error(error);
      setFeedback('Không thể cập nhật phần thưởng. Vui lòng thử lại.');
      setIsPlayingGame(false);
    }
  };

  const handleCancelGame = () => {
    setIsPlayingGame(false);
    setEnergySpent(0);
    setFeedback('Đã hủy game.');
  };

  const currentEnergy = status?.nangLuongGioChoi ?? 0;
  const spendableEnergy = Math.max(0, currentEnergy - (currentEnergy % 5));

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
          {isPlayingGame ? (
            <div className="panel">
              <MatchingCardsGame
                hocSinhId={hocSinhId}
                energySpent={energySpent}
                onWin={handleGameWin}
                onCancel={handleCancelGame}
              />
            </div>
          ) : (
            <div className="panel arcade-panel">
              <header>
                <div>
                  <p className="eyebrow">Sân chơi Arcade</p>
                  <h2>Matching Cards Game</h2>
                  <p className="muted small">
                    Lật các thẻ để tìm cặp từ vựng phù hợp. Thắng game để nhận 💎!
                  </p>
                </div>
              </header>

              <div className="arcade-info">
                <p className="muted">
                  Bạn đang có <strong>{currentEnergy}%</strong> năng lượng.
                  Mỗi lượt chơi tiêu hao 5% năng lượng (bội số của 5).
                </p>
                {spendableEnergy < 5 && (
                  <p className="muted" style={{ color: '#fca5a5' }}>
                    ⚠️ Hoàn thành thêm bài học để nạp năng lượng nhé!
                  </p>
                )}
              </div>

              <div className="energy-buttons">
                {[5, 10, 15, 20, 25].map((energy) => {
                  const canAfford = energy <= spendableEnergy;
                  return (
                    <button
                      key={energy}
                      className={`energy-button ${canAfford ? 'affordable' : 'disabled'}`}
                      onClick={() => canAfford && handleStartGame(energy)}
                      disabled={!canAfford}
                    >
                      {energy}% ⚡
                      <span className="energy-hint">Chơi với {energy}% năng lượng</span>
                    </button>
                  );
                })}
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}

