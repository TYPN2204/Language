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
import { LessonList } from '../components/LessonList';

interface SchoolProps {
  auth: AuthResponse;
}

type AnswerOption = 'A' | 'B' | 'C' | 'D';

export function School({ auth }: SchoolProps) {
  const navigate = useNavigate();
  const [status, setStatus] = useState<StudentStatusResponse | null>(null);
  const [courses, setCourses] = useState<CourseDto[]>([]);
  const [feedback, setFeedback] = useState<string | null>(null);
  const [isLoadingStatus, setIsLoadingStatus] = useState(false);
  const [isProcessingLesson, setIsProcessingLesson] = useState(false);
  const [lessonDetail, setLessonDetail] = useState<LessonDetailResponse | null>(null);
  const [lessonQuestionIndex, setLessonQuestionIndex] = useState(0);
  const [answeredQuestions, setAnsweredQuestions] = useState<number[]>([]);
  const [correctAnswers, setCorrectAnswers] = useState<Set<number>>(new Set());
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
      } catch (error) {
        console.error(error);
        setFeedback('Không thể tải dữ liệu. Vui lòng kiểm tra API.');
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
      setLessonQuestionIndex(0);
      setAnsweredQuestions([]);
      setCorrectAnswers(new Set());
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

  const currentQuestion =
    lessonDetail?.questions[lessonQuestionIndex < 0 ? 0 : lessonQuestionIndex] ?? null;

  const handleSubmitAnswer = async (option: AnswerOption) => {
    if (!currentQuestion || isGameOver || isLessonCompleted || isProcessingLesson) {
      return;
    }
    
    // Không cho phép trả lời lại câu đã trả lời
    if (answeredQuestions.includes(currentQuestion.cauHoiId)) {
      return;
    }

    setIsProcessingLesson(true);
    try {
      const response = await GameplayApi.submitAnswer({
        hocSinhId,
        cauHoiId: currentQuestion.cauHoiId,
        traLoi: option
      });
      
      const isCorrect = response.correct;
      setAnsweredQuestions((prev) => [...prev, currentQuestion.cauHoiId]);

      if (isCorrect) {
        setCorrectAnswers((prev) => new Set([...prev, currentQuestion.cauHoiId]));
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

        // Kiểm tra xem đã trả lời đúng tất cả câu hỏi chưa
        const allQuestions = lessonDetail?.questions.map((q) => q.cauHoiId) ?? [];
        const newCorrectSet = new Set([...correctAnswers, currentQuestion.cauHoiId]);
        const allAnswered = answeredQuestions.length + 1 >= allQuestions.length;
        const allCorrect = allQuestions.every((id) => newCorrectSet.has(id));

        if (allAnswered && allCorrect) {
          // Tự động hoàn thành bài học
          setTimeout(async () => {
            try {
              const updatedStatus = await GameplayApi.completeLesson({
                hocSinhId,
                baiHocId: lessonDetail!.baiHocId,
                diemSo: 100,
                remainingHearts: hearts // Gửi số tim còn lại
              });
              setStatus(updatedStatus);
              setIsLessonCompleted(true);
              // Message từ backend sẽ có format: "Tuyệt vời! Bạn nhận được X 💎!"
              setLessonFeedback(updatedStatus.message || '🎉 Chúc mừng! Bạn đã hoàn thành bài học!');
              setLessonFeedbackType('success');
            } catch (error) {
              console.error(error);
              setLessonFeedback('Hoàn thành bài học nhưng không thể cập nhật trạng thái.');
            }
          }, 2000);
        } else {
          // Tự động chuyển câu hỏi sau 2 giây
          setTimeout(() => {
            setLessonQuestionIndex((index) => {
              const nextIndex = index + 1;
              if (nextIndex >= allQuestions.length) {
                return index; // Giữ nguyên nếu đã hết câu hỏi
              }
              return nextIndex;
            });
            setLessonFeedback(null);
            setLessonFeedbackType(null);
          }, 2000);
        }
      } else {
        // Câu trả lời sai - mất 1 trái tim
        const newHearts = hearts - 1;
        setHearts(newHearts);
        setLessonFeedbackType('error');
        setLessonFeedback(
          `❌ Chưa đúng. ${response.explanation} (Còn ${newHearts} ❤️)`
        );

        if (newHearts <= 0) {
          // Game over - khóa bài học
          setIsGameOver(true);
          setTimeout(() => {
            setLessonFeedback('💔 Bạn đã hết mạng! Bài học đã bị khóa. Hãy thử lại sau.');
            setLessonFeedbackType('error');
          }, 2000);
        } else {
          // Tự động chuyển câu hỏi sau 2 giây
          setTimeout(() => {
            setLessonQuestionIndex((index) => {
              const nextIndex = index + 1;
              const allQuestions = lessonDetail?.questions.map((q) => q.cauHoiId) ?? [];
              if (nextIndex >= allQuestions.length) {
                return index; // Giữ nguyên nếu đã hết câu hỏi
              }
              return nextIndex;
            });
            setLessonFeedback(null);
            setLessonFeedbackType(null);
          }, 2000);
        }
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

        <div className="zone-content two-column">
          <LessonList
            courses={courses}
            onCompleteLesson={handleCompleteLesson}
            isProcessing={isProcessingLesson}
            onSelectLesson={handleSelectLesson}
          />
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

              {lessonLoading && <p>Đang tải câu hỏi...</p>}
              {!lessonLoading && currentQuestion && !isGameOver && !isLessonCompleted && (
                <>
                  <p className="question">
                    Câu {lessonQuestionIndex + 1}/{lessonDetail?.questions.length ?? 0}: {currentQuestion.noiDung}
                  </p>
                  <div className="options-grid">
                    {[
                      { key: 'A', text: currentQuestion.phuongAnA },
                      { key: 'B', text: currentQuestion.phuongAnB },
                      { key: 'C', text: currentQuestion.phuongAnC },
                      { key: 'D', text: currentQuestion.phuongAnD }
                    ].map((option) => {
                      const isAnswered = answeredQuestions.includes(currentQuestion.cauHoiId);
                      return (
                        <button
                          key={option.key}
                          className="option-card"
                          onClick={() => handleSubmitAnswer(option.key as AnswerOption)}
                          disabled={isProcessingLesson || isAnswered}
                        >
                          <strong>{option.key}.</strong> {option.text}
                        </button>
                      );
                    })}
                  </div>
                </>
              )}
              {!lessonLoading && !currentQuestion && lessonDetail && !isGameOver && !isLessonCompleted && (
                <div className="lesson-finished">
                  <p>Bạn đã trả lời hết câu hỏi cho bài này.</p>
                  <p className="muted">
                    {correctAnswers.size === (lessonDetail.questions.length)
                      ? 'Chúc mừng! Bạn đã trả lời đúng tất cả câu hỏi!'
                      : `Bạn đã trả lời đúng ${correctAnswers.size}/${lessonDetail.questions.length} câu hỏi.`}
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
              <p>Chọn một bài học ở bên trái để bắt đầu.</p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}

