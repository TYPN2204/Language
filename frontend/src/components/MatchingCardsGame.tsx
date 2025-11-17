import { useEffect, useState } from 'react';
import { GameplayApi } from '../api/gameplay';
import type { MatchingCardDto, MatchingGameDataResponse } from '../types/gameplay';
import './MatchingCardsGame.css';

interface MatchingCardsGameProps {
  hocSinhId: number;
  energySpent: number;
  onWin: (timeTaken: number, pairsMatched: number) => void;
  onCancel: () => void;
}

export function MatchingCardsGame({
  hocSinhId,
  energySpent,
  onWin,
  onCancel
}: MatchingCardsGameProps) {
  const [cards, setCards] = useState<MatchingCardDto[]>([]);
  const [flippedCards, setFlippedCards] = useState<number[]>([]);
  const [matchedPairs, setMatchedPairs] = useState<Set<number>>(new Set());
  const [isLoading, setIsLoading] = useState(true);
  const [startTime, setStartTime] = useState<number | null>(null);
  const [elapsedTime, setElapsedTime] = useState(0);
  const [gameWon, setGameWon] = useState(false);

  useEffect(() => {
    const loadGameData = async () => {
      setIsLoading(true);
      try {
        const data = await GameplayApi.getMatchingGameData();
        setCards(data.cards);
        setStartTime(Date.now());
      } catch (error) {
        console.error(error);
        alert('Không thể tải dữ liệu game. Vui lòng thử lại.');
        onCancel();
      } finally {
        setIsLoading(false);
      }
    };

    loadGameData();
  }, [onCancel]);

  // Timer
  useEffect(() => {
    if (!startTime || gameWon) return;

    const interval = setInterval(() => {
      setElapsedTime(Math.floor((Date.now() - startTime) / 1000));
    }, 1000);

    return () => clearInterval(interval);
  }, [startTime, gameWon]);

  const handleCardClick = (cardId: number) => {
    // Không cho phép click nếu đã có 2 thẻ đang lật hoặc thẻ đã được match
    if (flippedCards.length >= 2 || matchedPairs.has(cards.find((c) => c.id === cardId)?.pairId ?? 0)) {
      return;
    }

    // Không cho phép click lại thẻ đang lật
    if (flippedCards.includes(cardId)) {
      return;
    }

    const newFlipped = [...flippedCards, cardId];
    setFlippedCards(newFlipped);

    // Nếu đã lật 2 thẻ, kiểm tra xem có match không
    if (newFlipped.length === 2) {
      const card1 = cards.find((c) => c.id === newFlipped[0]);
      const card2 = cards.find((c) => c.id === newFlipped[1]);

      if (card1 && card2 && card1.pairId === card2.pairId) {
        // Match!
        const newMatchedPairs = new Set([...matchedPairs, card1.pairId]);
        setMatchedPairs(newMatchedPairs);
        setFlippedCards([]);

        // Kiểm tra xem đã thắng chưa
        setTimeout(() => {
          const allPairs = new Set(cards.map((c) => c.pairId));
          if (allPairs.size === newMatchedPairs.size) {
            // Đã match hết
            const timeTaken = Math.floor((Date.now() - (startTime ?? Date.now())) / 1000);
            setGameWon(true);
            onWin(timeTaken, newMatchedPairs.size);
          }
        }, 500);
      } else {
        // Không match, úp lại sau 1 giây
        setTimeout(() => {
          setFlippedCards([]);
        }, 1000);
      }
    }
  };

  if (isLoading) {
    return (
      <div className="matching-game-container">
        <p>Đang tải game...</p>
      </div>
    );
  }

  if (gameWon) {
    return (
      <div className="matching-game-container">
        <div className="game-won">
          <h2>🎉 Chúc mừng!</h2>
          <p>Bạn đã hoàn thành Matching Game!</p>
          <p className="stats">Thời gian: {elapsedTime} giây</p>
        </div>
      </div>
    );
  }

  const allPairs = new Set(cards.map((c) => c.pairId));
  const isAllMatched = allPairs.size === matchedPairs.size;

  return (
    <div className="matching-game-container">
      <div className="game-header">
        <div className="game-info">
          <span>⏱️ Thời gian: {elapsedTime}s</span>
          <span>🎯 Đã ghép: {matchedPairs.size}/{allPairs.size}</span>
          <span>⚡ Năng lượng: {energySpent}%</span>
        </div>
        <button className="cancel-button" onClick={onCancel}>
          ❌ Hủy
        </button>
      </div>

      <div className="cards-grid">
        {cards.map((card) => {
          const isFlipped = flippedCards.includes(card.id);
          const isMatched = matchedPairs.has(card.pairId);

          return (
            <button
              key={card.id}
              className={`matching-card ${isFlipped ? 'flipped' : ''} ${isMatched ? 'matched' : ''}`}
              onClick={() => handleCardClick(card.id)}
              disabled={isMatched}
            >
              <div className="card-front">?</div>
              <div className="card-back">
                <div className="card-emoji">{card.imageUrl}</div>
                <div className="card-text">{card.text}</div>
              </div>
            </button>
          );
        })}
      </div>

      {isAllMatched && (
        <div className="game-complete">
          <p>🎉 Hoàn thành! Đang xử lý phần thưởng...</p>
        </div>
      )}
    </div>
  );
}

