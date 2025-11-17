# Automation & Chatbot Setup

This folder chứa cấu hình n8n mẫu cho Chatbot AI của Game RPG Học Tập.

## Yêu cầu

- n8n (>=1.81) chạy ở chế độ self-host (Docker hoặc Desktop đều được).
- Một API key OpenAI hoặc tương thích.
- Backend của dự án đang chạy và có thể truy cập từ n8n (ví dụ `http://host.docker.internal:5059` khi chạy Docker Desktop trên Windows).

## Quy trình triển khai Chatbot

1. Mở n8n → `Import from File` → chọn `chatbot-workflow.json`.
2. Chỉnh node **HTTP Request – Fetch Courses**:
   - `url`: cập nhật đúng base URL backend của bạn.
3. Chỉnh node **OpenAI**:
   - Chọn credentials OpenAI của bạn và model mong muốn.
4. Publish workflow, ghi nhớ URL webhook (ví dụ `http://localhost:5678/webhook/languageapp/chatbot`).
5. Cập nhật `appsettings.Development.json` hoặc biến môi trường:

```json
"ExternalServices": {
  "Chatbot": {
    "N8nWebhookUrl": "http://localhost:5678/webhook/languageapp/chatbot",
    "N8nApiKey": ""
  }
}
```

6. Khởi động lại API. Từ giờ, endpoint `POST /api/chatbot/ask` sẽ gọi tới workflow n8n; nếu webhook không được cấu hình, API dùng fallback nội bộ dựa trên dữ liệu `BaiHoc`/`CauHoiTracNghiem`.

### Tự động báo cáo phụ huynh (gợi ý)

- Tạo workflow n8n khác theo lịch (Cron):
  1. Node HTTP Request lấy `GET /api/parents/{id}/summary`.
  2. Format nội dung.
  3. Gửi qua email/Zalo Official Account tùy nhu cầu.

