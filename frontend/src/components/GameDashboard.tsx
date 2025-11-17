import { useEffect, useState } from 'react';
import { GameplayApi } from '../api/gameplay';
import type { AuthResponse } from '../types/auth';
import type {
  ChatbotResponse,
  CourseDto,
  LeaderboardEntryDto,
  LessonDetailResponse,
  RewardDto,
  StudentStatusResponse
} from '../types/gameplay';
import { StatusCard } from './StatusCard';
import { LessonList } from './LessonList';
import { ArcadePanel } from './ArcadePanel';
import { ShopPanel } from './ShopPanel';
import { LeaderboardPanel } from './LeaderboardPanel';
import { ChatbotPanel } from './ChatbotPanel';

type CityZone = 'school' | 'arcade' | 'shop' | 'leaderboard' | 'chatbot' | null;
type AnswerOption = 'A' | 'B' | 'C' | 'D';

interface GameDashboardProps {
  auth: AuthResponse;
}

export function GameDashboard({ auth }: GameDashboardProps) {
  const [status, setStatus] = useState<StudentStatusResponse | null>(null);
  const [courses, setCourses] = useState<CourseDto[]>([]);
  const [rewards, setRewards] = useState<RewardDto[]>([]);
  const [feedback, setFeedback] = useState<string | null>(null);
  const [isLoadingStatus, setIsLoadingStatus] = useState(false);
  const [isProcessingLesson, setIsProcessingLesson] = useState(false);
  const [isPlayingArcade, setIsPlayingArcade] = useState(false);
  const [isPurchasing, setIsPurchasing] = useState(false);
  const [leaderboard, setLeaderboard] = useState<LeaderboardEntryDto[]>([]);
  const [isLoadingLeaderboard, setIsLoadingLeaderboard] = useState(false);

  const [activeZone, setActiveZone] = useState<CityZone>(null);
  const [lessonDetail, setLessonDetail] = useState<LessonDetailResponse | null>(null);
  const [lessonQuestionIndex, setLessonQuestionIndex] = useState(0);
  const [answeredQuestions, setAnsweredQuestions] = useState<number[]>([]);
  const [lessonFeedback, setLessonFeedback] = useState<string | null>(null);
  const [lessonFeedbackType, setLessonFeedbackType] = useState<'success' | 'error' | null>(null);
  const [lessonLoading, setLessonLoading] = useState(false);
  const [chatbotLog, setChatbotLog] = useState<ChatbotResponse[]>([]);
  const [isAskingBot, setIsAskingBot] = useState(false);

  const hocSinhId = auth.hocSinhId;

  useEffect(() => {
    const loadInitial = async () => {
      setIsLoadingStatus(true);
      try {
        const [statusResponse, courseResponse, rewardResponse] = await Promise.all([
          GameplayApi.getStatus(hocSinhId),
          GameplayApi.getCourses(),
          GameplayApi.getRewards()
        ]);
        setStatus(statusResponse);
        setCourses(courseResponse);
        setRewards(rewardResponse);
        setFeedback(statusResponse.message ?? null);
      } catch (error) {
        console.error(error);
        setFeedback('Không thể tải dữ liệu gameplay. Vui lòng kiểm tra API.');
      } finally {
        setIsLoadingStatus(false);
      }

      await refreshLeaderboard();
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

  const refreshLeaderboard = async () => {
    setIsLoadingLeaderboard(true);
    try {
      const data = await GameplayApi.getLeaderboard();
      setLeaderboard(data);
    } catch (error) {
      console.error(error);
      setFeedback((prev) => prev ?? 'Không thể tải bảng xếp hạng.');
    } finally {
      setIsLoadingLeaderboard(false);
    }
  };

  const handleCompleteLesson = async (lessonId: number) => {
    setIsProcessingLesson(true);
    try {
      const updatedStatus = await GameplayApi.completeLesson({
        hocSinhId,
        baiHocId: lessonId,
        diemSo: 100
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

  const handlePlayArcade = async (energySpent: number) => {
    setIsPlayingArcade(true);
    try {
      const updatedStatus = await GameplayApi.playArcade({ hocSinhId, energySpent });
      setStatus(updatedStatus);
      setFeedback(updatedStatus.message ?? 'Bạn đã chơi arcade thành công!');
    } catch (error) {
      console.error(error);
      setFeedback('Không thể tham gia sân chơi. Kiểm tra năng lượng nhé!');
    } finally {
      setIsPlayingArcade(false);
    }
  };

  const handlePurchase = async (rewardId: number) => {
    setIsPurchasing(true);
    try {
      const updatedStatus = await GameplayApi.purchaseReward({
        hocSinhId,
        phanThuongId: rewardId
      });
      setStatus(updatedStatus);
      setFeedback(updatedStatus.message ?? 'Đã mua vật phẩm!');
    } catch (error) {
      console.error(error);
      setFeedback('Mua vật phẩm thất bại. Kiểm tra số 💎 của bạn.');
    } finally {
      setIsPurchasing(false);
    }
  };

  const handleSelectLesson = async (lessonId: number) => {
    setLessonLoading(true);
    try {
      const detail = await GameplayApi.getLessonDetail(lessonId);
      setLessonDetail(detail);
      setLessonQuestionIndex(0);
      setAnsweredQuestions([]);
      setLessonFeedback(`Bắt đầu bài "${detail.tenBaiHoc}".`);
      if (activeZone !== 'school') {
        setActiveZone('school');
      }
    } catch (error) {
      console.error(error);
      setLessonFeedback('Không tải được nội dung bài học.');
    } finally {
      setLessonLoading(false);
    }
  };

  const currentQuestion =
    lessonDetail?.questions[lessonQuestionIndex < 0 ? 0 : lessonQuestionIndex] ?? null;

  const handleSubmitAnswer = async (option: AnswerOption) => {
    if (!currentQuestion) {
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
      setLessonFeedbackType(isCorrect ? 'success' : 'error');
      setLessonFeedback(
        isCorrect
          ? `✅ Đúng rồi! +${response.awardedGems} 💎, +${response.awardedEnergy}% năng lượng. ${response.explanation}`
          : `❌ Chưa đúng. ${response.explanation}`
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
      setAnsweredQuestions((prev) => [...prev, currentQuestion.cauHoiId]);
      
      // Tự động chuyển câu hỏi sau 2 giây
      setTimeout(() => {
        setLessonQuestionIndex((index) => index + 1);
        setLessonFeedback(null);
        setLessonFeedbackType(null);
      }, 2000);
    } catch (error) {
      console.error(error);
      setLessonFeedbackType('error');
      setLessonFeedback('Không thể chấm câu trả lời.');
    } finally {
      setIsProcessingLesson(false);
    }
  };

  const handleAskChatbot = async (question: string) => {
    setIsAskingBot(true);
    try {
      const response = await GameplayApi.askChatbot({ question, hocSinhId });
      setChatbotLog((prev) => [response, ...prev].slice(0, 5));
      return response;
    } finally {
      setIsAskingBot(false);
    }
  };

  const renderZonePanel = () => {
    switch (activeZone) {
      case 'school':
        return (
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
                {lessonLoading && <p>Đang tải câu hỏi...</p>}
                {!lessonLoading && currentQuestion && (
                  <>
                    <p className="question">{currentQuestion.noiDung}</p>
                    <div className="options-grid">
                      {[
                        { key: 'A', text: currentQuestion.phuongAnA },
                        { key: 'B', text: currentQuestion.phuongAnB },
                        { key: 'C', text: currentQuestion.phuongAnC },
                        { key: 'D', text: currentQuestion.phuongAnD }
                      ].map((option) => (
                        <button
                          key={option.key}
                          className="option-card"
                          onClick={() => handleSubmitAnswer(option.key as AnswerOption)}
                          disabled={isProcessingLesson}
                        >
                          <strong>{option.key}.</strong> {option.text}
                        </button>
                      ))}
                    </div>
                  </>
                )}
                {!lessonLoading && !currentQuestion && lessonDetail && (
                  <div className="lesson-finished">
                    <p>Bạn đã trả lời hết câu hỏi cho bài này.</p>
                    <button
                      className="primary"
                      onClick={() => handleCompleteLesson(lessonDetail.baiHocId)}
                      disabled={isProcessingLesson}
                    >
                      Đánh dấu hoàn thành & nạp năng lượng
                    </button>
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
        );
      case 'arcade':
        return (
          <ArcadePanel
            currentEnergy={status?.nangLuongGioChoi ?? 0}
            onPlay={handlePlayArcade}
            isPlaying={isPlayingArcade}
          />
        );
      case 'shop':
        return (
          <ShopPanel
            rewards={rewards}
            owned={status?.inventory ?? []}
            onPurchase={handlePurchase}
            isPurchasing={isPurchasing}
          />
        );
      case 'leaderboard':
        return (
          <LeaderboardPanel
            entries={leaderboard}
            isLoading={isLoadingLeaderboard}
            onRefresh={refreshLeaderboard}
          />
        );
      case 'chatbot':
        return (
          <div className="zone-content two-column">
            <ChatbotPanel hocSinhId={hocSinhId} onAsk={handleAskChatbot} />
            <div className="panel chatbot-log">
              <h3>Lịch sử trả lời</h3>
              {chatbotLog.length === 0 ? (
                <p className="muted">Chưa có câu hỏi nào.</p>
              ) : (
                <ul>
                  {chatbotLog.map((entry, index) => (
                    <li key={index}>
                      <p>{entry.answer}</p>
                      <span className="muted small">Nguồn: {entry.source}</span>
                    </li>
                  ))}
                </ul>
              )}
            </div>
          </div>
        );
      default:
        return <p className="muted">Hãy chọn một khu vực trên bản đồ để bắt đầu hành trình.</p>;
    }
  };

  return (
    <div className="city-experience">
      <div className="city-hud">
        <StatusCard status={status} onRefresh={refreshStatus} isLoading={isLoadingStatus} />
        {feedback && <div className="panel info-panel">{feedback}</div>}
      </div>

      <div className="city-map">
        <button className="map-node school" onClick={() => setActiveZone('school')}>
          Trường học
        </button>
        <button className="map-node arcade" onClick={() => setActiveZone('arcade')}>
          Sân chơi Arcade
        </button>
        <button className="map-node shop" onClick={() => setActiveZone('shop')}>
          Cửa hàng
        </button>
        <button className="map-node leaderboard" onClick={() => setActiveZone('leaderboard')}>
          Tượng đài vinh danh
        </button>
        <button className="map-node chatbot" onClick={() => setActiveZone('chatbot')}>
          Chatbot AI
        </button>
      </div>

      <div className="zone-panel">
        <header>
          <h2>
            {activeZone
              ? {
                  school: 'Khu Trường Học',
                  arcade: 'Sân chơi Arcade',
                  shop: 'Cửa hàng vật phẩm',
                  leaderboard: 'Bảng xếp hạng',
                  chatbot: 'Học cùng trợ lý AI'
                }[activeZone]
              : 'Chọn khu vực trên bản đồ'}
          </h2>
        </header>
        <div>{renderZonePanel()}</div>
      </div>
    </div>
  );
}

