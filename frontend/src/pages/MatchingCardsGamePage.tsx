import { useNavigate } from 'react-router-dom';
import { useState } from 'react';
import { MatchingCardsGame } from '../components/MatchingCardsGame';
import type { AuthResponse } from '../types/auth';
import { GameplayApi } from '../api/gameplay';

interface MatchingCardsGamePageProps {
  auth: AuthResponse;
}

export function MatchingCardsGamePage({ auth }: MatchingCardsGamePageProps) {
  const navigate = useNavigate();
  const [isStarted, setIsStarted] = useState(false);
  const [showInstructions, setShowInstructions] = useState(false);
  const hocSinhId = auth.hocSinhId;

  const handleStart = () => {
    setIsStarted(true);
  };

  const handleGameWin = async (timeTaken: number, pairsMatched: number) => {
    try {
      // Gọi API để nhận phần thưởng (không cần energySpent nữa, đã dùng vé rồi)
      await GameplayApi.matchingGameWin({
        hocSinhId,
        energySpent: 0, // Tạm thời dùng 0, có thể sửa backend sau
        timeTaken,
        pairsMatched
      });
    } catch (error) {
      console.error(error);
    }
  };

  const handleCancel = () => {
    navigate('/arcade');
  };

  if (isStarted) {
    return (
      <div className="page-container">
        <MatchingCardsGame
          hocSinhId={hocSinhId}
          energySpent={0}
          onWin={handleGameWin}
          onCancel={handleCancel}
        />
      </div>
    );
  }

  return (
    <div className="page-container">
      <header className="page-header">
        <button className="back-button" onClick={() => navigate('/arcade')}>
          ← Về Arcade
        </button>
        <h1>🎮 Matching Cards Game</h1>
      </header>

      <div className="page-content">
        <div className="zone-content">
          <div className="panel">
            <header>
              <div>
                <p className="eyebrow">Mini-Game</p>
                <h2>Matching Cards</h2>
                <p className="muted small">
                  Lật các thẻ để tìm cặp từ vựng phù hợp. Hoàn thành càng nhanh, phần thưởng càng lớn!
                </p>
              </div>
            </header>

            {showInstructions ? (
              <div className="game-instructions">
                <h3>📖 Hướng dẫn chơi:</h3>
                <ol>
                  <li>Click vào các thẻ để lật chúng</li>
                  <li>Tìm 2 thẻ có nội dung phù hợp (cùng pairId)</li>
                  <li>Khi tìm được cặp, chúng sẽ được đánh dấu</li>
                  <li>Hoàn thành tất cả các cặp để thắng game</li>
                  <li>Thời gian càng nhanh, phần thưởng 💎 càng nhiều!</li>
                </ol>
                <button className="primary" onClick={() => setShowInstructions(false)}>
                  Đã hiểu!
                </button>
              </div>
            ) : (
              <div className="game-actions">
                <button className="primary" onClick={handleStart}>
                  Bắt đầu
                </button>
                <button className="secondary" onClick={() => setShowInstructions(true)}>
                  Hướng dẫn
                </button>
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}

