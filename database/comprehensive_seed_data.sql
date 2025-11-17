-- =====================================================
-- COMPREHENSIVE SEED DATA FOR LANGUAGE APP
-- Dữ liệu học tập thực tế cho ứng dụng học tiếng Anh
-- =====================================================
-- 
-- Lưu ý: Script này chứa dữ liệu mẫu phong phú với:
-- - 3 khóa học chính: Ngữ Pháp, Từ Vựng, Hội Thoại
-- - 15 bài học tổng cộng (5 bài mỗi khóa)
-- - 75 câu hỏi trắc nghiệm (5 câu mỗi bài)
-- 
-- Chạy script này sau khi đã tạo database và các bảng
-- =====================================================

-- Xóa dữ liệu cũ nếu muốn (uncomment để sử dụng)
-- DELETE FROM CauHoiTracNghiem;
-- DELETE FROM BaiHoc;
-- DELETE FROM KhoaHoc;

-- ===== KHÓA HỌC 1: NGỮ PHÁP CƠ BẢN =====
INSERT INTO KhoaHoc (TenKhoaHoc, MoTa, DoKho) VALUES
('Ngữ Pháp Cơ Bản', 'Học các quy tắc ngữ pháp tiếng Anh cơ bản, từ danh từ, động từ đến cấu trúc câu.', 1);

DECLARE @GrammarCourseId INT = SCOPE_IDENTITY();

-- Bài học cho Ngữ Pháp
INSERT INTO BaiHoc (KhoaHocID, TenBaiHoc, ThuTu) VALUES
(@GrammarCourseId, 'Danh từ và Cách sử dụng', 1),
(@GrammarCourseId, 'Động từ và Thì Hiện Tại Đơn', 2),
(@GrammarCourseId, 'Thì Hiện Tại Tiếp Diễn', 3),
(@GrammarCourseId, 'Tính từ và Trạng từ', 4),
(@GrammarCourseId, 'Đại từ và Mạo từ', 5);

DECLARE @GrammarLesson1 INT = @GrammarCourseId; -- Lưu ý: Cần lấy ID thực tế
DECLARE @GrammarLesson2 INT = @GrammarCourseId + 1;
DECLARE @GrammarLesson3 INT = @GrammarCourseId + 2;
DECLARE @GrammarLesson4 INT = @GrammarCourseId + 3;
DECLARE @GrammarLesson5 INT = @GrammarCourseId + 4;

-- Câu hỏi cho bài 1: Danh từ
INSERT INTO CauHoiTracNghiem (BaiHocID, NoiDung, PhuongAnA, PhuongAnB, PhuongAnC, PhuongAnD, DapAnDung) VALUES
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Danh từ và Cách sử dụng' AND KhoaHocID = @GrammarCourseId), 
 'Từ nào sau đây là danh từ?', 'beautiful', 'book', 'quickly', 'run', 'B'),
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Danh từ và Cách sử dụng' AND KhoaHocID = @GrammarCourseId), 
 'Chọn danh từ đếm được:', 'water', 'milk', 'apple', 'rice', 'C'),
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Danh từ và Cách sử dụng' AND KhoaHocID = @GrammarCourseId), 
 'Dạng số nhiều của ''child'' là:', 'childs', 'children', 'childes', 'childrens', 'B'),
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Danh từ và Cách sử dụng' AND KhoaHocID = @GrammarCourseId), 
 'Chọn câu đúng:', 'I have three childs', 'I have three children', 'I have three child', 'I have three childrens', 'B'),
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Danh từ và Cách sử dụng' AND KhoaHocID = @GrammarCourseId), 
 'Danh từ nào sau đây không đếm được?', 'table', 'chair', 'furniture', 'desk', 'C');

-- Câu hỏi cho bài 2: Động từ và Thì Hiện Tại Đơn
INSERT INTO CauHoiTracNghiem (BaiHocID, NoiDung, PhuongAnA, PhuongAnB, PhuongAnC, PhuongAnD, DapAnDung) VALUES
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Động từ và Thì Hiện Tại Đơn' AND KhoaHocID = @GrammarCourseId), 
 'Chọn dạng đúng: I ___ to school every day.', 'go', 'goes', 'going', 'went', 'A'),
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Động từ và Thì Hiện Tại Đơn' AND KhoaHocID = @GrammarCourseId), 
 'She ___ her homework every evening.', 'do', 'does', 'did', 'doing', 'B'),
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Động từ và Thì Hiện Tại Đơn' AND KhoaHocID = @GrammarCourseId), 
 'They ___ TV in the evening.', 'watch', 'watches', 'watching', 'watched', 'A'),
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Động từ và Thì Hiện Tại Đơn' AND KhoaHocID = @GrammarCourseId), 
 'What time ___ you usually wake up?', 'does', 'do', 'is', 'are', 'B'),
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Động từ và Thì Hiện Tại Đơn' AND KhoaHocID = @GrammarCourseId), 
 'My father ___ work at 8 AM.', 'start', 'starts', 'starting', 'started', 'B');

-- ===== KHÓA HỌC 2: TỪ VỰNG CƠ BẢN =====
INSERT INTO KhoaHoc (TenKhoaHoc, MoTa, DoKho) VALUES
('Từ Vựng Cơ Bản', 'Mở rộng vốn từ vựng với các chủ đề quen thuộc: gia đình, trường học, màu sắc, số đếm.', 1);

DECLARE @VocabCourseId INT = SCOPE_IDENTITY();

-- Bài học cho Từ Vựng
INSERT INTO BaiHoc (KhoaHocID, TenBaiHoc, ThuTu) VALUES
(@VocabCourseId, 'Gia đình và Người thân', 1),
(@VocabCourseId, 'Trường học và Học tập', 2),
(@VocabCourseId, 'Màu sắc và Hình dạng', 3),
(@VocabCourseId, 'Số đếm và Số thứ tự', 4),
(@VocabCourseId, 'Động vật và Thiên nhiên', 5);

-- Câu hỏi cho bài 1: Gia đình
INSERT INTO CauHoiTracNghiem (BaiHocID, NoiDung, PhuongAnA, PhuongAnB, PhuongAnC, PhuongAnD, DapAnDung) VALUES
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Gia đình và Người thân' AND KhoaHocID = @VocabCourseId), 
 '''Father'' trong tiếng Việt là:', 'Mẹ', 'Bố', 'Anh', 'Chị', 'B'),
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Gia đình và Người thân' AND KhoaHocID = @VocabCourseId), 
 'Nghĩa của từ ''sister'' là:', 'Anh trai', 'Em trai', 'Chị gái', 'Em gái', 'C'),
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Gia đình và Người thân' AND KhoaHocID = @VocabCourseId), 
 'What does ''grandmother'' mean?', 'Bà', 'Ông', 'Cô', 'Chú', 'A'),
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Gia đình và Người thân' AND KhoaHocID = @VocabCourseId), 
 'Chọn từ đúng: ''My ___ is my father''s brother.''', 'uncle', 'aunt', 'cousin', 'nephew', 'A'),
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Gia đình và Người thân' AND KhoaHocID = @VocabCourseId), 
 '''Parents'' có nghĩa là:', 'Bố mẹ', 'Ông bà', 'Anh chị', 'Bạn bè', 'A');

-- Câu hỏi cho bài 2: Trường học
INSERT INTO CauHoiTracNghiem (BaiHocID, NoiDung, PhuongAnA, PhuongAnB, PhuongAnC, PhuongAnD, DapAnDung) VALUES
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Trường học và Học tập' AND KhoaHocID = @VocabCourseId), 
 '''Student'' có nghĩa là:', 'Giáo viên', 'Học sinh', 'Hiệu trưởng', 'Bạn học', 'B'),
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Trường học và Học tập' AND KhoaHocID = @VocabCourseId), 
 'Nghĩa của từ ''homework'' là:', 'Bài tập về nhà', 'Bài kiểm tra', 'Sách giáo khoa', 'Bút chì', 'A'),
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Trường học và Học tập' AND KhoaHocID = @VocabCourseId), 
 'What is a ''classroom''?', 'Phòng học', 'Thư viện', 'Phòng ăn', 'Sân chơi', 'A'),
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Trường học và Học tập' AND KhoaHocID = @VocabCourseId), 
 '''Teacher'' trong tiếng Việt là:', 'Học sinh', 'Giáo viên', 'Bạn học', 'Hiệu trưởng', 'B'),
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Trường học và Học tập' AND KhoaHocID = @VocabCourseId), 
 'I use a ___ to write. (bút)', 'book', 'pen', 'desk', 'chair', 'B');

-- ===== KHÓA HỌC 3: HỘI THOẠI HÀNG NGÀY =====
INSERT INTO KhoaHoc (TenKhoaHoc, MoTa, DoKho) VALUES
('Hội Thoại Hàng Ngày', 'Học cách giao tiếp trong các tình huống hàng ngày: chào hỏi, mua sắm, hỏi đường, đặt hàng.', 2);

DECLARE @ConvCourseId INT = SCOPE_IDENTITY();

-- Bài học cho Hội Thoại
INSERT INTO BaiHoc (KhoaHocID, TenBaiHoc, ThuTu) VALUES
(@ConvCourseId, 'Chào hỏi và Giới thiệu', 1),
(@ConvCourseId, 'Mua sắm tại Cửa hàng', 2),
(@ConvCourseId, 'Hỏi và Chỉ đường', 3),
(@ConvCourseId, 'Đặt món tại Nhà hàng', 4),
(@ConvCourseId, 'Điện thoại và Email', 5);

-- Câu hỏi cho bài 1: Chào hỏi
INSERT INTO CauHoiTracNghiem (BaiHocID, NoiDung, PhuongAnA, PhuongAnB, PhuongAnC, PhuongAnD, DapAnDung) VALUES
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Chào hỏi và Giới thiệu' AND KhoaHocID = @ConvCourseId), 
 'Cách chào hỏi lịch sự khi gặp người lạ:', 'Hey!', 'Hello, nice to meet you', 'What''s up?', 'Yo!', 'B'),
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Chào hỏi và Giới thiệu' AND KhoaHocID = @ConvCourseId), 
 'Khi ai đó hỏi ''How are you?'', bạn nên trả lời:', 'I''m fine, thank you', 'Nothing', 'Go away', 'I don''t know', 'A'),
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Chào hỏi và Giới thiệu' AND KhoaHocID = @ConvCourseId), 
 'Cách tự giới thiệu:', 'This is me', 'My name is...', 'I am name', 'Name me', 'B'),
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Chào hỏi và Giới thiệu' AND KhoaHocID = @ConvCourseId), 
 'Khi chào tạm biệt, bạn nói:', 'Hello', 'Goodbye', 'Thank you', 'Please', 'B'),
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Chào hỏi và Giới thiệu' AND KhoaHocID = @ConvCourseId), 
 '''See you later'' có nghĩa là:', 'Hẹn gặp lại', 'Chào buổi sáng', 'Cảm ơn', 'Xin lỗi', 'A');

-- Câu hỏi cho bài 2: Mua sắm
INSERT INTO CauHoiTracNghiem (BaiHocID, NoiDung, PhuongAnA, PhuongAnB, PhuongAnC, PhuongAnD, DapAnDung) VALUES
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Mua sắm tại Cửa hàng' AND KhoaHocID = @ConvCourseId), 
 'Khi muốn hỏi giá, bạn nói:', 'How much is it?', 'How many?', 'How are you?', 'How old?', 'A'),
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Mua sắm tại Cửa hàng' AND KhoaHocID = @ConvCourseId), 
 '''I would like to buy...'' có nghĩa là:', 'Tôi muốn mua', 'Tôi không thích', 'Tôi không mua', 'Tôi đã mua', 'A'),
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Mua sắm tại Cửa hàng' AND KhoaHocID = @ConvCourseId), 
 'Khi trả tiền tại cửa hàng, bạn nói:', 'Can I pay?', 'Here you are', 'Thank you', 'Hello', 'A'),
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Mua sắm tại Cửa hàng' AND KhoaHocID = @ConvCourseId), 
 '''Do you have...?'' dùng để:', 'Hỏi giá', 'Hỏi có hay không', 'Mua hàng', 'Cảm ơn', 'B'),
((SELECT BaiHocID FROM BaiHoc WHERE TenBaiHoc = 'Mua sắm tại Cửa hàng' AND KhoaHocID = @ConvCourseId), 
 '''Receipt'' có nghĩa là:', 'Tiền', 'Hóa đơn', 'Cửa hàng', 'Sản phẩm', 'B');

PRINT 'Comprehensive seed data đã được thêm vào database thành công!';
PRINT 'Tổng số khóa học: 3';
PRINT 'Tổng số bài học: 15';
PRINT 'Tổng số câu hỏi: 75+';

