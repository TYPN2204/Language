import { ExerciseDto } from '../types/gameplay';
import { TracNghiemComponent } from './exercises/TracNghiemComponent';
import { ChonCapComponent } from './exercises/ChonCapComponent';
import { DichCauComponent } from './exercises/DichCauComponent';
import { DienVaoChoTrongComponent } from './exercises/DienVaoChoTrongComponent';
import { SapXepTuComponent } from './exercises/SapXepTuComponent';

interface ExerciseHostProps {
  exercise: ExerciseDto;
  onSubmit: (answer: string) => void;
  isProcessing: boolean;
  isAnswered: boolean;
}

export function ExerciseHost({ exercise, onSubmit, isProcessing, isAnswered }: ExerciseHostProps) {
  const renderExercise = () => {
    switch (exercise.loaiCauHoi) {
      case 'TRAC_NGHIEM':
        return (
          <TracNghiemComponent
            exercise={exercise}
            onSubmit={onSubmit}
            isProcessing={isProcessing}
            isAnswered={isAnswered}
          />
        );
      case 'CHON_CAP':
        return (
          <ChonCapComponent
            exercise={exercise}
            onSubmit={onSubmit}
            isProcessing={isProcessing}
            isAnswered={isAnswered}
          />
        );
      case 'DICH_CAU':
        return (
          <DichCauComponent
            exercise={exercise}
            onSubmit={onSubmit}
            isProcessing={isProcessing}
            isAnswered={isAnswered}
          />
        );
      case 'DIEN_VAO_CHO_TRONG':
        return (
          <DienVaoChoTrongComponent
            exercise={exercise}
            onSubmit={onSubmit}
            isProcessing={isProcessing}
            isAnswered={isAnswered}
          />
        );
      case 'SAP_XEP_TU':
        return (
          <SapXepTuComponent
            exercise={exercise}
            onSubmit={onSubmit}
            isProcessing={isProcessing}
            isAnswered={isAnswered}
          />
        );
      default:
        return (
          <div className="exercise-error">
            <p>Loại bài tập không được hỗ trợ: {exercise.loaiCauHoi}</p>
          </div>
        );
    }
  };

  return <div className="exercise-host">{renderExercise()}</div>;
}

