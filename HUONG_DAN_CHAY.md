# HƯỚNG DẪN CHẠY DỰ ÁN

## BƯỚC 1: TẮT BACKEND ĐANG CHẠY

1. **Tìm cửa sổ PowerShell/CMD đang chạy `dotnet run`**
   - Nhấn `Ctrl + C` trong cửa sổ đó để dừng backend
   - Hoặc đóng cửa sổ đó

2. **Nếu không thấy cửa sổ, tắt process:**
   - Mở Task Manager (Ctrl + Shift + Esc)
   - Tìm process tên `LanguageApp.Api` hoặc `dotnet`
   - Click chuột phải → End Task

## BƯỚC 2: KHỞI ĐỘNG LẠI BACKEND

1. Mở **PowerShell mới** (hoặc CMD)
2. Chạy lệnh:
   ```
   cd "C:\Users\ADMIN\OneDrive\Máy tính\LanguageApp\backend"
   dotnet run
   ```
3. Đợi thấy dòng: `Now listening on: http://localhost:5059` hoặc `https://localhost:7090`
4. **GIỮ CỬA SỔ NÀY MỞ** (không đóng)

## BƯỚC 3: KHỞI ĐỘNG FRONTEND

1. Mở **PowerShell/CMD mới khác** (giữ backend chạy ở bước 2)
2. Chạy lệnh:
   ```
   cd "C:\Users\ADMIN\OneDrive\Máy tính\LanguageApp\frontend"
   npm run dev
   ```
3. Đợi thấy: `Local: http://localhost:5173`
4. Mở trình duyệt và vào: `http://localhost:5173`

## BƯỚC 4: KIỂM TRA

- Đăng ký/Đăng nhập tài khoản mới
- Click vào các khu vực trên bản đồ thành phố
- Chọn bài học và làm quiz
- Chơi arcade, mua vật phẩm

## LƯU Ý

- **Luôn chạy 2 cửa sổ riêng**: 1 cho backend, 1 cho frontend
- Nếu thay đổi code backend → tắt và chạy lại `dotnet run`
- Nếu thay đổi code frontend → Vite tự động reload (không cần restart)

