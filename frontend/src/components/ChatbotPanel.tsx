import { FormEvent, useState } from 'react';
import type { ChatbotResponse } from '../types/gameplay';

interface ChatbotPanelProps {
  hocSinhId: number;
  onAsk: (question: string) => Promise<ChatbotResponse>;
}

export function ChatbotPanel({ hocSinhId, onAsk }: ChatbotPanelProps) {
  const [question, setQuestion] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [answer, setAnswer] = useState<ChatbotResponse | null>(null);

  const handleSubmit = async (event: FormEvent) => {
    event.preventDefault();
    if (!question.trim()) {
      return;
    }

    setIsLoading(true);
    try {
      const response = await onAsk(question.trim());
      setAnswer(response);
    } catch (error) {
      console.error(error);
      setAnswer({
        answer: 'Chatbot đang bận. Vui lòng thử lại sau.',
        source: 'local'
      });
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="panel chatbot-panel">
      <header>
        <div>
          <p className="eyebrow">Chatbot AI</p>
          <h2>Trợ lý Học thuật</h2>
          <p className="muted small">
            Bot sử dụng chính nội dung bài học để trả lời câu hỏi của học sinh #{hocSinhId}.
          </p>
        </div>
      </header>

      <form className="chatbot-form" onSubmit={handleSubmit}>
        <textarea
          value={question}
          onChange={(event) => setQuestion(event.target.value)}
          placeholder="Hỏi về ngữ pháp, hội thoại hoặc mẹo ôn tập..."
          rows={4}
          required
        />
        <button type="submit" className="primary" disabled={isLoading}>
          {isLoading ? 'Đang suy nghĩ...' : 'Hỏi bot'}
        </button>
      </form>

      {answer && (
        <div className="chatbot-answer">
          <p>{answer.answer}</p>
          <p className="muted small">
            Nguồn: {answer.source === 'n8n' ? 'Workflow n8n' : 'Kiến thức nội bộ'}
            {answer.lessonReference && ` • Tham khảo: ${answer.lessonReference}`}
          </p>
        </div>
      )}
    </div>
  );
}

