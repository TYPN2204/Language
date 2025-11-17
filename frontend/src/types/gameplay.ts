export interface LessonDto {
  baiHocId: number;
  khoaHocId: number;
  tenBaiHoc: string;
  thuTu?: number | null;
}

export interface CourseDto {
  khoaHocId: number;
  tenKhoaHoc: string;
  moTa?: string | null;
  doKho?: number | null;
  lessons: LessonDto[];
}

export interface RewardDto {
  phanThuongId: number;
  tenPhanThuong: string;
  loaiPhanThuong: string;
  moTa?: string | null;
  gia: number;
  assetUrl?: string | null;
}

export interface OwnedRewardDto extends RewardDto {
  soLanSoHuu: number;
}

export interface StudentStatusResponse {
  hocSinhId: number;
  tenDangNhap: string;
  email?: string | null;
  tongDiem: number;
  nangLuongGioChoi: number;
  completedLessons: number;
  inventory: OwnedRewardDto[];
  message?: string | null;
}

export interface CompleteLessonRequest {
  hocSinhId: number;
  baiHocId: number;
  diemSo: number;
}

export interface ArcadePlayRequest {
  hocSinhId: number;
  energySpent: number;
}

export interface PurchaseRewardRequest {
  hocSinhId: number;
  phanThuongId: number;
}

export interface LeaderboardEntryDto {
  hocSinhId: number;
  tenDangNhap: string;
  rank: number;
  tongDiemThang: number;
  tongDiem: number;
}

export interface QuizQuestionDto {
  cauHoiId: number;
  noiDung: string;
  phuongAnA: string;
  phuongAnB: string;
  phuongAnC: string;
  phuongAnD: string;
}

export interface LessonDetailResponse {
  baiHocId: number;
  tenBaiHoc: string;
  courseName?: string;
  questionCount: number;
  questions: QuizQuestionDto[];
}

export interface SubmitAnswerRequest {
  hocSinhId: number;
  cauHoiId: number;
  traLoi: string;
}

export interface SubmitAnswerResponse {
  correct: boolean;
  explanation: string;
  awardedGems: number;
  awardedEnergy: number;
  totalGems: number;
  totalEnergy: number;
}

export interface ChatbotRequest {
  question: string;
  hocSinhId?: number;
}

export interface ChatbotResponse {
  answer: string;
  source: string;
  lessonReference?: string | null;
}

export interface MatchingCardDto {
  id: number;
  text: string;
  imageUrl: string;
  pairId: number;
}

export interface MatchingGameDataResponse {
  cards: MatchingCardDto[];
}

export interface MatchingGameWinRequest {
  hocSinhId: number;
  energySpent: number;
  timeTaken: number;
  pairsMatched: number;
}

