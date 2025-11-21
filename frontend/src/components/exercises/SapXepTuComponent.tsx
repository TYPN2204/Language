import { useState, useEffect } from 'react';
import { ExerciseDto } from '../../types/gameplay';
import './SapXepTuComponent.css';

interface SapXepTuComponentProps {
  exercise: ExerciseDto;
  onSubmit: (answer: string) => void;
  isProcessing: boolean;
  isAnswered: boolean;
}

interface Word {
  id: number;
  text: string;
  selected: boolean;
}

export function SapXepTuComponent({
  exercise,
  onSubmit,
  isProcessing,
  isAnswered
}: SapXepTuComponentProps) {
  // Parse từ từ NoiDung (phân tách bằng dấu |)
  const parseWords = (): Word[] => {
    const words = exercise.noiDung.split('|').filter(Boolean);
    return words.map((text, index) => ({
      id: index,
      text: text.trim(),
      selected: false
    }));
  };

  const [availableWords, setAvailableWords] = useState<Word[]>([]);
  const [selectedOrder, setSelectedOrder] = useState<number[]>([]);

  useEffect(() => {
    // Shuffle words on mount
    const words = parseWords();
    const shuffled = [...words].sort(() => Math.random() - 0.5);
    setAvailableWords(shuffled);
  }, []);

  const handleWordClick = (wordId: number) => {
    if (isProcessing || isAnswered) return;

    const word = availableWords.find((w) => w.id === wordId);
    if (!word || word.selected) return;

    // Thêm từ vào thứ tự đã chọn
    setSelectedOrder((prev) => [...prev, wordId]);
    setAvailableWords((prev) =>
      prev.map((w) => (w.id === wordId ? { ...w, selected: true } : w))
    );
  };

  const handleRemoveWord = (index: number) => {
    if (isProcessing || isAnswered) return;

    const wordId = selectedOrder[index];
    setSelectedOrder((prev) => prev.filter((_, i) => i !== index));
    setAvailableWords((prev) =>
      prev.map((w) => (w.id === wordId ? { ...w, selected: false } : w))
    );
  };

  const handleSubmit = () => {
    if (selectedOrder.length === availableWords.length && !isProcessing && !isAnswered) {
      // Tạo chuỗi đáp án từ thứ tự đã chọn
      const answer = selectedOrder.map((id) => availableWords.find((w) => w.id === id)?.text).join(' ');
      onSubmit(answer);
    }
  };

  const selectedWords = selectedOrder.map((id) => availableWords.find((w) => w.id === id)).filter(Boolean) as Word[];

  return (
    <div className="exercise-sap-xep-tu">
      <p className="exercise-instruction">Sắp xếp các từ sau thành câu đúng:</p>
      
      <div className="available-words">
        {availableWords.map((word) => (
          <button
            key={word.id}
            className={`word-chip ${word.selected ? 'selected' : ''}`}
            onClick={() => handleWordClick(word.id)}
            disabled={isProcessing || isAnswered || word.selected}
          >
            {word.text}
          </button>
        ))}
      </div>

      <div className="selected-sentence">
        <p className="sentence-label">Câu của bạn:</p>
        <div className="sentence-words">
          {selectedWords.length === 0 ? (
            <p className="empty-sentence">Chọn các từ ở trên để tạo câu...</p>
          ) : (
            selectedWords.map((word, index) => (
              <span key={word.id} className="sentence-word">
                <span className="word-text">{word.text}</span>
                <button
                  className="remove-word"
                  onClick={() => handleRemoveWord(index)}
                  disabled={isProcessing || isAnswered}
                >
                  ×
                </button>
              </span>
            ))
          )}
        </div>
        {selectedWords.length > 0 && (
          <button
            className="submit-sentence primary"
            onClick={handleSubmit}
            disabled={isProcessing || isAnswered || selectedWords.length !== availableWords.length}
          >
            {isProcessing ? 'Đang kiểm tra...' : 'Gửi đáp án'}
          </button>
        )}
      </div>
    </div>
  );
}

