import type { OwnedRewardDto, RewardDto } from '../types/gameplay';

interface ShopPanelProps {
  rewards: RewardDto[];
  owned: OwnedRewardDto[];
  onPurchase: (rewardId: number) => void;
  isPurchasing: boolean;
}

export function ShopPanel({ rewards, owned, onPurchase, isPurchasing }: ShopPanelProps) {
  const ownedMap = new Map(owned.map((item) => [item.phanThuongId, item.soLanSoHuu]));

  return (
    <div className="panel shop-panel">
      <header>
        <div>
          <p className="eyebrow">Cửa hàng</p>
          <h2>Trang trí & Hỗ trợ học tập</h2>
        </div>
      </header>

      <ul className="shop-grid">
        {rewards.map((reward) => (
          <li key={reward.phanThuongId}>
            <div>
              <h3>{reward.tenPhanThuong}</h3>
              <p className="muted">{reward.loaiPhanThuong}</p>
              {reward.moTa && <p className="muted small">{reward.moTa}</p>}
            </div>
            <div className="shop-footer">
              <span className="badge">{reward.gia} 💎</span>
              {ownedMap.has(reward.phanThuongId) && (
                <span className="muted small">Đang sở hữu x{ownedMap.get(reward.phanThuongId)}</span>
              )}
              <button
                className="mini"
                onClick={() => onPurchase(reward.phanThuongId)}
                disabled={isPurchasing}
              >
                Mua
              </button>
            </div>
          </li>
        ))}
      </ul>
    </div>
  );
}

