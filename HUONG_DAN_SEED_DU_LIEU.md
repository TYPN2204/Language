# 📚 HƯỚNG DẪN SEED DỮ LIỆU BÀI HỌC

## 🎯 VẤN ĐỀ
Trang School hiển thị danh sách khóa học nhưng chưa có dữ liệu bài học và câu hỏi.

---

## ✅ CÁCH 1: CHẠY SQL SCRIPT (Khuyến nghị - Nhanh nhất)

### Bước 1: Mở SQL Server Management Studio

1. Mở **SQL Server Management Studio**
2. Kết nối đến `localhost`
3. Chọn database `LanguageAppDb`

### Bước 2: Chạy Script Seed Data

1. **Mở file:** `database/comprehensive_seed_data.sql`
2. **Copy toàn bộ nội dung** (Ctrl + A, Ctrl + C)
3. **Paste vào cửa sổ query** trong SSMS
4. **Đảm bảo đã chọn database `LanguageAppDb`** (từ dropdown hoặc thêm `USE LanguageAppDb;` ở đầu)
5. **Nhấn F5** (Execute)

### Bước 3: Kiểm tra kết quả

**Chạy query kiểm tra:**
```sql
-- Kiểm tra số khóa học
SELECT COUNT(*) AS SoKhoaHoc FROM KhoaHoc;

-- Kiểm tra số bài học
SELECT COUNT(*) AS SoBaiHoc FROM BaiHoc;

-- Kiểm tra số câu hỏi
SELECT COUNT(*) AS SoCauHoi FROM CauHoiTracNghiem;

-- Xem danh sách khóa học
SELECT KhoaHocID, TenKhoaHoc, MoTa FROM KhoaHoc;

-- Xem danh sách bài học
SELECT b.BaiHocID, b.TenBaiHoc, k.TenKhoaHoc 
FROM BaiHoc b
JOIN KhoaHoc k ON b.KhoaHocID = k.KhoaHocID;
```

**Kết quả mong đợi:**
- Có ít nhất 2-3 khóa học
- Có ít nhất 6-15 bài học
- Có ít nhất 30-75 câu hỏi

---

## ✅ CÁCH 2: BACKEND TỰ ĐỘNG SEED (Nếu chưa có dữ liệu)

Backend sẽ tự động seed dữ liệu khi khởi động **nếu database chưa có dữ liệu**.

### Bước 1: Kiểm tra Program.cs

Backend đã được cấu hình để tự động seed trong `Program.cs`:
```csharp
// Seed dữ liệu nếu chưa có
await DataSeeder.SeedAsync(app.Services);
```

### Bước 2: Khởi động Backend

```powershell
cd backend
dotnet run
```

**Kiểm tra console:**
- Nếu thấy message về seed data → Đã seed thành công
- Nếu không có message → Database đã có dữ liệu hoặc có lỗi

### Bước 3: Kiểm tra trong Database

Chạy query kiểm tra như ở Cách 1.

---

## 🔄 CÁCH 3: XÓA VÀ SEED LẠI (Nếu muốn reset)

**Cảnh báo:** Cách này sẽ xóa tất cả dữ liệu hiện có!

### Bước 1: Xóa dữ liệu cũ

Trong SQL Server Management Studio, chạy:
```sql
USE LanguageAppDb;
GO

-- Xóa theo thứ tự (quan trọng!)
DELETE FROM CauHoiTracNghiem;
DELETE FROM TienDo;  -- Xóa tiến độ học tập
DELETE FROM BaiHoc;
DELETE FROM KhoaHoc;
GO
```

### Bước 2: Seed lại

Chạy script `comprehensive_seed_data.sql` như Cách 1.

---

## 📋 DỮ LIỆU SẼ ĐƯỢC SEED

### Khóa học:
1. **Ngữ Pháp Cơ Bản** (5 bài học)
   - Danh từ và Cách sử dụng
   - Động từ và Thì Hiện Tại Đơn
   - Thì Hiện Tại Tiếp Diễn
   - Tính từ và Trạng từ
   - Đại từ và Mạo từ

2. **Từ Vựng Cơ Bản** (5 bài học)
   - Gia đình và Người thân
   - Trường học và Học tập
   - Màu sắc và Hình dạng
   - Số đếm và Số thứ tự
   - Động vật và Thiên nhiên

3. **Hội Thoại Cơ Bản** (5 bài học)
   - Chào hỏi và Giới thiệu
   - Mua sắm
   - Hỏi đường
   - Đặt món ăn
   - Gọi taxi

**Tổng cộng:**
- 3 khóa học
- 15 bài học
- 75 câu hỏi (5 câu mỗi bài)

---

## ✅ KIỂM TRA SAU KHI SEED

### 1. Kiểm tra trong Database:
```sql
SELECT 
    k.TenKhoaHoc,
    COUNT(b.BaiHocID) AS SoBaiHoc,
    COUNT(c.CauHoiID) AS SoCauHoi
FROM KhoaHoc k
LEFT JOIN BaiHoc b ON k.KhoaHocID = b.KhoaHocID
LEFT JOIN CauHoiTracNghiem c ON b.BaiHocID = c.BaiHocID
GROUP BY k.KhoaHocID, k.TenKhoaHoc;
```

### 2. Kiểm tra trong Frontend:

1. **Refresh trang School** (`/school`)
2. **Kiểm tra:**
   - ✅ Hiển thị danh sách khóa học
   - ✅ Mỗi khóa học có danh sách bài học
   - ✅ Có nút "Học ngay" cho mỗi bài học
3. **Chọn một bài học:**
   - ✅ Hiển thị câu hỏi
   - ✅ Có thể trả lời và nhận 💎

---

## 🐛 XỬ LÝ LỖI

### Lỗi: "Cannot insert duplicate key"
**Nguyên nhân:** Dữ liệu đã tồn tại
**Giải pháp:** 
- Kiểm tra xem đã có dữ liệu chưa
- Nếu muốn seed lại, xóa dữ liệu cũ trước (Cách 3)

### Lỗi: "Foreign key constraint"
**Nguyên nhân:** Xóa dữ liệu không đúng thứ tự
**Giải pháp:**
- Xóa theo thứ tự: CauHoiTracNghiem → BaiHoc → KhoaHoc

### Lỗi: Backend không seed tự động
**Nguyên nhân:** 
- Database đã có dữ liệu (seeder chỉ chạy khi chưa có)
- Hoặc có lỗi trong code
**Giải pháp:**
- Dùng Cách 1 (SQL script) thay vì chờ backend seed

---

## 🎯 TÓM TẮT NHANH

**Cách nhanh nhất:**
1. Mở SQL Server Management Studio
2. Chọn database `LanguageAppDb`
3. Mở file `database/comprehensive_seed_data.sql`
4. Copy và paste vào query window
5. Nhấn F5
6. Xong! ✅

**Sau đó refresh trang School để xem dữ liệu mới.**

---

**Nếu vẫn không có dữ liệu, hãy cho tôi biết lỗi cụ thể!**

