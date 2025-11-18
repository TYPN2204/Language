# 📘 HƯỚNG DẪN CHI TIẾT CHẠY SQL MIGRATION

## 🎯 Mục đích
Thêm cột `SoVeChoiGame` vào bảng `HocSinh` trong database để lưu trữ số vé chơi game của học sinh.

---

## 📋 CÁCH 1: SỬ DỤNG SQL SERVER MANAGEMENT STUDIO (SSMS)

### Bước 1: Mở SQL Server Management Studio

1. **Tìm kiếm SSMS** trong Windows:
   - Nhấn `Windows + S`
   - Gõ "SQL Server Management Studio"
   - Click vào ứng dụng

2. **Hoặc mở từ Start Menu:**
   - Tìm trong danh sách ứng dụng Microsoft SQL Server Tools

### Bước 2: Kết nối đến Database Server

1. **Màn hình "Connect to Server" sẽ hiện ra:**
   ```
   Server type: Database Engine
   Server name: localhost hoặc . hoặc tên server của bạn
   Authentication: Windows Authentication (hoặc SQL Server Authentication)
   ```

2. **Nhập thông tin:**
   - **Server name:** 
     - Nếu SQL Server chạy trên máy local: `localhost` hoặc `.` hoặc `(local)`
     - Nếu có tên instance: `localhost\SQLEXPRESS` hoặc `localhost\MSSQLSERVER`
     - Nếu remote: nhập IP hoặc tên server
   
   - **Authentication:**
     - **Windows Authentication:** (Khuyến nghị - dùng tài khoản Windows)
     - **SQL Server Authentication:** (Nếu có username/password riêng)

3. **Click "Connect"**

### Bước 3: Tìm Database của Project

1. **Trong Object Explorer (bên trái), mở rộng:**
   ```
   Databases
     └── [Tên database của bạn]
   ```
   
   **Theo file `appsettings.json` của project, database name là:**
   - `LanguageAppDb` ✅

2. **Nếu không thấy database:**
   - Kiểm tra file `backend/appsettings.json`
   - Tìm connection string, ví dụ:
     ```json
     "ConnectionStrings": {
       "LanguageAppDb": "Server=localhost;Database=LanguageAppDb;..."
     }
     ```
   - Database name là phần sau `Database=`

### Bước 4: Mở File SQL Script

1. **Mở file migration:**
   - Đường dẫn: `database/add_ticket_column_migration.sql`
   - Mở bằng Notepad, VS Code, hoặc bất kỳ text editor nào

2. **Copy toàn bộ nội dung** (Ctrl + A, Ctrl + C)

### Bước 5: Tạo Query Mới và Paste Script

1. **Trong SSMS, click chuột phải vào database của bạn:**
   ```
   Databases
     └── [Tên database]
         └── (Click chuột phải) → New Query
   ```

2. **Hoặc click nút "New Query" trên thanh toolbar**

3. **Paste script vào cửa sổ query** (Ctrl + V)

4. **Kiểm tra script đã được paste đúng:**
   ```sql
   -- =================================================================
   -- MIGRATION: Thêm cột SoVeChoiGame vào bảng HocSinh
   -- ...
   
   IF NOT EXISTS (
       SELECT 1 
       FROM sys.columns 
       WHERE object_id = OBJECT_ID(N'[dbo].[HocSinh]') 
       AND name = 'SoVeChoiGame'
   )
   BEGIN
       ALTER TABLE HocSinh
       ADD SoVeChoiGame INT DEFAULT 0;
       ...
   ```

### Bước 6: Chọn Database Đúng

**Quan trọng:** Đảm bảo bạn đang chạy script trên đúng database!

1. **Kiểm tra dropdown ở trên cùng cửa sổ query:**
   ```
   [Dropdown]  [Execute] [Parse]
   ```
   
2. **Chọn database của project** từ dropdown (ví dụ: `LanguageAppDb`)

3. **Hoặc thêm dòng này vào đầu script:**
   ```sql
   USE LanguageAppDb;
   GO
   
   -- =================================================================
   -- MIGRATION: Thêm cột SoVeChoiGame vào bảng HocSinh
   -- ...
   ```

### Bước 7: Chạy Script

1. **Click nút "Execute"** (hoặc nhấn F5)

2. **Xem kết quả ở tab "Messages" (phía dưới):**
   ```
   Messages
   ---------
   Đã thêm cột SoVeChoiGame vào bảng HocSinh.
   Migration hoàn tất!
   ```

3. **Nếu thấy lỗi:**
   - Đọc message lỗi
   - Kiểm tra lại database name
   - Kiểm tra quyền truy cập

### Bước 8: Kiểm tra Migration Thành Công

**Chạy query kiểm tra:**
```sql
-- Kiểm tra cột đã được thêm chưa
SELECT 
    COLUMN_NAME, 
    DATA_TYPE, 
    IS_NULLABLE, 
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'HocSinh' 
  AND COLUMN_NAME = 'SoVeChoiGame';
```

**Kết quả mong đợi:**
```
COLUMN_NAME      DATA_TYPE    IS_NULLABLE    COLUMN_DEFAULT
SoVeChoiGame     int          YES            0
```

**Hoặc kiểm tra bằng cách:**
```sql
-- Xem cấu trúc bảng HocSinh
SELECT TOP 1 * FROM HocSinh;
```

Nếu thấy cột `SoVeChoiGame` trong kết quả → **Thành công! ✅**

---

## 📋 CÁCH 2: SỬ DỤNG COMMAND LINE (sqlcmd)

### Bước 1: Mở PowerShell hoặc Command Prompt

1. **Nhấn `Windows + X`**
2. **Chọn "Windows PowerShell" hoặc "Terminal"**

### Bước 2: Di chuyển đến thư mục project

```powershell
cd "C:\Users\ADMIN\OneDrive\Máy tính\LanguageApp"
```

### Bước 3: Chạy sqlcmd

**Cú pháp:**
```powershell
sqlcmd -S localhost -d [TênDatabase] -i database\add_ticket_column_migration.sql
```

**Ví dụ cụ thể (theo appsettings.json của project):**
```powershell
# Database: LanguageAppDb, Server: localhost
sqlcmd -S localhost -d LanguageAppDb -i database\add_ticket_column_migration.sql

# Nếu dùng SQL Server Express
sqlcmd -S localhost\SQLEXPRESS -d LanguageAppDb -i database\add_ticket_column_migration.sql

# Nếu dùng Windows Authentication (Trusted_Connection=True)
sqlcmd -S localhost -d LanguageAppDb -E -i database\add_ticket_column_migration.sql

# Nếu cần SQL Server Authentication
sqlcmd -S localhost -d LanguageAppDb -U sa -P [Password] -i database\add_ticket_column_migration.sql
```

**Giải thích tham số:**
- `-S localhost`: Server name
- `-d LanguageAppDb`: Database name
- `-i database\add_ticket_column_migration.sql`: Đường dẫn file script
- `-U sa`: Username (nếu cần)
- `-P [Password]`: Password (nếu cần)

### Bước 4: Xem kết quả

Nếu thành công, bạn sẽ thấy:
```
Đã thêm cột SoVeChoiGame vào bảng HocSinh.
Migration hoàn tất!
```

---

## 📋 CÁCH 3: SỬ DỤNG AZURE DATA STUDIO (Nếu có)

1. **Mở Azure Data Studio**
2. **Kết nối đến SQL Server** (tương tự SSMS)
3. **Mở file** `database/add_ticket_column_migration.sql`
4. **Chọn database** từ dropdown
5. **Click "Run"** hoặc nhấn `F5`

---

## 📋 CÁCH 4: CHẠY BẰNG SCRIPT POWERSHELL TỰ ĐỘNG (Dễ nhất!)

**Đã tạo sẵn script:** `database/run_migration.ps1`

**Cách chạy:**

1. **Mở PowerShell** (Windows + X → Windows PowerShell)

2. **Di chuyển đến thư mục project:**
   ```powershell
   cd "C:\Users\ADMIN\OneDrive\Máy tính\LanguageApp"
   ```

3. **Chạy script:**
   ```powershell
   .\database\run_migration.ps1
   ```

**Script sẽ tự động:**
- ✅ Đọc connection string từ `appsettings.json`
- ✅ Extract database name (`LanguageAppDb`)
- ✅ Extract server name (`localhost`)
- ✅ Chạy migration
- ✅ Hiển thị kết quả

**Nếu thành công, bạn sẽ thấy:**
```
✅ Migration thành công!
Đã thêm cột SoVeChoiGame vào bảng HocSinh.
Migration hoàn tất!
```

**Nếu gặp lỗi, script sẽ hiển thị hướng dẫn xử lý.**

---

## ✅ KIỂM TRA SAU KHI CHẠY MIGRATION

### Test 1: Kiểm tra cột đã tồn tại

```sql
SELECT COLUMN_NAME 
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'HocSinh' 
  AND COLUMN_NAME = 'SoVeChoiGame';
```

**Kết quả:** Phải có 1 dòng với `COLUMN_NAME = 'SoVeChoiGame'`

### Test 2: Kiểm tra giá trị mặc định

```sql
SELECT TOP 5 
    HocSinhID, 
    TenDangNhap, 
    TongDiem, 
    SoVeChoiGame 
FROM HocSinh;
```

**Kết quả:** Cột `SoVeChoiGame` hiển thị với giá trị `0` hoặc `NULL` (sẽ được set thành 0)

### Test 3: Test update giá trị

```sql
-- Test update (không ảnh hưởng dữ liệu thật)
UPDATE HocSinh 
SET SoVeChoiGame = 5 
WHERE HocSinhID = 1;

-- Kiểm tra
SELECT SoVeChoiGame FROM HocSinh WHERE HocSinhID = 1;

-- Reset về 0
UPDATE HocSinh 
SET SoVeChoiGame = 0 
WHERE HocSinhID = 1;
```

---

## 🐛 XỬ LÝ LỖI THƯỜNG GẶP

### Lỗi 1: "Cannot open database"
**Nguyên nhân:** Database name sai hoặc database chưa tồn tại
**Giải pháp:**
1. Kiểm tra tên database trong `appsettings.json`
2. Đảm bảo database đã được tạo
3. Chạy script tạo database trước (nếu cần)

### Lỗi 2: "Login failed"
**Nguyên nhân:** Không có quyền truy cập
**Giải pháp:**
1. Dùng Windows Authentication (nếu có quyền)
2. Hoặc dùng SQL Server Authentication với user có quyền
3. Liên hệ DBA để cấp quyền

### Lỗi 3: "Column 'SoVeChoiGame' already exists"
**Nguyên nhân:** Migration đã chạy rồi
**Giải pháp:**
- Không sao! Script có kiểm tra `IF NOT EXISTS`, sẽ bỏ qua
- Hoặc kiểm tra xem cột đã tồn tại chưa bằng query ở trên

### Lỗi 4: "Invalid object name 'HocSinh'"
**Nguyên nhân:** 
- Chưa chọn đúng database
- Hoặc bảng `HocSinh` chưa tồn tại
**Giải pháp:**
1. Chọn đúng database từ dropdown
2. Kiểm tra bảng `HocSinh` đã tồn tại:
   ```sql
   SELECT * FROM INFORMATION_SCHEMA.TABLES 
   WHERE TABLE_NAME = 'HocSinh';
   ```

### Lỗi 5: "sqlcmd is not recognized"
**Nguyên nhân:** SQL Server Command Line Tools chưa được cài
**Giải pháp:**
1. Cài SQL Server Command Line Utilities
2. Hoặc dùng SSMS (Cách 1) thay vì command line

---

## 📝 LƯU Ý QUAN TRỌNG

1. **Backup database trước khi chạy migration** (nếu có dữ liệu quan trọng):
   ```sql
   BACKUP DATABASE [TênDatabase] 
   TO DISK = 'C:\Backup\LanguageAppDb.bak';
   ```

2. **Đảm bảo không có ứng dụng nào đang kết nối** đến database khi chạy migration

3. **Kiểm tra connection string** trong `appsettings.json` để biết chính xác database name

4. **Nếu migration thất bại**, có thể rollback bằng cách:
   ```sql
   ALTER TABLE HocSinh DROP COLUMN SoVeChoiGame;
   ```

---

## 🎯 TÓM TẮT NHANH

### ⚡ Cách nhanh nhất (Khuyến nghị):
```powershell
cd "C:\Users\ADMIN\OneDrive\Máy tính\LanguageApp"
.\database\run_migration.ps1
```
**Xong! ✅**

### 📝 Cách thủ công (Nếu script không chạy được):
1. Mở SQL Server Management Studio
2. Kết nối đến `localhost`
3. Chọn database `LanguageAppDb`
4. Mở file `database/add_ticket_column_migration.sql`
5. Copy và paste vào cửa sổ query
6. Click "Execute" (F5)
7. Kiểm tra message: "Migration hoàn tất!"

**Xong! ✅**

---

**Nếu vẫn gặp khó khăn, hãy cho tôi biết lỗi cụ thể bạn gặp phải!**

