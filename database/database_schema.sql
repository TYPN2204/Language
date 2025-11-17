-- =================================================================
-- BẢN VẼ DATABASE CHO GAME RPG HỌC TẬP "THỊ TRẤN HỌC THUẬT"
-- =================================================================

-- MỤC 1: QUẢN LÝ NGƯỜI DÙNG (USER MANAGEMENT)
-- =================================================================

CREATE TABLE HocSinh (
    HocSinhID INT PRIMARY KEY IDENTITY(1,1),
    TenDangNhap NVARCHAR(50) NOT NULL UNIQUE,
    MatKhauHash NVARCHAR(255) NOT NULL,
    Email NVARCHAR(100) UNIQUE,
    NgayTao DATETIME DEFAULT GETDATE(),
    TongDiem INT DEFAULT 0, -- Chính là tiền tệ "Đá Quý" 💎
    NangLuongGioChoi INT DEFAULT 0 -- Thanh năng lượng từ 0-100%
);

CREATE TABLE PhuHuynh (
    PhuHuynhID INT PRIMARY KEY IDENTITY(1,1),
    TenPhuHuynh NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE,
    SoDienThoai NVARCHAR(20),
    ZaloID NVARCHAR(50) UNIQUE -- ID Zalo để nhận báo cáo
);

CREATE TABLE HocSinh_PhuHuynh (
    HocSinhID INT FOREIGN KEY REFERENCES HocSinh(HocSinhID),
    PhuHuynhID INT FOREIGN KEY REFERENCES PhuHuynh(PhuHuynhID),
    PRIMARY KEY (HocSinhID, PhuHuynhID)
);



-- MỤC 2: QUẢN LÝ NỘI DUNG HỌC TẬP (LEARNING CONTENT)
-- =================================================================

CREATE TABLE KhoaHoc (
    KhoaHocID INT PRIMARY KEY IDENTITY(1,1),
    TenKhoaHoc NVARCHAR(100) NOT NULL,
    MoTa NVARCHAR(500),
    DoKho INT -- Ví dụ: 1-Dễ, 2-Trung bình, 3-Khó
);

CREATE TABLE BaiHoc (
    BaiHocID INT PRIMARY KEY IDENTITY(1,1),
    KhoaHocID INT FOREIGN KEY REFERENCES KhoaHoc(KhoaHocID),
    TenBaiHoc NVARCHAR(150) NOT NULL,
    ThuTu INT
);

CREATE TABLE CauHoiTracNghiem (
    CauHoiID INT PRIMARY KEY IDENTITY(1,1),
    BaiHocID INT FOREIGN KEY REFERENCES BaiHoc(BaiHocID),
    NoiDung NVARCHAR(MAX) NOT NULL, -- Nội dung câu hỏi
    PhuongAnA NVARCHAR(255) NOT NULL,
    PhuongAnB NVARCHAR(255) NOT NULL,
    PhuongAnC NVARCHAR(255) NOT NULL,
    PhuongAnD NVARCHAR(255) NOT NULL,
    DapAnDung CHAR(1) NOT NULL -- 'A', 'B', 'C', or 'D'
);



-- MỤC 3: QUẢN LÝ TIẾN ĐỘ & LỊCH SỬ (PROGRESS & HISTORY)
-- =================================================================

CREATE TABLE TienDo (
    TienDoID INT PRIMARY KEY IDENTITY(1,1),
    HocSinhID INT FOREIGN KEY REFERENCES HocSinh(HocSinhID),
    BaiHocID INT FOREIGN KEY REFERENCES BaiHoc(BaiHocID),
    NgayHoanThanh DATETIME,
    DiemSo INT -- Điểm cho bài học cụ thể này
);

-- MỤC 5: HỆ THỐNG VẬT PHẨM & CỬA HÀNG (ITEM & SHOP SYSTEM)
-- =================================================================

CREATE TABLE PhanThuong (
    PhanThuongID INT PRIMARY KEY IDENTITY(1,1),
    TenPhanThuong NVARCHAR(100) NOT NULL,
    LoaiPhanThuong NVARCHAR(50) NOT NULL, -- 'Cosmetic' (Trang trí), 'Utility' (Hỗ trợ học tập)
    MoTa NVARCHAR(500),
    Gia INT NOT NULL, -- Giá bán bằng "Đá Quý" 💎
    AssetURL NVARCHAR(255) -- URL đến hình ảnh của vật phẩm
);

CREATE TABLE HocSinh_PhanThuong (
    HocSinhPhanThuongID INT PRIMARY KEY IDENTITY(1,1),
    HocSinhID INT FOREIGN KEY REFERENCES HocSinh(HocSinhID),
    PhanThuongID INT FOREIGN KEY REFERENCES PhanThuong(PhanThuongID),
    NgayNhan DATETIME DEFAULT GETDATE()
);



-- MỤC 6: BÁO CÁO & XẾP HẠNG (REPORTING & LEADERBOARD)
-- =================================================================

CREATE TABLE BangXepHang (
    BangXepHangID INT PRIMARY KEY IDENTITY(1,1),
    HocSinhID INT FOREIGN KEY REFERENCES HocSinh(HocSinhID),
    Thang INT,
    Nam INT,
    ThuHang INT,
    TongDiemThang INT
);

CREATE TABLE BaoCaoZalo (
    BaoCaoID INT PRIMARY KEY IDENTITY(1,1),
    PhuHuynhID INT FOREIGN KEY REFERENCES PhuHuynh(PhuHuynhID),
    NoiDungBaoCao NVARCHAR(MAX),
    NgayGui DATETIME,
    TrangThai NVARCHAR(50) -- 'Thành công', 'Thất bại'
);

