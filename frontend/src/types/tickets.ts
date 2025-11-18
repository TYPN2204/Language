export interface BuyTicketRequest {
  hocSinhId: number;
  quantity: number;
}

export interface UseTicketRequest {
  hocSinhId: number;
}

export interface TicketResponse {
  soVeChoiGame: number;
  message?: string | null;
}

