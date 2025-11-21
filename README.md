# 🎮 Language App - Game RPG Học Tiếng Anh

## 🚀 Chạy Dự Án

### Yêu Cầu
- .NET 8 SDK
- Node.js 18+ và npm
- SQL Server (LocalDB hoặc SQL Server Express)

### Bước 1: Setup Database

**Cách 1: Chạy bằng PowerShell Script (Nếu gặp lỗi Execution Policy, dùng Cách 2)**

```powershell
cd database
.\run_migration.ps1 -MigrationFile "add_exercise_types_migration.sql"
```

**Cách 2: Chạy trực tiếp SQL (Khuyến nghị)**

Mở SQL Server Management Studio (SSMS) hoặc Azure Data Studio:
1. Kết nối đến database `LanguageAppDb`
2. Mở file `database/add_exercise_types_migration.sql`
3. Chạy toàn bộ script (F5)

Hoặc dùng sqlcmd:
```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -d LanguageAppDb -i "database\add_exercise_types_migration.sql"
```

### Bước 2: Chạy Backend

```powershell
cd backend
dotnet run
```

Backend chạy tại: `http://localhost:5059` hoặc `https://localhost:7090`

### Bước 3: Chạy Frontend

Mở terminal mới:

**Nếu gặp lỗi Execution Policy:**
```powershell
# Cách 1: Bypass tạm thời
powershell -ExecutionPolicy Bypass -Command "cd frontend; npm install; npm run dev"

# Cách 2: Dùng cmd thay vì PowerShell
cmd
cd frontend
npm install
npm run dev
```

**Nếu không gặp lỗi:**
```powershell
cd frontend
npm install  # Chỉ cần chạy lần đầu
npm run dev
```

Frontend chạy tại: `http://localhost:5173`

## 📝 Tính Năng

- 🏫 **Trường học**: Learning Path kiểu Duolingo với nhiều loại bài tập
- 🎯 **Bài tập đa dạng**: Trắc nghiệm, Dịch câu, Chọn cặp, Điền vào chỗ trống, Sắp xếp từ
- 🎮 **Arcade**: Mini-game matching
- 🛒 **Cửa hàng**: Mua vật phẩm bằng Đá Quý
- 📊 **Bảng xếp hạng**: Theo dõi tiến độ học tập

## 🔧 Cấu Trúc Dự Án

```
LanguageApp/
├── backend/          # ASP.NET Core API
├── frontend/         # React + TypeScript + Vite
├── database/         # SQL scripts và migrations
└── automation/       # Scripts tự động hóa
```

## 📌 Lưu Ý

- Luôn chạy backend và frontend ở 2 terminal riêng
- Backend tự động reload khi thay đổi code
- Frontend tự động reload nhờ Vite HMR

## ⚠️ Xử Lý Lỗi Execution Policy

Nếu gặp lỗi "running scripts is disabled" khi chạy PowerShell scripts:

**Giải pháp nhanh:**
- Dùng **CMD** thay vì PowerShell
- Hoặc chạy: `powershell -ExecutionPolicy Bypass -Command "lệnh của bạn"`

**Giải pháp vĩnh viễn (cần quyền Admin):**
```powershell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

