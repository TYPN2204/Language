# 📋 HƯỚNG DẪN CÀI ĐẶT VÀ CHẠY DỰ ÁN LANGUAGE APP

## 🎯 Tổng quan

Dự án Language App là một game RPG học tiếng Anh với giao diện desktop game và mini-game tương tác.

---

## 📦 PHẦN 1: CÀI ĐẶT NODE.JS VÀ NPM

### Bước 1: Kiểm tra Node.js và npm đã cài chưa

Mở PowerShell hoặc Command Prompt và chạy:

```powershell
node --version
npm --version
```

Nếu thấy hiển thị số phiên bản (ví dụ: `v18.17.0` và `9.6.7`), bạn đã cài đặt rồi! Bỏ qua bước 2.

### Bước 2: Cài đặt Node.js và npm (nếu chưa có)

1. **Tải Node.js:**
   - Truy cập: https://nodejs.org/
   - Tải phiên bản **LTS (Long Term Support)** - khuyến nghị
   - Hoặc tải phiên bản **Current** - có tính năng mới nhất

2. **Cài đặt:**
   - Chạy file installer vừa tải
   - Chọn "Next" qua các bước
   - **Quan trọng:** Đảm bảo checkbox "Add to PATH" được chọn
   - Hoàn tất cài đặt

3. **Kiểm tra lại:**
   - Mở **PowerShell mới** (quan trọng: phải mở cửa sổ mới)
   - Chạy lại:
     ```powershell
     node --version
     npm --version
     ```

---

## 🚀 PHẦN 2: CÀI ĐẶT DEPENDENCIES CHO FRONTEND

### Bước 1: Di chuyển vào thư mục frontend

```powershell
cd frontend
```

### Bước 2: Cài đặt các package cần thiết

```powershell
npm install
```

Lệnh này sẽ:
- Đọc file `package.json`
- Tải và cài đặt tất cả dependencies bao gồm:
  - `react`, `react-dom`
  - `react-router-dom` (mới thêm)
  - `axios`
  - Và các dev dependencies khác

**Thời gian:** Có thể mất 2-5 phút tùy tốc độ mạng

### Bước 3: Kiểm tra cài đặt thành công

Sau khi `npm install` hoàn thành, bạn sẽ thấy:
- Thư mục `node_modules/` được tạo (chứa tất cả packages)
- File `package-lock.json` được cập nhật

---

## 🎨 PHẦN 3: SETUP FILE ẢNH TOWN BACKGROUND

### Tùy chọn A: Sử dụng placeholder SVG (Đã tạo sẵn)

File `town_background.svg` đã được tạo trong `frontend/public/`. Nếu muốn dùng, cần sửa `Town.tsx`:

```tsx
// Trong frontend/src/pages/Town.tsx, dòng 57
<img src="/town_background.svg" alt="Town Background" ... />
```

### Tùy chọn B: Thêm file ảnh thật (Khuyến nghị)

1. **Chuẩn bị ảnh:**
   - Tên file: `town_background.jpg`
   - Kích thước: 1920x1080px hoặc lớn hơn
   - Format: JPG hoặc PNG
   - Nội dung: Ảnh thị trấn/cảnh quan với các tòa nhà (Trường học, Arcade, Cửa hàng, v.v.)

2. **Đặt file vào:**
   - Copy file `town_background.jpg` vào thư mục: `frontend/public/`

3. **File sẽ tự động được sử dụng** - không cần sửa code

---

## 🗄️ PHẦN 4: SETUP DATABASE (Backend)

### Yêu cầu:
- SQL Server đã cài đặt
- Connection string đã cấu hình trong `backend/appsettings.json`

### Seed dữ liệu:

Backend sẽ tự động seed dữ liệu khi khởi động lần đầu, hoặc:

1. **Chạy SQL script thủ công:**
   ```powershell
   # Mở SQL Server Management Studio
   # Chạy file: database/comprehensive_seed_data.sql
   ```

2. **Hoặc để backend tự động seed:**
   - Backend sẽ gọi `DataSeeder.SeedAsync()` khi khởi động
   - Kiểm tra xem có khóa học chưa, nếu chưa sẽ tự động seed

---

## ▶️ PHẦN 5: CHẠY ỨNG DỤNG

### Chạy Frontend (Terminal 1):

```powershell
cd frontend
npm run dev
```

Frontend sẽ chạy tại: `http://localhost:5173`

### Chạy Backend (Terminal 2):

```powershell
cd backend
dotnet run
```

Backend sẽ chạy tại: `http://localhost:5059` (hoặc port khác nếu đã cấu hình)

### Mở trình duyệt:

- Truy cập: `http://localhost:5173`
- Đăng ký/Đăng nhập tài khoản mới
- Bắt đầu khám phá thị trấn học tập!

---

## 🔧 XỬ LÝ LỖI THƯỜNG GẶP

### Lỗi: "npm is not recognized"

**Nguyên nhân:** Node.js chưa được cài hoặc PATH chưa được cập nhật.

**Giải pháp:**
1. Cài lại Node.js và đảm bảo chọn "Add to PATH"
2. Khởi động lại PowerShell/Command Prompt
3. Nếu vẫn lỗi, thêm thủ công vào PATH:
   - Mở "Environment Variables"
   - Thêm đường dẫn: `C:\Program Files\nodejs\`

### Lỗi: "Cannot find module 'react-router-dom'"

**Nguyên nhân:** Dependencies chưa được cài đặt.

**Giải pháp:**
```powershell
cd frontend
npm install
```

### Lỗi: "Port 5173 already in use"

**Nguyên nhân:** Port đã được sử dụng bởi ứng dụng khác.

**Giải pháp:**
1. Tắt ứng dụng đang dùng port 5173
2. Hoặc thay đổi port trong `vite.config.ts`:
   ```typescript
   server: {
     port: 5174  // Đổi sang port khác
   }
   ```

### Lỗi: Backend không kết nối được database

**Nguyên nhân:** Connection string sai hoặc SQL Server chưa chạy.

**Giải pháp:**
1. Kiểm tra `backend/appsettings.json` - Connection string
2. Đảm bảo SQL Server đang chạy
3. Kiểm tra database đã được tạo chưa

---

## 📝 TÓM TẮT CÁC LỆNH QUAN TRỌNG

```powershell
# 1. Kiểm tra Node.js
node --version
npm --version

# 2. Cài đặt dependencies frontend
cd frontend
npm install

# 3. Chạy frontend
npm run dev

# 4. Chạy backend (terminal khác)
cd backend
dotnet run
```

---

## ✅ CHECKLIST CÀI ĐẶT

- [ ] Node.js và npm đã cài đặt
- [ ] Đã chạy `npm install` trong thư mục `frontend/`
- [ ] File `town_background.jpg` đã được thêm vào `frontend/public/` (hoặc dùng SVG placeholder)
- [ ] SQL Server đã cài đặt và chạy
- [ ] Connection string đã cấu hình trong `backend/appsettings.json`
- [ ] Frontend chạy thành công tại `http://localhost:5173`
- [ ] Backend chạy thành công tại `http://localhost:5059`

---

## 🎉 HOÀN THÀNH!

Sau khi hoàn thành tất cả các bước trên, bạn đã sẵn sàng sử dụng Language App!

Chúc bạn học tập vui vẻ! 📚🎮

