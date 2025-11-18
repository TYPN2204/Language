-- =================================================================
-- MIGRATION: Thêm cột SoVeChoiGame vào bảng HocSinh
-- Ngày tạo: Gameplay Overhaul 2.0
-- =================================================================

-- Đảm bảo đang sử dụng đúng database
USE LanguageAppDb;
GO

-- Bước 1: Thêm cột SoVeChoiGame vào bảng HocSinh (nếu chưa có)
IF NOT EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[HocSinh]') 
    AND name = 'SoVeChoiGame'
)
BEGIN
    ALTER TABLE HocSinh
    ADD SoVeChoiGame INT DEFAULT 0;
    
    PRINT 'Đã thêm cột SoVeChoiGame vào bảng HocSinh.';
END
ELSE
BEGIN
    PRINT 'Cột SoVeChoiGame đã tồn tại trong bảng HocSinh.';
END
GO

-- Bước 2: Cập nhật giá trị mặc định cho các bản ghi hiện có
-- (Chạy riêng sau khi cột đã được tạo để tránh lỗi parse)
IF EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[HocSinh]') 
    AND name = 'SoVeChoiGame'
)
BEGIN
    UPDATE HocSinh
    SET SoVeChoiGame = 0
    WHERE SoVeChoiGame IS NULL;
    
    PRINT 'Đã cập nhật giá trị mặc định cho các bản ghi hiện có.';
END
GO

PRINT 'Migration hoàn tất!';
GO

