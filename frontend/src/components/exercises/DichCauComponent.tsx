import { useState } from 'react';
import { ExerciseDto } from '../../types/gameplay';

interface DichCauComponentProps {
  exercise: ExerciseDto;
  onSubmit: (answer: string) => void;
  isProcessing: boolean;
  isAnswered: boolean;
}

export function DichCauComponent({
  exercise,
  onSubmit,
  isProcessing,
  isAnswered
}: DichCauComponentProps) {
  const [userAnswer, setUserAnswer] = useState('');

  const handleSubmit = () => {
    if (userAnswer.trim() && !isProcessing && !isAnswered) {
      onSubmit(userAnswer.trim());
    }
  };

  const handleKeyPress = (e: React.KeyboardEvent<HTMLTextAreaElement>) => {
    if (e.key === 'Enter' && (e.ctrlKey || e.metaKey)) {
      handleSubmit();
    }
  };

  // Xác định hướng dịch (Anh -> Việt hoặc Việt -> Anh)
  const sourceText = exercise.cauTienAnh || exercise.cauTienViet || exercise.noiDung;
  const targetLanguage = exercise.cauTienAnh ? 'Tiếng Việt' : 'Tiếng Anh';
  const instruction = exercise.cauTienAnh 
    ? 'Dịch câu sau sang tiếng Việt:'
    : 'Dịch câu sau sang tiếng Anh:';

  return (
    <div className="exercise-dich-cau">
      <p className="exercise-instruction">{instruction}</p>
      <div className="source-sentence">
        <p className="sentence-text">{sourceText}</p>
      </div>
      <div className="translation-input">
        <label htmlFor="translation">Bản dịch ({targetLanguage}):</label>
        <textarea
          id="translation"
          value={userAnswer}
          onChange={(e) => setUserAnswer(e.target.value)}
          onKeyDown={handleKeyPress}
          placeholder={`Nhập bản dịch ${targetLanguage.toLowerCase()}...`}
          rows={4}
          disabled={isProcessing || isAnswered}
          className="translation-textarea"
        />
        <button
          className="submit-translation primary"
          onClick={handleSubmit}
          disabled={!userAnswer.trim() || isProcessing || isAnswered}
        >
          {isProcessing ? 'Đang kiểm tra...' : 'Gửi đáp án'}
        </button>
        <p className="hint">Nhấn Ctrl+Enter để gửi nhanh</p>
      </div>
    </div>
  );
}

