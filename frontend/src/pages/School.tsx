import { useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { GameplayApi } from '../api/gameplay';
import type { AuthResponse } from '../types/auth';
import type {
  CourseDto,
  LessonDetailResponse,
  StudentStatusResponse
} from '../types/gameplay';
import { StatusCard } from '../components/StatusCard';
import { LearningPath } from '../components/LearningPath';
import { ExerciseHost } from '../components/ExerciseHost';
import type { ExerciseDto } from '../types/gameplay';

interface SchoolProps {
  auth: AuthResponse;
}

export function School({ auth }: SchoolProps) {
  const navigate = useNavigate();
  const [status, setStatus] = useState<StudentStatusResponse | null>(null);
  const [courses, setCourses] = useState<CourseDto[]>([]);
  const [feedback, setFeedback] = useState<string | null>(null);
  const [isLoadingStatus, setIsLoadingStatus] = useState(false);
  const [isProcessingLesson, setIsProcessingLesson] = useState(false);
  const [lessonDetail, setLessonDetail] = useState<LessonDetailResponse | null>(null);
  const [lessonExerciseIndex, setLessonExerciseIndex] = useState(0);
  const [answeredExercises, setAnsweredExercises] = useState<number[]>([]);
  const [correctExercises, setCorrectExercises] = useState<Set<number>>(new Set());
  const [wrongExercises, setWrongExercises] = useState<number[]>([]); // Danh sách câu sai để làm lại
  const [isReviewMode, setIsReviewMode] = useState(false); // Chế độ làm lại câu sai
  const [lessonFeedback, setLessonFeedback] = useState<string | null>(null);
  const [lessonFeedbackType, setLessonFeedbackType] = useState<'success' | 'error' | null>(null);
  const [lessonLoading, setLessonLoading] = useState(false);
  const [hearts, setHearts] = useState(3);
  const [isGameOver, setIsGameOver] = useState(false);
  const [isLessonCompleted, setIsLessonCompleted] = useState(false);

  const hocSinhId = auth.hocSinhId;

  useEffect(() => {
    const loadInitial = async () => {
      setIsLoadingStatus(true);
      try {
        const [statusResponse, courseResponse] = await Promise.all([
          GameplayApi.getStatus(hocSinhId),
          GameplayApi.getCourses()
        ]);
        setStatus(statusResponse);
        setCourses(courseResponse);
        setFeedback(statusResponse.message ?? null);
        console.log('Loaded courses:', courseResponse);
        console.log('Loaded status:', statusResponse);
      } catch (error) {
        console.error('Error loading school data:', error);
        setFeedback('Không thể tải dữ liệu. Vui lòng kiểm tra API và đảm bảo backend đang chạy.');
      } finally {
        setIsLoadingStatus(false);
      }
    };

    loadInitial();
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

  const handleCompleteLesson = async (lessonId: number) => {
    setIsProcessingLesson(true);
    try {
      // Nếu đang trong lesson, dùng số tim hiện tại, nếu không thì mặc định 3
      const remainingHearts = hearts > 0 ? hearts : 3;
      const updatedStatus = await GameplayApi.completeLesson({
        hocSinhId,
        baiHocId: lessonId,
        diemSo: 100,
        remainingHearts
      });
      setStatus(updatedStatus);
      setFeedback(updatedStatus.message ?? 'Hoàn thành bài học!');
    } catch (error) {
      console.error(error);
      setFeedback('Không thể hoàn thành bài học. Có thể bạn đã học bài này.');
    } finally {
      setIsProcessingLesson(false);
    }
  };

  const handleSelectLesson = async (lessonId: number) => {
    setLessonLoading(true);
    try {
      const detail = await GameplayApi.getLessonDetail(lessonId);
      setLessonDetail(detail);
      setLessonExerciseIndex(0);
      setAnsweredExercises([]);
      setCorrectExercises(new Set());
      setWrongExercises([]);
      setIsReviewMode(false);
      setHearts(3);
      setIsGameOver(false);
      setIsLessonCompleted(false);
      setLessonFeedback(`Bắt đầu bài "${detail.tenBaiHoc}". Bạn có 3 ❤️`);
      setLessonFeedbackType(null);
    } catch (error) {
      console.error(error);
      setLessonFeedback('Không tải được nội dung bài học.');
      setLessonFeedbackType('error');
    } finally {
      setLessonLoading(false);
    }
  };

  // Lấy danh sách exercises hiện tại (có thể là exercises gốc hoặc câu sai để làm lại)
  const getCurrentExercises = (): ExerciseDto[] => {
    if (!lessonDetail) return [];
    if (isReviewMode && wrongExercises.length > 0) {
      // Chế độ làm lại: chỉ hiển thị các câu sai
      return lessonDetail.exercises.filter((ex) => wrongExercises.includes(ex.cauHoiId));
    }
    return lessonDetail.exercises;
  };

  const currentExercises = getCurrentExercises();
  const currentExercise = currentExercises[lessonExerciseIndex < 0 ? 0 : lessonExerciseIndex] ?? null;

  const handleSubmitAnswer = async (answer: string) => {
    if (!currentExercise || isGameOver || isLessonCompleted || isProcessingLesson) {
      return;
    }
    
    // Không cho phép trả lời lại câu đã trả lời đúng
    if (correctExercises.has(currentExercise.cauHoiId)) {
      return;
    }

    setIsProcessingLesson(true);
    try {
      const response = await GameplayApi.submitAnswer({
        hocSinhId,
        cauHoiId: currentExercise.cauHoiId,
        traLoi: answer
      });
      
      const isCorrect = response.correct;
      
      if (isCorrect) {
        setCorrectExercises((prev) => new Set([...prev, currentExercise.cauHoiId]));
        // Xóa khỏi danh sách câu sai nếu có
        setWrongExercises((prev) => prev.filter((id) => id !== currentExercise.cauHoiId));
        setLessonFeedbackType('success');
        setLessonFeedback(
          `✅ Đúng rồi! +${response.awardedGems} 💎, +${response.awardedEnergy}% năng lượng. ${response.explanation}`
        );
        setStatus((prev) =>
          prev
            ? {
                ...prev,
                tongDiem: response.totalGems,
                nangLuongGioChoi: response.totalEnergy
              }
            : prev
        );

        // Kiểm tra xem đã trả lời đúng tất cả exercises chưa
        const allExercises = getCurrentExercises();
        const newCorrectSet = new Set([...correctExercises, currentExercise.cauHoiId]);
        const allAnswered = answeredExercises.length + 1 >= allExercises.length;
        const allCorrect = allExercises.every((ex) => newCorrectSet.has(ex.cauHoiId));

        if (allAnswered && allCorrect) {
          // Kiểm tra xem có câu sai nào cần làm lại không
          if (!isReviewMode && wrongExercises.length > 0) {
            // Chuyển sang chế độ làm lại câu sai
            setTimeout(() => {
              setIsReviewMode(true);
              setLessonExerciseIndex(0);
              setAnsweredExercises([]);
              setLessonFeedback('🔄 Bắt đầu làm lại các câu sai...');
              setLessonFeedbackType(null);
            }, 2000);
          } else {
            // Hoàn thành bài học
            setTimeout(async () => {
              try {
                const updatedStatus = await GameplayApi.completeLesson({
                  hocSinhId,
                  baiHocId: lessonDetail!.baiHocId,
                  diemSo: 100,
                  remainingHearts: hearts
                });
                setStatus(updatedStatus);
                setIsLessonCompleted(true);
                setLessonFeedback(updatedStatus.message || '🎉 Chúc mừng! Bạn đã hoàn thành bài học!');
                setLessonFeedbackType('success');
              } catch (error) {
                console.error(error);
                setLessonFeedback('Hoàn thành bài học nhưng không thể cập nhật trạng thái.');
              }
            }, 2000);
          }
        } else {
          // Tự động chuyển exercise sau 2 giây
          setTimeout(() => {
            setLessonExerciseIndex((index) => {
              const nextIndex = index + 1;
              if (nextIndex >= allExercises.length) {
                return index;
              }
              return nextIndex;
            });
            setLessonFeedback(null);
            setLessonFeedbackType(null);
          }, 2000);
        }
      } else {
        // Câu trả lời sai
        const newHearts = hearts - 1;
        setHearts(newHearts);
        
        // Thêm vào danh sách câu sai (nếu chưa có)
        if (!wrongExercises.includes(currentExercise.cauHoiId)) {
          setWrongExercises((prev) => [...prev, currentExercise.cauHoiId]);
        }
        
        setLessonFeedbackType('error');
        setLessonFeedback(
          `❌ Chưa đúng. ${response.explanation} (Còn ${newHearts} ❤️)`
        );

        if (newHearts <= 0) {
          setIsGameOver(true);
          setTimeout(() => {
            setLessonFeedback('💔 Bạn đã hết mạng! Bài học đã bị khóa. Hãy thử lại sau.');
            setLessonFeedbackType('error');
          }, 2000);
        } else {
          // Tự động chuyển exercise sau 2 giây
          setTimeout(() => {
            setLessonExerciseIndex((index) => {
              const nextIndex = index + 1;
              const allExercises = getCurrentExercises();
              if (nextIndex >= allExercises.length) {
                return index;
              }
              return nextIndex;
            });
            setLessonFeedback(null);
            setLessonFeedbackType(null);
          }, 2000);
        }
      }
      
      // Đánh dấu đã trả lời
      if (!answeredExercises.includes(currentExercise.cauHoiId)) {
        setAnsweredExercises((prev) => [...prev, currentExercise.cauHoiId]);
      }
    } catch (error) {
      console.error(error);
      setLessonFeedbackType('error');
      setLessonFeedback('Không thể chấm câu trả lời.');
    } finally {
      setIsProcessingLesson(false);
    }
  };

  return (
    <div className="page-container">
      <header className="page-header">
        <button className="back-button" onClick={() => navigate('/town')}>
          ← Về thị trấn
        </button>
        <h1>🏫 Trường học</h1>
      </header>

      <div className="page-content">
        <div className="city-hud">
          <StatusCard status={status} onRefresh={refreshStatus} isLoading={isLoadingStatus} />
          {feedback && <div className="panel info-panel">{feedback}</div>}
        </div>

        <div className="zone-content learning-path-layout">
          <div className="learning-path-section">
            <LearningPath
              courses={courses}
              hocSinhId={hocSinhId}
              onSelectLesson={handleSelectLesson}
            />
          </div>
          {lessonDetail ? (
            <div className="panel lesson-panel">
              <header>
                <div>
                  <p className="eyebrow">Quiz bài học</p>
                  <h2>{lessonDetail.tenBaiHoc}</h2>
                  <p className="muted small">{lessonDetail.courseName}</p>
                </div>
              </header>
              {/* Hiển thị số trái tim */}
              {lessonDetail && !lessonLoading && (
                <div className="hearts-display">
                  <span className="hearts-label">Mạng:</span>
                  <div className="hearts-container">
                    {[1, 2, 3].map((heart) => (
                      <span
                        key={heart}
                        className={`heart ${heart <= hearts ? 'active' : 'lost'}`}
                      >
                        ❤️
                      </span>
                    ))}
                  </div>
                </div>
              )}

              {lessonLoading && <p>Đang tải bài tập...</p>}
              {!lessonLoading && currentExercise && !isGameOver && !isLessonCompleted && (
                <>
                  <div className="exercise-header">
                    <p className="exercise-counter">
                      {isReviewMode ? '🔄 Làm lại' : 'Bài tập'} {lessonExerciseIndex + 1}/{currentExercises.length}
                      {isReviewMode && ` (${wrongExercises.length} câu sai)`}
                    </p>
                  </div>
                  <ExerciseHost
                    exercise={currentExercise}
                    onSubmit={handleSubmitAnswer}
                    isProcessing={isProcessingLesson}
                    isAnswered={correctExercises.has(currentExercise.cauHoiId)}
                  />
                </>
              )}
              {!lessonLoading && !currentExercise && lessonDetail && !isGameOver && !isLessonCompleted && (
                <div className="lesson-finished">
                  <p>Bạn đã trả lời hết bài tập cho bài này.</p>
                  <p className="muted">
                    {correctExercises.size === (lessonDetail.exercises.length)
                      ? 'Chúc mừng! Bạn đã trả lời đúng tất cả bài tập!'
                      : `Bạn đã trả lời đúng ${correctExercises.size}/${lessonDetail.exercises.length} bài tập.`}
                  </p>
                </div>
              )}
              {isGameOver && (
                <div className="lesson-game-over">
                  <h3>💔 Game Over!</h3>
                  <p>Bạn đã hết mạng. Bài học đã bị khóa.</p>
                  <button
                    className="primary"
                    onClick={() => handleSelectLesson(lessonDetail!.baiHocId)}
                  >
                    Thử lại
                  </button>
                </div>
              )}
              {isLessonCompleted && (
                <div className="lesson-completed">
                  <h3>🎉 Hoàn thành!</h3>
                  <p>Bạn đã hoàn thành bài học này.</p>
                </div>
              )}
              {lessonFeedback && (
                <div className={`quiz-feedback ${lessonFeedbackType ?? ''}`}>
                  <p>{lessonFeedback}</p>
                </div>
              )}
            </div>
          ) : (
            <div className="panel lesson-panel">
              <p>Chọn một bài học trên con đường học tập để bắt đầu.</p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}

