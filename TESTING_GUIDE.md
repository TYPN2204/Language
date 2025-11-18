# 🧪 HƯỚNG DẪN TEST GAMEPLAY OVERHAUL 2.0

## 📋 MỤC LỤC
1. [Chuẩn bị](#chuẩn-bị)
2. [Test Backend APIs](#test-backend-apis)
3. [Test Frontend - Luồng Hoàn thành Bài học](#test-frontend---luồng-hoàn-thành-bài-học)
4. [Test Frontend - Hệ thống Vé Chơi Game](#test-frontend---hệ-thống-vé-chơi-game)
5. [Test Frontend - Slot Machine & Game Page](#test-frontend---slot-machine--game-page)
6. [Checklist Test](#checklist-test)

---

## 🔧 CHUẨN BỊ

### Bước 1: Chạy Migration Database

**Quan trọng:** Phải chạy migration trước khi test!

1. **Mở SQL Server Management Studio** (hoặc tool SQL bạn dùng)

2. **Kết nối đến database** của project

3. **Chạy script migration:**
   ```sql
   -- Mở file: database/add_ticket_column_migration.sql
   -- Copy toàn bộ nội dung và chạy trong SQL Server
   ```

4. **Kiểm tra migration thành công:**
   ```sql
   -- Chạy query này để kiểm tra cột đã được thêm chưa
   SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, COLUMN_DEFAULT
   FROM INFORMATION_SCHEMA.COLUMNS
   WHERE TABLE_NAME = 'HocSinh' AND COLUMN_NAME = 'SoVeChoiGame';
   ```
   
   Kết quả mong đợi: Cột `SoVeChoiGame` với `DATA_TYPE = 'int'`, `IS_NULLABLE = 'YES'`, `COLUMN_DEFAULT = '0'`

### Bước 2: Khởi động Backend và Frontend

**Terminal 1 - Backend:**
```powershell
cd backend
dotnet run
```

Backend sẽ chạy tại: `http://localhost:5059` (hoặc port khác trong `launchSettings.json`)

**Terminal 2 - Frontend:**
```powershell
cd frontend
npm run dev
```

Frontend sẽ chạy tại: `http://localhost:5173`

---

## 🧪 TEST BACKEND APIS

### Test 1: Kiểm tra cột SoVeChoiGame trong Database

**Cách test:**
1. Mở SQL Server Management Studio
2. Chạy query:
   ```sql
   SELECT HocSinhID, TenDangNhap, TongDiem, SoVeChoiGame
   FROM HocSinh
   WHERE HocSinhID = 1; -- Thay bằng ID học sinh của bạn
   ```

**Kết quả mong đợi:**
- Cột `SoVeChoiGame` tồn tại
- Giá trị mặc định là `0` hoặc `NULL` (sẽ được set thành 0)

---

### Test 2: API GET /api/me/tickets

**Endpoint:** `GET http://localhost:5059/api/me/tickets?hocSinhId=1`

**Cách test:**
- Dùng Postman, Thunder Client, hoặc curl:
  ```bash
  curl "http://localhost:5059/api/me/tickets?hocSinhId=1"
  ```

**Kết quả mong đợi:**
```json
{
  "soVeChoiGame": 0,
  "message": null
}
```

**Test case:**
- ✅ HocSinhId hợp lệ → Trả về số vé
- ✅ HocSinhId không tồn tại → 404 Not Found

---

### Test 3: API POST /api/shop/buy-ticket

**Endpoint:** `POST http://localhost:5059/api/shop/buy-ticket`

**Body:**
```json
{
  "hocSinhId": 1,
  "quantity": 1
}
```

**Cách test:**
```bash
curl -X POST "http://localhost:5059/api/shop/buy-ticket" \
  -H "Content-Type: application/json" \
  -d '{"hocSinhId": 1, "quantity": 1}'
```

**Kết quả mong đợi:**
```json
{
  "hocSinhId": 1,
  "tenDangNhap": "testuser",
  "tongDiem": 50,  // Đã trừ 50 💎
  "soVeChoiGame": 1,  // Đã cộng 1 vé
  "message": "Bạn đã mua thành công 1 vé chơi game! (Đã trừ 50 💎)"
}
```

**Test cases:**
- ✅ Đủ 💎 (≥50) → Mua thành công, trừ 💎, cộng vé
- ✅ Không đủ 💎 (<50) → 400 Bad Request với message lỗi
- ✅ HocSinhId không tồn tại → 404 Not Found
- ✅ Mua nhiều vé (quantity > 1) → Tính đúng tổng cost

---

### Test 4: API POST /api/arcade/use-ticket

**Endpoint:** `POST http://localhost:5059/api/arcade/use-ticket`

**Body:**
```json
{
  "hocSinhId": 1
}
```

**Cách test:**
```bash
curl -X POST "http://localhost:5059/api/arcade/use-ticket" \
  -H "Content-Type: application/json" \
  -d '{"hocSinhId": 1}'
```

**Kết quả mong đợi:**
```json
{
  "soVeChoiGame": 0,  // Đã trừ 1 vé
  "message": "Bạn đã sử dụng 1 vé chơi game thành công!"
}
```

**Test cases:**
- ✅ Có vé (≥1) → Sử dụng thành công, trừ 1 vé
- ✅ Không có vé (0) → 400 Bad Request với message lỗi
- ✅ HocSinhId không tồn tại → 404 Not Found

---

### Test 5: API POST /api/lessons/complete (với remainingHearts)

**Endpoint:** `POST http://localhost:5059/api/lessons/complete`

**Body:**
```json
{
  "hocSinhId": 1,
  "baiHocId": 1,
  "diemSo": 100,
  "remainingHearts": 3
}
```

**Cách test:**
```bash
curl -X POST "http://localhost:5059/api/lessons/complete" \
  -H "Content-Type: application/json" \
  -d '{"hocSinhId": 1, "baiHocId": 1, "diemSo": 100, "remainingHearts": 3}'
```

**Kết quả mong đợi:**
```json
{
  "hocSinhId": 1,
  "tongDiem": 115,  // Đã cộng 15 💎 (vì remainingHearts = 3)
  "message": "Tuyệt vời! Bạn nhận được 15 💎!"
}
```

**Test cases cho remainingHearts:**
- ✅ `remainingHearts = 3` → +15 💎
- ✅ `remainingHearts = 2` → +10 💎
- ✅ `remainingHearts = 1` → +5 💎
- ✅ `remainingHearts = 0` → +0 💎
- ✅ Không gửi `remainingHearts` → Validation error (nếu có)

**Lưu ý:** Đảm bảo bài học chưa được hoàn thành trước đó, nếu không sẽ bị 409 Conflict.

---

## 🎮 TEST FRONTEND - LUỒNG HOÀN THÀNH BÀI HỌC

### Test 6: Hoàn thành Bài học với Tim còn lại

**Bước 1:** Đăng nhập vào ứng dụng

**Bước 2:** Vào trang **Trường học** (`/school`)

**Bước 3:** Chọn một bài học chưa hoàn thành

**Bước 4:** Trả lời các câu hỏi:
- **Trả lời đúng tất cả** → Còn 3 ❤️ → Nhận **15 💎**
- **Sai 1 câu** → Còn 2 ❤️ → Nhận **10 💎**
- **Sai 2 câu** → Còn 1 ❤️ → Nhận **5 💎**
- **Sai 3 câu** → Hết tim → Game Over (không nhận 💎)

**Bước 5:** Kiểm tra:
- ✅ Thông báo hiển thị: "Tuyệt vời! Bạn nhận được X 💎!"
- ✅ Số 💎 trong StatusCard tăng đúng
- ✅ Console không có lỗi

**Test cases:**
- ✅ Hoàn thành với 3 tim → Nhận 15 💎
- ✅ Hoàn thành với 2 tim → Nhận 10 💎
- ✅ Hoàn thành với 1 tim → Nhận 5 💎
- ✅ Hết tim trước khi hoàn thành → Không nhận 💎

---

## 🎫 TEST FRONTEND - HỆ THỐNG VÉ CHƠI GAME

### Test 7: Mua Vé tại Cửa hàng

**Bước 1:** Đảm bảo bạn có ít nhất **50 💎** (hoàn thành bài học để kiếm 💎)

**Bước 2:** Vào trang **Cửa hàng** (`/shop`)

**Bước 3:** Tìm section **"🎫 Vé Chơi Game"** ở đầu trang

**Bước 4:** Kiểm tra giao diện:
- ✅ Hiển thị số vé hiện có: "Bạn đang có: X 🎫 vé"
- ✅ Có 3 nút: "Mua 1 vé (50 💎)", "Mua 2 vé (100 💎)", "Mua 5 vé (250 💎)"
- ✅ Nút bị disabled nếu không đủ 💎
- ✅ Có cảnh báo nếu không đủ 💎

**Bước 5:** Nhấn "Mua 1 vé (50 💎)"

**Bước 6:** Kiểm tra:
- ✅ Số 💎 trong StatusCard giảm 50
- ✅ Số vé trong StatusCard tăng 1
- ✅ Message: "Bạn đã mua thành công 1 vé chơi game! (Đã trừ 50 💎)"
- ✅ Số vé trong section "Vé Chơi Game" cập nhật

**Test cases:**
- ✅ Đủ 💎 → Mua thành công, trừ 💎, cộng vé
- ✅ Không đủ 💎 → Nút disabled, hiển thị cảnh báo
- ✅ Mua 2 vé → Trừ 100 💎, cộng 2 vé
- ✅ Mua 5 vé → Trừ 250 💎, cộng 5 vé

**Hoặc test qua API (nếu muốn test nhanh):**
```bash
# Mua 1 vé
curl -X POST "http://localhost:5059/api/shop/buy-ticket" \
  -H "Content-Type: application/json" \
  -d '{"hocSinhId": 1, "quantity": 1}'
```

---

### Test 8: Hiển thị Số Vé trong StatusCard

**Bước 1:** Vào bất kỳ trang nào (Town, School, Arcade...)

**Bước 2:** Kiểm tra StatusCard ở góc trên

**Kết quả mong đợi:**
- ✅ Hiển thị: "Vé chơi game: X 🎫"
- ✅ Số vé cập nhật real-time khi mua/sử dụng

---

## 🎰 TEST FRONTEND - SLOT MACHINE & GAME PAGE

### Test 9: Luồng Quay Slot Machine

**Bước 1:** Đảm bảo bạn có ít nhất **1 vé** (mua qua API hoặc Shop)

**Bước 2:** Vào trang **Arcade** (`/arcade`)

**Bước 3:** Kiểm tra giao diện:
- ✅ Hiển thị số vé: "Bạn đang có: X vé chơi game"
- ✅ Nút "Sử dụng 1 vé và Quay số! 🎰" sáng (nếu có vé)
- ✅ Nút bị disabled và có cảnh báo (nếu không có vé)

**Bước 4:** Nhấn nút "Sử dụng 1 vé và Quay số!"

**Bước 5:** Quan sát Slot Machine:
- ✅ Animation cuộn nhanh trong 2-3 giây
- ✅ Hiển thị danh sách games: Matching Cards, Word Puzzle, Memory Game...
- ✅ Dừng ngẫu nhiên ở một game
- ✅ Hiển thị 2 nút: "Chơi game" (xanh) và "Để sau" (đỏ)

**Bước 6:** Kiểm tra số vé:
- ✅ Số vé đã giảm 1 (trước khi quay)
- ✅ StatusCard cập nhật số vé mới

**Test cases:**
- ✅ Có vé → Quay được, trừ vé
- ✅ Không có vé → Hiển thị lỗi, không quay
- ✅ Nhấn "Để sau" → Quay về Arcade, không mất vé (đã trừ rồi)

---

### Test 10: Chơi Matching Cards Game

**Bước 1:** Sau khi quay Slot Machine, chọn game "Matching Cards"

**Bước 2:** Nhấn nút "Chơi game" (màu xanh)

**Bước 3:** Kiểm tra điều hướng:
- ✅ Chuyển đến trang `/games/matching-cards`
- ✅ Hiển thị header: "🎮 Matching Cards Game"
- ✅ Có nút "Bắt đầu" và "Hướng dẫn"

**Bước 4:** Nhấn "Hướng dẫn":
- ✅ Hiển thị danh sách hướng dẫn chơi
- ✅ Có nút "Đã hiểu!" để quay lại

**Bước 5:** Nhấn "Bắt đầu":
- ✅ Game bắt đầu
- ✅ Hiển thị các thẻ bài
- ✅ Có thể lật thẻ và chơi

**Bước 6:** Hoàn thành game:
- ✅ Nhận phần thưởng 💎
- ✅ Có thể quay lại Arcade

**Test cases:**
- ✅ Chọn "Matching Cards" → Điều hướng đúng
- ✅ Chọn game khác (Word Puzzle, etc.) → Hiển thị "đang phát triển"
- ✅ Nhấn "Để sau" → Quay về Arcade

---

### Test 11: Các Game khác (Chưa implement)

**Bước 1:** Quay Slot Machine

**Bước 2:** Nếu chọn game khác "Matching Cards" (ví dụ: Word Puzzle)

**Kết quả mong đợi:**
- ✅ Hiển thị message: "Game 'Word Puzzle' đang được phát triển. Vui lòng thử lại sau!"
- ✅ Quay về Arcade
- ✅ Vé đã bị trừ (vì đã dùng để quay)

---

## ✅ CHECKLIST TEST

### Backend APIs
- [ ] Migration database thành công
- [ ] GET /api/me/tickets hoạt động
- [ ] POST /api/shop/buy-ticket hoạt động (đủ 💎)
- [ ] POST /api/shop/buy-ticket báo lỗi (không đủ 💎)
- [ ] POST /api/arcade/use-ticket hoạt động (có vé)
- [ ] POST /api/arcade/use-ticket báo lỗi (không có vé)
- [ ] POST /api/lessons/complete với remainingHearts = 3 → +15 💎
- [ ] POST /api/lessons/complete với remainingHearts = 2 → +10 💎
- [ ] POST /api/lessons/complete với remainingHearts = 1 → +5 💎

### Frontend - Bài học
- [ ] Hoàn thành bài học với 3 tim → Nhận 15 💎
- [ ] Hoàn thành bài học với 2 tim → Nhận 10 💎
- [ ] Hoàn thành bài học với 1 tim → Nhận 5 💎
- [ ] Message hiển thị đúng: "Tuyệt vời! Bạn nhận được X 💎!"

### Frontend - Vé chơi game
- [ ] StatusCard hiển thị số vé
- [ ] Shop page có section mua vé
- [ ] Mua 1 vé thành công (50 💎)
- [ ] Mua nhiều vé thành công
- [ ] Nút disabled khi không đủ 💎
- [ ] Số vé cập nhật real-time sau khi mua

### Frontend - Arcade & Slot Machine
- [ ] Arcade hiển thị số vé
- [ ] Nút quay số hoạt động (có vé)
- [ ] Nút quay số bị disabled (không có vé)
- [ ] Slot Machine animation hoạt động
- [ ] Chọn game ngẫu nhiên
- [ ] Nút "Chơi game" và "Để sau" hiển thị

### Frontend - Game Page
- [ ] Điều hướng đến /games/matching-cards
- [ ] Trang game có nút "Bắt đầu" và "Hướng dẫn"
- [ ] Hướng dẫn hiển thị đúng
- [ ] Game bắt đầu khi nhấn "Bắt đầu"
- [ ] Game khác hiển thị "đang phát triển"

---

## 🐛 XỬ LÝ LỖI THƯỜNG GẶP

### Lỗi: "Column 'SoVeChoiGame' does not exist"
**Nguyên nhân:** Chưa chạy migration
**Giải pháp:** Chạy script `database/add_ticket_column_migration.sql`

### Lỗi: "Cannot read property 'soVeChoiGame' of undefined"
**Nguyên nhân:** Backend chưa trả về `soVeChoiGame` trong response
**Giải pháp:** Kiểm tra `StudentStatusFactory.cs` đã cập nhật chưa

### Lỗi: "remainingHearts is required"
**Nguyên nhân:** Frontend chưa gửi `remainingHearts` khi complete lesson
**Giải pháp:** Kiểm tra `School.tsx` đã cập nhật chưa

### Lỗi: Slot Machine không hiển thị
**Nguyên nhân:** CSS chưa được import
**Giải pháp:** Kiểm tra `SlotMachine.css` đã được import trong component

### Lỗi: Route "/games/matching-cards" không hoạt động
**Nguyên nhân:** Chưa thêm route vào `App.tsx`
**Giải pháp:** Kiểm tra `App.tsx` đã có route chưa

---

## 📝 GHI CHÚ

- Tất cả các test nên được chạy trên môi trường development
- Đảm bảo backend và frontend đều đang chạy
- Kiểm tra Console (F12) để xem lỗi JavaScript nếu có
- Kiểm tra Network tab để xem API calls

---

**Chúc bạn test thành công! 🎉**

