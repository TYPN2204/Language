import type { LeaderboardEntryDto } from '../types/gameplay';

interface LeaderboardPanelProps {
  entries: LeaderboardEntryDto[];
  isLoading: boolean;
  onRefresh: () => void;
}

export function LeaderboardPanel({ entries, isLoading, onRefresh }: LeaderboardPanelProps) {
  return (
    <div className="panel leaderboard-panel">
      <header>
        <div>
          <p className="eyebrow">Bang xếp hạng</p>
          <h2>Top Học Viên</h2>
        </div>
        <button className="mini ghost" onClick={onRefresh} disabled={isLoading}>
          Làm mới
        </button>
      </header>

      {entries.length === 0 ? (
        <p className="muted">Chưa có dữ liệu tháng này. Hãy hoàn thành bài học để lên bảng vàng!</p>
      ) : (
        <ol className="leaderboard-list">
          {entries.map((entry) => (
            <li key={entry.hocSinhId}>
              <span className="rank">#{entry.rank}</span>
              <div className="leader-info">
                <strong>{entry.tenDangNhap}</strong>
                <span className="muted small">{entry.tongDiemThang} pts tháng này</span>
              </div>
              <span className="badge">{entry.tongDiem} 💎</span>
            </li>
          ))}
        </ol>
      )}
    </div>
  );
}

