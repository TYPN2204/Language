# 🚀 QUICK START - HƯỚNG DẪN NHANH

## 📦 BƯỚC 1: CÀI ĐẶT NPM

### Windows (PowerShell):

```powershell
# 1. Kiểm tra Node.js đã cài chưa
node --version

# Nếu chưa có, tải và cài từ: https://nodejs.org/

# 2. Cài đặt dependencies cho frontend
cd frontend
npm install

# 3. Chờ cài đặt hoàn tất (2-5 phút)
```

### Nếu gặp lỗi "npm is not recognized":
- Cài lại Node.js từ https://nodejs.org/
- Đảm bảo chọn "Add to PATH" khi cài đặt
- Khởi động lại PowerShell

---

## 🎨 BƯỚC 2: FILE ẢNH BACKGROUND

Đã có **SVG placeholder** sẵn tại `frontend/public/town_background.svg`

**Nếu muốn dùng ảnh JPG thật:**
1. Đặt file `town_background.jpg` vào `frontend/public/`
2. Ứng dụng sẽ tự động ưu tiên file JPG

---

## ▶️ BƯỚC 3: CHẠY ỨNG DỤNG

### Terminal 1 - Frontend:
```powershell
cd frontend
npm run dev
```
→ Mở: http://localhost:5173

### Terminal 2 - Backend:
```powershell
cd backend
dotnet run
```
→ API chạy tại: http://localhost:5059

---

## ✅ CHECKLIST NHANH

- [ ] `node --version` hiển thị số phiên bản
- [ ] `cd frontend` và `npm install` chạy thành công
- [ ] Frontend chạy tại http://localhost:5173
- [ ] Backend chạy tại http://localhost:5059
- [ ] Mở trình duyệt và đăng ký tài khoản

---

**Chi tiết đầy đủ xem file: `SETUP_INSTRUCTIONS.md`**

