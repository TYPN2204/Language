import { useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { GameplayApi } from '../api/gameplay';
import type { AuthResponse } from '../types/auth';
import type { ChatbotResponse } from '../types/gameplay';
import { ChatbotPanel } from '../components/ChatbotPanel';

interface ChatbotProps {
  auth: AuthResponse;
}

export function Chatbot({ auth }: ChatbotProps) {
  const navigate = useNavigate();
  const [chatbotLog, setChatbotLog] = useState<ChatbotResponse[]>([]);
  const [isAskingBot, setIsAskingBot] = useState(false);

  const hocSinhId = auth.hocSinhId;

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

  return (
    <div className="page-container">
      <header className="page-header">
        <button className="back-button" onClick={() => navigate('/town')}>
          ← Về thị trấn
        </button>
        <h1>🤖 Chatbot AI</h1>
      </header>

      <div className="page-content">
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
      </div>
    </div>
  );
}

