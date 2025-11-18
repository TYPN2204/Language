import type { StudentStatusResponse } from '../types/gameplay';

interface StatusCardProps {
  status: StudentStatusResponse | null;
  onRefresh: () => void;
  isLoading: boolean;
}

export function StatusCard({ status, onRefresh, isLoading }: StatusCardProps) {
  if (!status) {
    return (
      <div className="panel status-panel">
        <p>Đang tải trạng thái học sinh...</p>
      </div>
    );
  }

  return (
    <div className="panel status-panel">
      <header>
        <div>
          <p className="eyebrow">Học sinh</p>
          <h2>{status.tenDangNhap}</h2>
        </div>
        <button className="mini ghost" onClick={onRefresh} disabled={isLoading}>
          Làm mới
        </button>
      </header>

      <div className="status-grid">
        <div>
          <p className="eyebrow">Đá Quý</p>
          <p className="stat-value">{status.tongDiem} 💎</p>
        </div>
        <div>
          <p className="eyebrow">Vé chơi game</p>
          <p className="stat-value">{status.soVeChoiGame} 🎫</p>
        </div>
        <div>
          <p className="eyebrow">Năng lượng</p>
          <p className="stat-value">{status.nangLuongGioChoi}%</p>
        </div>
        <div>
          <p className="eyebrow">Bài học đã hoàn thành</p>
          <p className="stat-value">{status.completedLessons}</p>
        </div>
      </div>

      <div>
        <p className="eyebrow">Kho đồ</p>
        {status.inventory.length === 0 ? (
          <p className="muted">Chưa có vật phẩm nào. Hãy ghé cửa hàng!</p>
        ) : (
          <ul className="inventory-list">
            {status.inventory.map((item) => (
              <li key={item.phanThuongId}>
                <div>
                  <strong>{item.tenPhanThuong}</strong>
                  <span>{item.loaiPhanThuong}</span>
                </div>
                <span className="badge">x{item.soLanSoHuu}</span>
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  );
}

