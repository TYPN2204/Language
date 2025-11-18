import { useState, useEffect } from 'react';
import './SlotMachine.css';

interface SlotMachineProps {
  onSelectGame: (gameName: string) => void;
  onCancel: () => void;
}

const GAMES = [
  'Matching Cards',
  'Word Puzzle',
  'Memory Game',
  'Spelling Bee',
  'Grammar Challenge',
  'Vocabulary Quiz'
];

export function SlotMachine({ onSelectGame, onCancel }: SlotMachineProps) {
  const [isSpinning, setIsSpinning] = useState(true);
  const [currentIndex, setCurrentIndex] = useState(0);
  const [selectedGame, setSelectedGame] = useState<string | null>(null);

  useEffect(() => {
    if (!isSpinning) return;

    const interval = setInterval(() => {
      setCurrentIndex((prev) => (prev + 1) % GAMES.length);
    }, 100); // Cuộn nhanh mỗi 100ms

    // Dừng sau 2-3 giây
    const stopTimeout = setTimeout(() => {
      setIsSpinning(false);
      // Chọn game ngẫu nhiên
      const randomIndex = Math.floor(Math.random() * GAMES.length);
      setCurrentIndex(randomIndex);
      setSelectedGame(GAMES[randomIndex]);
    }, 2500 + Math.random() * 500); // 2.5-3 giây

    return () => {
      clearInterval(interval);
      clearTimeout(stopTimeout);
    };
  }, [isSpinning]);

  const handlePlay = () => {
    if (selectedGame) {
      onSelectGame(selectedGame);
    }
  };

  return (
    <div className="slot-machine-container">
      <div className="slot-machine">
        <h2>🎰 Slot Machine</h2>
        <div className="slot-display">
          <div className={`slot-reel ${isSpinning ? 'spinning' : ''}`}>
            {GAMES.map((game, index) => (
              <div
                key={index}
                className={`slot-item ${
                  index === currentIndex ? 'active' : ''
                }`}
              >
                {game}
              </div>
            ))}
          </div>
        </div>
        {selectedGame && !isSpinning && (
          <div className="slot-result">
            <p className="result-text">🎉 Game được chọn:</p>
            <p className="game-name">{selectedGame}</p>
            <div className="slot-actions">
              <button className="btn-play" onClick={handlePlay}>
                Chơi game
              </button>
              <button className="btn-cancel" onClick={onCancel}>
                Để sau
              </button>
            </div>
          </div>
        )}
        {isSpinning && (
          <p className="spinning-text">Đang quay số...</p>
        )}
      </div>
    </div>
  );
}

