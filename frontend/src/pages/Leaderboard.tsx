import { useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { GameplayApi } from '../api/gameplay';
import type { AuthResponse } from '../types/auth';
import type { LeaderboardEntryDto } from '../types/gameplay';
import { LeaderboardPanel } from '../components/LeaderboardPanel';

interface LeaderboardProps {
  auth: AuthResponse;
}

export function Leaderboard({ auth }: LeaderboardProps) {
  const navigate = useNavigate();
  const [leaderboard, setLeaderboard] = useState<LeaderboardEntryDto[]>([]);
  const [isLoadingLeaderboard, setIsLoadingLeaderboard] = useState(false);

  useEffect(() => {
    refreshLeaderboard();
  }, []);

  const refreshLeaderboard = async () => {
    setIsLoadingLeaderboard(true);
    try {
      const data = await GameplayApi.getLeaderboard();
      setLeaderboard(data);
    } catch (error) {
      console.error(error);
    } finally {
      setIsLoadingLeaderboard(false);
    }
  };

  return (
    <div className="page-container">
      <header className="page-header">
        <button className="back-button" onClick={() => navigate('/town')}>
          ← Về thị trấn
        </button>
        <h1>🏆 Bảng xếp hạng</h1>
      </header>

      <div className="page-content">
        <div className="zone-content">
          <LeaderboardPanel
            entries={leaderboard}
            isLoading={isLoadingLeaderboard}
            onRefresh={refreshLeaderboard}
          />
        </div>
      </div>
    </div>
  );
}

