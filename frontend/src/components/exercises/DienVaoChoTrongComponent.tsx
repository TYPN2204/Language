import { useState, useRef, useEffect } from 'react';
import { ExerciseDto } from '../../types/gameplay';

interface DienVaoChoTrongComponentProps {
  exercise: ExerciseDto;
  onSubmit: (answer: string) => void;
  isProcessing: boolean;
  isAnswered: boolean;
}

export function DienVaoChoTrongComponent({
  exercise,
  onSubmit,
  isProcessing,
  isAnswered
}: DienVaoChoTrongComponentProps) {
  const [userAnswer, setUserAnswer] = useState('');
  const audioRef = useRef<HTMLAudioElement>(null);

  const handleSubmit = () => {
    if (userAnswer.trim() && !isProcessing && !isAnswered) {
      onSubmit(userAnswer.trim());
    }
  };

  const playAudio = () => {
    if (exercise.audioURL && audioRef.current) {
      audioRef.current.play().catch((error) => {
        console.error('Error playing audio:', error);
      });
    }
  };

  // Parse câu có chỗ trống từ NoiDung (format: "Câu có ___ chỗ trống")
  const sentence = exercise.noiDung || '';
  const blankCount = (sentence.match(/___/g) || []).length;

  return (
    <div className="exercise-dien-vao-cho-trong">
      <p className="exercise-instruction">Nghe và điền vào chỗ trống:</p>
      
      {exercise.audioURL && (
        <div className="audio-player">
          <button className="play-audio-button" onClick={playAudio} disabled={isProcessing || isAnswered}>
            🔊 Phát âm thanh
          </button>
          <audio ref={audioRef} src={exercise.audioURL} />
        </div>
      )}

      <div className="sentence-with-blanks">
        <p className="sentence-text">{sentence}</p>
      </div>

      <div className="fill-blank-input">
        <label htmlFor="fill-blank">Điền từ còn thiếu:</label>
        <input
          id="fill-blank"
          type="text"
          value={userAnswer}
          onChange={(e) => setUserAnswer(e.target.value)}
          onKeyDown={(e) => {
            if (e.key === 'Enter') {
              handleSubmit();
            }
          }}
          placeholder="Nhập từ cần điền..."
          disabled={isProcessing || isAnswered}
          className="fill-blank-text-input"
        />
        <button
          className="submit-fill-blank primary"
          onClick={handleSubmit}
          disabled={!userAnswer.trim() || isProcessing || isAnswered}
        >
          {isProcessing ? 'Đang kiểm tra...' : 'Gửi đáp án'}
        </button>
      </div>
    </div>
  );
}

