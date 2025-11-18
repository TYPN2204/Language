# 🚀 CÁC BƯỚC TIẾP THEO SAU KHI CHẠY MIGRATION

## ✅ BƯỚC 1: KIỂM TRA MIGRATION THÀNH CÔNG

### Kiểm tra trong SQL Server:

```sql
-- Kiểm tra cột đã tồn tại
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'HocSinh' AND COLUMN_NAME = 'SoVeChoiGame';

-- Kiểm tra giá trị hiện tại
SELECT TOP 5 HocSinhID, TenDangNhap, TongDiem, SoVeChoiGame 
FROM HocSinh;
```

**Kết quả mong đợi:**
- Cột `SoVeChoiGame` tồn tại với `DATA_TYPE = 'int'`
- Tất cả học sinh có `SoVeChoiGame = 0` (hoặc NULL sẽ được set thành 0)

---

## 🏃 BƯỚC 2: KHỞI ĐỘNG BACKEND VÀ FRONTEND

### Terminal 1 - Backend:
```powershell
cd backend
dotnet run
```

**Kiểm tra:**
- Backend chạy tại: `http://localhost:5059` (hoặc port khác)
- Không có lỗi compile
- Console hiển thị: "Now listening on: http://localhost:5059"

### Terminal 2 - Frontend:
```powershell
cd frontend
npm run dev
```

**Kiểm tra:**
- Frontend chạy tại: `http://localhost:5173`
- Không có lỗi compile
- Browser tự động mở

---

## 🧪 BƯỚC 3: TEST CÁC TÍNH NĂNG MỚI

### Test 1: Kiểm tra API Get Tickets

**Mở Postman, Thunder Client, hoặc browser:**

```
GET http://localhost:5059/api/me/tickets?hocSinhId=1
```

**Kết quả mong đợi:**
```json
{
  "soVeChoiGame": 0,
  "message": null
}
```

---

### Test 2: Hoàn thành Bài học để kiếm 💎

1. **Đăng nhập vào ứng dụng** tại `http://localhost:5173`
2. **Vào trang Trường học** (`/school`)
3. **Chọn một bài học** chưa hoàn thành
4. **Trả lời đúng tất cả câu hỏi** (giữ 3 ❤️)
5. **Kiểm tra:**
   - ✅ Message: "Tuyệt vời! Bạn nhận được 15 💎!"
   - ✅ Số 💎 trong StatusCard tăng 15
   - ✅ Console không có lỗi

**Test với các số tim khác:**
- Sai 1 câu (2 ❤️) → Nhận 10 💎
- Sai 2 câu (1 ❤️) → Nhận 5 💎

---

### Test 3: Mua Vé tại Cửa hàng

1. **Đảm bảo có ít nhất 50 💎** (hoàn thành bài học)
2. **Vào trang Cửa hàng** (`/shop`)
3. **Tìm section "🎫 Vé Chơi Game"** ở đầu trang
4. **Nhấn "Mua 1 vé (50 💎)"**
5. **Kiểm tra:**
   - ✅ Số 💎 giảm 50
   - ✅ Số vé trong StatusCard tăng 1
   - ✅ Message: "Bạn đã mua thành công 1 vé chơi game!"

---

### Test 4: Quay Slot Machine

1. **Vào trang Arcade** (`/arcade`)
2. **Kiểm tra giao diện:**
   - ✅ Hiển thị số vé: "Bạn đang có: X vé chơi game"
   - ✅ Nút "Sử dụng 1 vé và Quay số! 🎰" sáng (nếu có vé)
3. **Nhấn nút quay số**
4. **Quan sát:**
   - ✅ Animation cuộn trong 2-3 giây
   - ✅ Dừng ở một game ngẫu nhiên
   - ✅ Hiển thị 2 nút: "Chơi game" (xanh) và "Để sau" (đỏ)
   - ✅ Số vé đã giảm 1

---

### Test 5: Chơi Matching Cards Game

1. **Sau khi quay Slot Machine, chọn "Matching Cards"**
2. **Nhấn "Chơi game"** (màu xanh)
3. **Kiểm tra:**
   - ✅ Điều hướng đến `/games/matching-cards`
   - ✅ Trang game có nút "Bắt đầu" và "Hướng dẫn"
4. **Nhấn "Bắt đầu"**
5. **Chơi game và hoàn thành:**
   - ✅ Nhận phần thưởng 💎
   - ✅ Có thể quay lại Arcade

---

## ✅ CHECKLIST HOÀN CHỈNH

### Backend APIs:
- [ ] GET /api/me/tickets hoạt động
- [ ] POST /api/shop/buy-ticket hoạt động
- [ ] POST /api/arcade/use-ticket hoạt động
- [ ] POST /api/lessons/complete với remainingHearts hoạt động

### Frontend - Bài học:
- [ ] Hoàn thành bài học với 3 tim → Nhận 15 💎
- [ ] Hoàn thành bài học với 2 tim → Nhận 10 💎
- [ ] Hoàn thành bài học với 1 tim → Nhận 5 💎
- [ ] Message hiển thị đúng

### Frontend - Vé chơi game:
- [ ] StatusCard hiển thị số vé
- [ ] Shop page có section mua vé
- [ ] Mua vé thành công
- [ ] Số vé cập nhật real-time

### Frontend - Arcade & Slot Machine:
- [ ] Arcade hiển thị số vé
- [ ] Nút quay số hoạt động
- [ ] Slot Machine animation hoạt động
- [ ] Chọn game ngẫu nhiên
- [ ] Điều hướng đến game page

### Frontend - Game Page:
- [ ] Trang game có nút "Bắt đầu" và "Hướng dẫn"
- [ ] Game bắt đầu khi nhấn "Bắt đầu"
- [ ] Hoàn thành game nhận phần thưởng

---

## 🐛 NẾU GẶP LỖI

### Lỗi: "Cannot read property 'soVeChoiGame' of undefined"
**Nguyên nhân:** Backend chưa trả về `soVeChoiGame` trong response
**Giải pháp:** 
- Kiểm tra `StudentStatusFactory.cs` đã cập nhật chưa
- Restart backend

### Lỗi: "remainingHearts is required"
**Nguyên nhân:** Frontend chưa gửi `remainingHearts`
**Giải pháp:**
- Kiểm tra `School.tsx` đã cập nhật chưa
- Kiểm tra `CompleteLessonRequest` có `remainingHearts` chưa

### Lỗi: Route "/games/matching-cards" không hoạt động
**Nguyên nhân:** Chưa thêm route vào `App.tsx`
**Giải pháp:**
- Kiểm tra `App.tsx` đã có route chưa
- Restart frontend

### Lỗi: Slot Machine không hiển thị
**Nguyên nhân:** CSS chưa được import
**Giải pháp:**
- Kiểm tra `SlotMachine.css` đã được import chưa
- Kiểm tra console có lỗi CSS không

---

## 📝 GHI CHÚ

- Tất cả các test nên được chạy trên môi trường development
- Kiểm tra Console (F12) để xem lỗi JavaScript nếu có
- Kiểm tra Network tab để xem API calls
- Nếu backend có lỗi, kiểm tra terminal backend

---

## 🎉 HOÀN THÀNH!

Sau khi test xong tất cả các tính năng, bạn đã hoàn thành Gameplay Overhaul 2.0!

**Các tính năng mới:**
- ✅ Hệ thống Đá Quý dựa trên số tim còn lại
- ✅ Hệ thống Vé Chơi Game
- ✅ Slot Machine để chọn game ngẫu nhiên
- ✅ Trang game riêng biệt với hướng dẫn

**Chúc bạn test thành công! 🚀**

