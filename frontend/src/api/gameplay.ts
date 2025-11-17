import { httpClient } from './httpClient';
import type {
  ArcadePlayRequest,
  CourseDto,
  PurchaseRewardRequest,
  RewardDto,
  CompleteLessonRequest,
  StudentStatusResponse,
  LeaderboardEntryDto,
  LessonDetailResponse,
  SubmitAnswerRequest,
  SubmitAnswerResponse,
  ChatbotRequest,
  ChatbotResponse
} from '../types/gameplay';

export const GameplayApi = {
  async getStatus(hocSinhId: number): Promise<StudentStatusResponse> {
    const { data } = await httpClient.get<StudentStatusResponse>(`/students/${hocSinhId}/status`);
    return data;
  },

  async getCourses(): Promise<CourseDto[]> {
    const { data } = await httpClient.get<CourseDto[]>('/courses');
    return data;
  },

  async getRewards(): Promise<RewardDto[]> {
    const { data } = await httpClient.get<RewardDto[]>('/shop/rewards');
    return data;
  },

  async completeLesson(payload: CompleteLessonRequest): Promise<StudentStatusResponse> {
    const { data } = await httpClient.post<StudentStatusResponse>('/lessons/complete', payload);
    return data;
  },

  async playArcade(payload: ArcadePlayRequest): Promise<StudentStatusResponse> {
    const { data } = await httpClient.post<StudentStatusResponse>('/arcade/play', payload);
    return data;
  },

  async purchaseReward(payload: PurchaseRewardRequest): Promise<StudentStatusResponse> {
    const { data } = await httpClient.post<StudentStatusResponse>('/shop/purchase', payload);
    return data;
  },

  async getLeaderboard(): Promise<LeaderboardEntryDto[]> {
    const { data } = await httpClient.get<LeaderboardEntryDto[]>('/leaderboard/current');
    return data;
  },

  async getLessonDetail(lessonId: number): Promise<LessonDetailResponse> {
    const { data } = await httpClient.get<LessonDetailResponse>(`/lessons/${lessonId}/detail`);
    return data;
  },

  async submitAnswer(payload: SubmitAnswerRequest): Promise<SubmitAnswerResponse> {
    const { data } = await httpClient.post<SubmitAnswerResponse>('/lessons/quiz', payload);
    return data;
  },

  async askChatbot(payload: ChatbotRequest): Promise<ChatbotResponse> {
    const { data } = await httpClient.post<ChatbotResponse>('/chatbot/ask', payload);
    return data;
  }
};

