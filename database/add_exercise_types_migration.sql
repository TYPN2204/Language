-- =================================================================
-- MIGRATION: THÊM HỖ TRỢ ĐA DẠNG LOẠI BÀI TẬP
-- Mục đích: Tái cấu trúc bảng CauHoiTracNghiem để hỗ trợ nhiều loại câu hỏi
--           và thêm cột SoLanHoanThanh vào TienDo để theo dõi mức độ thông thạo
-- =================================================================

-- Đảm bảo đang sử dụng đúng database
USE LanguageAppDb;
GO

-- BƯỚC 1: Thêm các cột mới vào bảng CauHoiTracNghiem
-- =================================================================

-- Thêm cột LoaiCauHoi để xác định loại bài tập
IF NOT EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[CauHoiTracNghiem]') 
    AND name = 'LoaiCauHoi'
)
BEGIN
    PRINT 'Bắt đầu thêm cột LoaiCauHoi...';
    
    ALTER TABLE CauHoiTracNghiem
    ADD LoaiCauHoi NVARCHAR(50) NULL;
    
    PRINT 'Đã thêm cột LoaiCauHoi vào bảng CauHoiTracNghiem.';
END
ELSE
BEGIN
    PRINT 'Cột LoaiCauHoi đã tồn tại trong bảng CauHoiTracNghiem.';
END
GO

-- Đặt giá trị mặc định cho dữ liệu hiện có (tất cả là trắc nghiệm)
-- Chạy riêng sau khi cột đã được tạo
IF EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[CauHoiTracNghiem]') 
    AND name = 'LoaiCauHoi'
    AND is_nullable = 1
)
BEGIN
    PRINT 'Đang cập nhật giá trị mặc định cho LoaiCauHoi...';
    
    UPDATE CauHoiTracNghiem
    SET LoaiCauHoi = 'TRAC_NGHIEM'
    WHERE LoaiCauHoi IS NULL;
    
    -- Đặt NOT NULL sau khi cập nhật dữ liệu
    ALTER TABLE CauHoiTracNghiem
    ALTER COLUMN LoaiCauHoi NVARCHAR(50) NOT NULL;
    
    PRINT 'Đã cập nhật giá trị mặc định và đặt NOT NULL cho LoaiCauHoi.';
END
GO

-- Thêm cột AudioURL cho bài tập nghe
IF NOT EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[CauHoiTracNghiem]') 
    AND name = 'AudioURL'
)
BEGIN
    PRINT 'Bắt đầu thêm cột AudioURL...';
    
    ALTER TABLE CauHoiTracNghiem
    ADD AudioURL NVARCHAR(500) NULL;
    
    PRINT 'Đã thêm cột AudioURL vào bảng CauHoiTracNghiem.';
END
ELSE
BEGIN
    PRINT 'Cột AudioURL đã tồn tại trong bảng CauHoiTracNghiem.';
END
GO

-- Thêm các cột cho bài tập dịch câu
IF NOT EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[CauHoiTracNghiem]') 
    AND name = 'CauTienViet'
)
BEGIN
    PRINT 'Bắt đầu thêm cột CauTienViet...';
    
    ALTER TABLE CauHoiTracNghiem
    ADD CauTienViet NVARCHAR(MAX) NULL;
    
    PRINT 'Đã thêm cột CauTienViet vào bảng CauHoiTracNghiem.';
END
ELSE
BEGIN
    PRINT 'Cột CauTienViet đã tồn tại trong bảng CauHoiTracNghiem.';
END
GO

IF NOT EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[CauHoiTracNghiem]') 
    AND name = 'CauTienAnh'
)
BEGIN
    PRINT 'Bắt đầu thêm cột CauTienAnh...';
    
    ALTER TABLE CauHoiTracNghiem
    ADD CauTienAnh NVARCHAR(MAX) NULL;
    
    PRINT 'Đã thêm cột CauTienAnh vào bảng CauHoiTracNghiem.';
END
ELSE
BEGIN
    PRINT 'Cột CauTienAnh đã tồn tại trong bảng CauHoiTracNghiem.';
END
GO

-- Làm cho các cột phương án trở nên nullable (vì không phải tất cả loại câu hỏi đều cần)
-- Chỉ thực hiện nếu cột chưa nullable
IF EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[CauHoiTracNghiem]') 
    AND name = 'PhuongAnA'
    AND is_nullable = 0
)
BEGIN
    PRINT 'Đang làm cho PhuongAnA nullable...';
    ALTER TABLE CauHoiTracNghiem
    ALTER COLUMN PhuongAnA NVARCHAR(255) NULL;
    PRINT 'Đã cập nhật PhuongAnA thành nullable.';
END
GO

IF EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[CauHoiTracNghiem]') 
    AND name = 'PhuongAnB'
    AND is_nullable = 0
)
BEGIN
    PRINT 'Đang làm cho PhuongAnB nullable...';
    ALTER TABLE CauHoiTracNghiem
    ALTER COLUMN PhuongAnB NVARCHAR(255) NULL;
    PRINT 'Đã cập nhật PhuongAnB thành nullable.';
END
GO

IF EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[CauHoiTracNghiem]') 
    AND name = 'PhuongAnC'
    AND is_nullable = 0
)
BEGIN
    PRINT 'Đang làm cho PhuongAnC nullable...';
    ALTER TABLE CauHoiTracNghiem
    ALTER COLUMN PhuongAnC NVARCHAR(255) NULL;
    PRINT 'Đã cập nhật PhuongAnC thành nullable.';
END
GO

IF EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[CauHoiTracNghiem]') 
    AND name = 'PhuongAnD'
    AND is_nullable = 0
)
BEGIN
    PRINT 'Đang làm cho PhuongAnD nullable...';
    ALTER TABLE CauHoiTracNghiem
    ALTER COLUMN PhuongAnD NVARCHAR(255) NULL;
    PRINT 'Đã cập nhật PhuongAnD thành nullable.';
END
GO

-- Làm cho DapAnDung nullable
IF EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[CauHoiTracNghiem]') 
    AND name = 'DapAnDung'
    AND is_nullable = 0
)
BEGIN
    PRINT 'Đang làm cho DapAnDung nullable...';
    ALTER TABLE CauHoiTracNghiem
    ALTER COLUMN DapAnDung CHAR(1) NULL;
    PRINT 'Đã cập nhật DapAnDung thành nullable.';
END
GO

-- BƯỚC 2: Thêm cột SoLanHoanThanh vào bảng TienDo
-- =================================================================

IF NOT EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[TienDo]') 
    AND name = 'SoLanHoanThanh'
)
BEGIN
    PRINT 'Bắt đầu thêm cột SoLanHoanThanh...';
    
    ALTER TABLE TienDo
    ADD SoLanHoanThanh INT DEFAULT 0 NOT NULL;
    
    PRINT 'Đã thêm cột SoLanHoanThanh vào bảng TienDo.';
END
ELSE
BEGIN
    PRINT 'Cột SoLanHoanThanh đã tồn tại trong bảng TienDo.';
END
GO

-- Cập nhật dữ liệu hiện có: nếu đã có NgayHoanThanh thì coi như đã hoàn thành 1 lần
-- Chạy riêng sau khi cột đã được tạo
IF EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[TienDo]') 
    AND name = 'SoLanHoanThanh'
)
BEGIN
    PRINT 'Đang cập nhật giá trị mặc định cho SoLanHoanThanh...';
    
    UPDATE TienDo
    SET SoLanHoanThanh = 1
    WHERE NgayHoanThanh IS NOT NULL AND SoLanHoanThanh = 0;
    
    PRINT 'Đã cập nhật giá trị mặc định cho SoLanHoanThanh.';
END
GO

PRINT 'Migration hoàn tất!';
GO

-- =================================================================
-- KẾT THÚC MIGRATION
-- =================================================================
-- Sau khi chạy migration này:
-- 1. Tất cả câu hỏi hiện có sẽ có LoaiCauHoi = 'TRAC_NGHIEM'
-- 2. Có thể thêm các loại câu hỏi mới: 'DIEN_VAO_CHO_TRONG', 'DICH_CAU', 'SAP_XEP_TU', 'CHON_CAP'
-- 3. TienDo.SoLanHoanThanh sẽ theo dõi số lần hoàn thành bài học (cần 2 lần để thông thạo)
-- =================================================================

