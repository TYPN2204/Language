# 🔧 SỬA LỖI POWERSHELL EXECUTION POLICY

## ❌ Lỗi gặp phải:
```
npm : File C:\Program Files\nodejs\npm.ps1 cannot be loaded because running scripts is disabled on this system.
```

## 🎯 NGUYÊN NHÂN
PowerShell mặc định chặn chạy scripts để bảo mật. Cần thay đổi Execution Policy.

---

## ✅ GIẢI PHÁP

### CÁCH 1: Thay đổi Execution Policy (Khuyến nghị)

**Bước 1: Mở PowerShell với quyền Administrator**

1. Nhấn `Windows + X`
2. Chọn **"Windows PowerShell (Admin)"** hoặc **"Terminal (Admin)"**
3. Click **"Yes"** khi có UAC prompt

**Bước 2: Kiểm tra Execution Policy hiện tại**

```powershell
Get-ExecutionPolicy
```

**Kết quả có thể là:** `Restricted`, `AllSigned`, `RemoteSigned`, etc.

**Bước 3: Thay đổi Execution Policy**

**Tùy chọn A: Chỉ cho phép scripts local (An toàn nhất - Khuyến nghị)**
```powershell
Set-ExecutionPolicy RemoteSigned -Scope CurrentUser
```

**Tùy chọn B: Cho phép tất cả scripts (Ít an toàn hơn)**
```powershell
Set-ExecutionPolicy Unrestricted -Scope CurrentUser
```

**Giải thích:**
- `RemoteSigned`: Cho phép scripts local chạy, scripts từ internet cần được signed
- `Unrestricted`: Cho phép tất cả scripts (không khuyến nghị)
- `-Scope CurrentUser`: Chỉ áp dụng cho user hiện tại (không ảnh hưởng toàn hệ thống)

**Bước 4: Xác nhận**

Khi được hỏi, nhấn `Y` (Yes)

**Bước 5: Kiểm tra lại**

```powershell
Get-ExecutionPolicy
```

**Kết quả mong đợi:** `RemoteSigned` hoặc `Unrestricted`

**Bước 6: Thử lại npm**

```powershell
cd "C:\Users\ADMIN\OneDrive\Máy tính\LanguageApp\frontend"
npm run dev
```

---

### CÁCH 2: Bypass tạm thời (Không cần Admin)

**Chạy lệnh này mỗi lần mở PowerShell mới:**

```powershell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process
```

**Sau đó chạy npm:**
```powershell
npm run dev
```

**Lưu ý:** Cần chạy lại lệnh này mỗi lần mở PowerShell mới.

---

### CÁCH 3: Dùng Command Prompt thay vì PowerShell

**Mở Command Prompt (cmd.exe):**

1. Nhấn `Windows + R`
2. Gõ `cmd` và nhấn Enter
3. Chạy:
```cmd
cd "C:\Users\ADMIN\OneDrive\Máy tính\LanguageApp\frontend"
npm run dev
```

**Command Prompt không bị ảnh hưởng bởi Execution Policy.**

---

### CÁCH 4: Dùng npx thay vì npm (Nếu có sẵn)

```powershell
npx vite
```

Hoặc:
```powershell
npx vite --host
```

---

## 🔍 KIỂM TRA SAU KHI SỬA

**Chạy lệnh test:**
```powershell
npm --version
```

**Nếu thành công, bạn sẽ thấy:**
```
9.6.7
```
(Số version có thể khác)

---

## ⚠️ LƯU Ý BẢO MẬT

- **RemoteSigned** là lựa chọn an toàn và đủ cho hầu hết các trường hợp
- Chỉ dùng **Unrestricted** nếu thực sự cần thiết
- **CurrentUser scope** chỉ ảnh hưởng user hiện tại, không ảnh hưởng toàn hệ thống

---

## 🎯 TÓM TẮT NHANH

**Cách nhanh nhất (Cần Admin):**
```powershell
# Mở PowerShell (Admin)
Set-ExecutionPolicy RemoteSigned -Scope CurrentUser
# Nhấn Y
```

**Cách không cần Admin:**
- Dùng Command Prompt (cmd.exe) thay vì PowerShell
- Hoặc chạy: `Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process` mỗi lần

---

**Sau khi sửa, thử lại:**
```powershell
cd "C:\Users\ADMIN\OneDrive\Máy tính\LanguageApp\frontend"
npm run dev
```

