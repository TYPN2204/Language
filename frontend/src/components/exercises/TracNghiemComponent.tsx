import { ExerciseDto } from '../../types/gameplay';

interface TracNghiemComponentProps {
  exercise: ExerciseDto;
  onSubmit: (answer: string) => void;
  isProcessing: boolean;
  isAnswered: boolean;
}

type AnswerOption = 'A' | 'B' | 'C' | 'D';

export function TracNghiemComponent({
  exercise,
  onSubmit,
  isProcessing,
  isAnswered
}: TracNghiemComponentProps) {
  const handleOptionClick = (option: AnswerOption) => {
    if (!isProcessing && !isAnswered) {
      onSubmit(option);
    }
  };

  const options = [
    { key: 'A' as AnswerOption, text: exercise.phuongAnA },
    { key: 'B' as AnswerOption, text: exercise.phuongAnB },
    { key: 'C' as AnswerOption, text: exercise.phuongAnC },
    { key: 'D' as AnswerOption, text: exercise.phuongAnD }
  ].filter((opt) => opt.text); // Lọc bỏ các option null

  return (
    <div className="exercise-trac-nghiem">
      <p className="exercise-question">{exercise.noiDung}</p>
      <div className="options-grid">
        {options.map((option) => (
          <button
            key={option.key}
            className="option-card"
            onClick={() => handleOptionClick(option.key)}
            disabled={isProcessing || isAnswered}
          >
            <strong>{option.key}.</strong> {option.text}
          </button>
        ))}
      </div>
    </div>
  );
}

