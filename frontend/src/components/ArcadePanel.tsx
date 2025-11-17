interface ArcadePanelProps {
  currentEnergy: number;
  onPlay: (energy: number) => void;
  isPlaying: boolean;
}

export function ArcadePanel({ currentEnergy, onPlay, isPlaying }: ArcadePanelProps) {
  const spendableEnergy = Math.max(0, currentEnergy - (currentEnergy % 5));

  const handlePlay = () => {
    if (spendableEnergy >= 5) {
      onPlay(Math.min(25, spendableEnergy));
    }
  };

  return (
    <div className="panel arcade-panel">
      <header>
        <div>
          <p className="eyebrow">Sân chơi Arcade</p>
          <h2>Đổi năng lượng lấy Đá Quý</h2>
        </div>
      </header>

      <p className="muted">
        Mỗi lượt chơi tiêu hao năng lượng (bội số của 5) và trả về lượng 💎 ngẫu nhiên. Bạn đang có{' '}
        <strong>{currentEnergy}%</strong> năng lượng.
      </p>

      <button className="primary" onClick={handlePlay} disabled={isPlaying || spendableEnergy < 5}>
        {isPlaying ? 'Đang quay...' : `Chơi với ${Math.min(25, spendableEnergy)}% năng lượng`}
      </button>

      {spendableEnergy < 5 && <p className="muted">Hoàn thành thêm bài học để nạp năng lượng nhé!</p>}
    </div>
  );
}

