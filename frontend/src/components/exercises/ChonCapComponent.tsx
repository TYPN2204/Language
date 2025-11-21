import { useState, useEffect } from 'react';
import { ExerciseDto } from '../../types/gameplay';
import './ChonCapComponent.css';

interface ChonCapComponentProps {
  exercise: ExerciseDto;
  onSubmit: (answer: string) => void;
  isProcessing: boolean;
  isAnswered: boolean;
}

interface Pair {
  id: number;
  text: string;
  selected: boolean;
  matched: boolean;
}

export function ChonCapComponent({
  exercise,
  onSubmit,
  isProcessing,
  isAnswered
}: ChonCapComponentProps) {
  // Parse đáp án từ DapAnDung (format: JSON array hoặc string)
  const parsePairs = (): { left: Pair[]; right: Pair[] } => {
    // Giả sử NoiDung chứa danh sách từ/cụm từ, phân tách bằng dấu |
    // Format: "word1|word2|word3||translation1|translation2|translation3"
    const parts = exercise.noiDung.split('||');
    const leftWords = parts[0]?.split('|').filter(Boolean) || [];
    const rightWords = parts[1]?.split('|').filter(Boolean) || [];

    const left: Pair[] = leftWords.map((text, index) => ({
      id: index,
      text: text.trim(),
      selected: false,
      matched: false
    }));

    const right: Pair[] = rightWords.map((text, index) => ({
      id: index + 100, // Offset để tránh trùng ID
      text: text.trim(),
      selected: false,
      matched: false
    }));

    return { left, right };
  };

  const [pairs, setPairs] = useState(parsePairs());
  const [selectedLeft, setSelectedLeft] = useState<number | null>(null);
  const [selectedRight, setSelectedRight] = useState<number | null>(null);
  const [matchedPairs, setMatchedPairs] = useState<Array<{ left: number; right: number }>>([]);

  // Shuffle arrays on mount
  useEffect(() => {
    const shuffle = (array: Pair[]) => {
      const shuffled = [...array];
      for (let i = shuffled.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [shuffled[i], shuffled[j]] = [shuffled[j], shuffled[i]];
      }
      return shuffled;
    };

    setPairs({
      left: shuffle(pairs.left),
      right: shuffle(pairs.right)
    });
  }, []);

  const handleLeftClick = (id: number) => {
    if (isProcessing || isAnswered || pairs.left.find((p) => p.id === id)?.matched) return;

    if (selectedLeft === id) {
      setSelectedLeft(null);
    } else {
      setSelectedLeft(id);
      // Nếu đã chọn bên phải, thử match
      if (selectedRight !== null) {
        checkMatch(id, selectedRight);
      }
    }
  };

  const handleRightClick = (id: number) => {
    if (isProcessing || isAnswered || pairs.right.find((p) => p.id === id)?.matched) return;

    if (selectedRight === id) {
      setSelectedRight(null);
    } else {
      setSelectedRight(id);
      // Nếu đã chọn bên trái, thử match
      if (selectedLeft !== null) {
        checkMatch(selectedLeft, id);
      }
    }
  };

  const checkMatch = (leftId: number, rightId: number) => {
    const leftIndex = pairs.left.findIndex((p) => p.id === leftId);
    const rightIndex = pairs.right.findIndex((p) => p.id === rightId);

    // Parse đáp án đúng từ DapAnDung (format: "0-100,1-101" nghĩa là left[0] match với right[0])
    const correctAnswer = exercise.dapAnDung || '';
    const expectedMatch = `${leftIndex}-${rightIndex}`;
    const isCorrect = correctAnswer.includes(expectedMatch) || 
                     correctAnswer === `${leftIndex}-${rightIndex - 100}`;

    if (isCorrect) {
      // Match đúng
      setPairs((prev) => ({
        left: prev.left.map((p) => (p.id === leftId ? { ...p, matched: true } : p)),
        right: prev.right.map((p) => (p.id === rightId ? { ...p, matched: true } : p))
      }));
      setMatchedPairs((prev) => [...prev, { left: leftId, right: rightId }]);
      setSelectedLeft(null);
      setSelectedRight(null);

      // Kiểm tra xem đã match hết chưa
      const allMatched = pairs.left.every((p) => 
        matchedPairs.some((mp) => mp.left === p.id) || p.id === leftId
      );
      if (allMatched) {
        onSubmit(correctAnswer);
      }
    } else {
      // Match sai - reset selection
      setSelectedLeft(null);
      setSelectedRight(null);
    }
  };

  return (
    <div className="exercise-chon-cap">
      <p className="exercise-instruction">{exercise.noiDung.split('||')[0] ? 'Chọn cặp từ đúng:' : exercise.noiDung}</p>
      <div className="pairs-container">
        <div className="pairs-column left-column">
          <h3>Tiếng Anh</h3>
          {pairs.left.map((pair) => (
            <button
              key={pair.id}
              className={`pair-item ${pair.matched ? 'matched' : ''} ${selectedLeft === pair.id ? 'selected' : ''}`}
              onClick={() => handleLeftClick(pair.id)}
              disabled={isProcessing || isAnswered || pair.matched}
            >
              {pair.text}
            </button>
          ))}
        </div>
        <div className="pairs-column right-column">
          <h3>Tiếng Việt</h3>
          {pairs.right.map((pair) => (
            <button
              key={pair.id}
              className={`pair-item ${pair.matched ? 'matched' : ''} ${selectedRight === pair.id ? 'selected' : ''}`}
              onClick={() => handleRightClick(pair.id)}
              disabled={isProcessing || isAnswered || pair.matched}
            >
              {pair.text}
            </button>
          ))}
        </div>
      </div>
    </div>
  );
}

