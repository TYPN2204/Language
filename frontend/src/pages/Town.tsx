import { useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { GameplayApi } from '../api/gameplay';
import type { AuthResponse } from '../types/auth';
import type { StudentStatusResponse } from '../types/gameplay';
import { StatusCard } from '../components/StatusCard';
import './Town.css';

interface TownProps {
  auth: AuthResponse;
}

export function Town({ auth }: TownProps) {
  const navigate = useNavigate();
  const [status, setStatus] = useState<StudentStatusResponse | null>(null);
  const [isLoadingStatus, setIsLoadingStatus] = useState(false);

  const hocSinhId = auth.hocSinhId;

  useEffect(() => {
    const loadStatus = async () => {
      setIsLoadingStatus(true);
      try {
        const statusResponse = await GameplayApi.getStatus(hocSinhId);
        setStatus(statusResponse);
      } catch (error) {
        console.error(error);
      } finally {
        setIsLoadingStatus(false);
      }
    };

    loadStatus();
  }, [hocSinhId]);

  const refreshStatus = async () => {
    setIsLoadingStatus(true);
    try {
      const latest = await GameplayApi.getStatus(hocSinhId);
      setStatus(latest);
    } catch (error) {
      console.error(error);
    } finally {
      setIsLoadingStatus(false);
    }
  };

  const handleBuildingClick = (route: string) => {
    navigate(route);
  };

  return (
    <div className="town-container">
      {/* Ảnh nền toàn màn hình */}
      <div className="town-background">
        <img 
          src="/town_background.jpg" 
          alt="Town Background" 
          className="town-bg-image"
          onError={(e) => {
            // Fallback sang SVG placeholder nếu ảnh JPG không tồn tại
            const target = e.target as HTMLImageElement;
            if (!target.src.includes('.svg')) {
              target.src = '/town_background.svg';
            } else {
              // Nếu SVG cũng lỗi, dùng CSS fallback
              target.style.display = 'none';
              target.parentElement?.classList.add('fallback-bg');
            }
          }}
        />
        
        {/* Các nút bấm trong suốt tương ứng với vị trí các tòa nhà */}
        <button
          className="building-button school-building"
          onClick={() => handleBuildingClick('/school')}
          title="Trường học"
        >
          <span className="building-icon">🏫</span>
          <span className="building-label">Trường học</span>
        </button>

        <button
          className="building-button arcade-building"
          onClick={() => handleBuildingClick('/arcade')}
          title="Sân chơi Arcade"
        >
          <span className="building-icon">🎮</span>
          <span className="building-label">Arcade</span>
        </button>

        <button
          className="building-button shop-building"
          onClick={() => handleBuildingClick('/shop')}
          title="Cửa hàng"
        >
          <span className="building-icon">🛒</span>
          <span className="building-label">Cửa hàng</span>
        </button>

        <button
          className="building-button leaderboard-building"
          onClick={() => handleBuildingClick('/leaderboard')}
          title="Tượng đài vinh danh"
        >
          <span className="building-icon">🏆</span>
          <span className="building-label">Bảng xếp hạng</span>
        </button>

        <button
          className="building-button chatbot-building"
          onClick={() => handleBuildingClick('/chatbot')}
          title="Chatbot AI"
        >
          <span className="building-icon">🤖</span>
          <span className="building-label">Chatbot</span>
        </button>
      </div>

      {/* HUD hiển thị thông tin ở góc trên */}
      <div className="town-hud">
        <StatusCard status={status} onRefresh={refreshStatus} isLoading={isLoadingStatus} />
      </div>
    </div>
  );
}

