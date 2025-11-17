using LanguageApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LanguageApp.Api.Seed;

/// <summary>
/// Comprehensive data seeder với dữ liệu học tập thực tế và phong phú
/// Dựa trên chương trình học tiếng Anh cơ bản cho học sinh
/// </summary>
public static class ComprehensiveDataSeeder
{
    public static async Task SeedComprehensiveDataAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<LanguageAppDbContext>();

        if (!await context.Database.CanConnectAsync())
        {
            return;
        }

        // Chỉ seed nếu chưa có dữ liệu
        if (await context.KhoaHocs.AnyAsync())
        {
            return;
        }

        // ===== KHÓA HỌC 1: NGỮ PHÁP CƠ BẢN =====
        var grammarCourse = new KhoaHoc
        {
            TenKhoaHoc = "Ngữ Pháp Cơ Bản",
            MoTa = "Học các quy tắc ngữ pháp tiếng Anh cơ bản, từ danh từ, động từ đến cấu trúc câu.",
            DoKho = 1
        };

        var grammarLessons = new List<BaiHoc>
        {
            new BaiHoc { TenBaiHoc = "Danh từ và Cách sử dụng", ThuTu = 1 },
            new BaiHoc { TenBaiHoc = "Động từ và Thì Hiện Tại Đơn", ThuTu = 2 },
            new BaiHoc { TenBaiHoc = "Thì Hiện Tại Tiếp Diễn", ThuTu = 3 },
            new BaiHoc { TenBaiHoc = "Tính từ và Trạng từ", ThuTu = 4 },
            new BaiHoc { TenBaiHoc = "Đại từ và Mạo từ", ThuTu = 5 }
        };

        grammarCourse.BaiHocs = grammarLessons;
        await context.KhoaHocs.AddAsync(grammarCourse);
        await context.SaveChangesAsync();

        // Câu hỏi cho bài 1: Danh từ
        var grammar1Questions = new List<CauHoiTracNghiem>
        {
            new CauHoiTracNghiem
            {
                BaiHocID = grammarLessons[0].BaiHocID,
                NoiDung = "Từ nào sau đây là danh từ?",
                PhuongAnA = "beautiful",
                PhuongAnB = "book",
                PhuongAnC = "quickly",
                PhuongAnD = "run",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = grammarLessons[0].BaiHocID,
                NoiDung = "Chọn danh từ đếm được:",
                PhuongAnA = "water",
                PhuongAnB = "milk",
                PhuongAnC = "apple",
                PhuongAnD = "rice",
                DapAnDung = "C"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = grammarLessons[0].BaiHocID,
                NoiDung = "Dạng số nhiều của 'child' là:",
                PhuongAnA = "childs",
                PhuongAnB = "children",
                PhuongAnC = "childes",
                PhuongAnD = "childrens",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = grammarLessons[0].BaiHocID,
                NoiDung = "Chọn câu đúng:",
                PhuongAnA = "I have three childs",
                PhuongAnB = "I have three children",
                PhuongAnC = "I have three child",
                PhuongAnD = "I have three childrens",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = grammarLessons[0].BaiHocID,
                NoiDung = "Danh từ nào sau đây không đếm được?",
                PhuongAnA = "table",
                PhuongAnB = "chair",
                PhuongAnC = "furniture",
                PhuongAnD = "desk",
                DapAnDung = "C"
            }
        };

        // Câu hỏi cho bài 2: Động từ và Thì Hiện Tại Đơn
        var grammar2Questions = new List<CauHoiTracNghiem>
        {
            new CauHoiTracNghiem
            {
                BaiHocID = grammarLessons[1].BaiHocID,
                NoiDung = "Chọn dạng đúng: I ___ to school every day.",
                PhuongAnA = "go",
                PhuongAnB = "goes",
                PhuongAnC = "going",
                PhuongAnD = "went",
                DapAnDung = "A"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = grammarLessons[1].BaiHocID,
                NoiDung = "She ___ her homework every evening.",
                PhuongAnA = "do",
                PhuongAnB = "does",
                PhuongAnC = "did",
                PhuongAnD = "doing",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = grammarLessons[1].BaiHocID,
                NoiDung = "They ___ TV in the evening.",
                PhuongAnA = "watch",
                PhuongAnB = "watches",
                PhuongAnC = "watching",
                PhuongAnD = "watched",
                DapAnDung = "A"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = grammarLessons[1].BaiHocID,
                NoiDung = "What time ___ you usually wake up?",
                PhuongAnA = "does",
                PhuongAnB = "do",
                PhuongAnC = "is",
                PhuongAnD = "are",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = grammarLessons[1].BaiHocID,
                NoiDung = "My father ___ work at 8 AM.",
                PhuongAnA = "start",
                PhuongAnB = "starts",
                PhuongAnC = "starting",
                PhuongAnD = "started",
                DapAnDung = "B"
            }
        };

        // Câu hỏi cho bài 3: Thì Hiện Tại Tiếp Diễn
        var grammar3Questions = new List<CauHoiTracNghiem>
        {
            new CauHoiTracNghiem
            {
                BaiHocID = grammarLessons[2].BaiHocID,
                NoiDung = "I ___ studying English now.",
                PhuongAnA = "am",
                PhuongAnB = "is",
                PhuongAnC = "are",
                PhuongAnD = "be",
                DapAnDung = "A"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = grammarLessons[2].BaiHocID,
                NoiDung = "They ___ playing football at the moment.",
                PhuongAnA = "am",
                PhuongAnB = "is",
                PhuongAnC = "are",
                PhuongAnD = "be",
                DapAnDung = "C"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = grammarLessons[2].BaiHocID,
                NoiDung = "What ___ you doing right now?",
                PhuongAnA = "am",
                PhuongAnB = "is",
                PhuongAnC = "are",
                PhuongAnD = "be",
                DapAnDung = "C"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = grammarLessons[2].BaiHocID,
                NoiDung = "She ___ reading a book now.",
                PhuongAnA = "am",
                PhuongAnB = "is",
                PhuongAnC = "are",
                PhuongAnD = "be",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = grammarLessons[2].BaiHocID,
                NoiDung = "Chọn câu đúng:",
                PhuongAnA = "I am go to school",
                PhuongAnB = "I am going to school",
                PhuongAnC = "I go to school",
                PhuongAnD = "I going to school",
                DapAnDung = "B"
            }
        };

        // Câu hỏi cho bài 4: Tính từ và Trạng từ
        var grammar4Questions = new List<CauHoiTracNghiem>
        {
            new CauHoiTracNghiem
            {
                BaiHocID = grammarLessons[3].BaiHocID,
                NoiDung = "Chọn tính từ:",
                PhuongAnA = "quickly",
                PhuongAnB = "beautiful",
                PhuongAnC = "carefully",
                PhuongAnD = "slowly",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = grammarLessons[3].BaiHocID,
                NoiDung = "She runs very ___. (nhanh)",
                PhuongAnA = "quick",
                PhuongAnB = "quickly",
                PhuongAnC = "quicker",
                PhuongAnD = "quickest",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = grammarLessons[3].BaiHocID,
                NoiDung = "This is a ___ car. (đẹp)",
                PhuongAnA = "beautifully",
                PhuongAnB = "beautiful",
                PhuongAnC = "beauty",
                PhuongAnD = "beautify",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = grammarLessons[3].BaiHocID,
                NoiDung = "He speaks English ___. (tốt)",
                PhuongAnA = "good",
                PhuongAnB = "well",
                PhuongAnC = "better",
                PhuongAnD = "best",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = grammarLessons[3].BaiHocID,
                NoiDung = "She is a ___ student. (chăm chỉ)",
                PhuongAnA = "hard",
                PhuongAnB = "hardly",
                PhuongAnC = "hardly",
                PhuongAnD = "hardest",
                DapAnDung = "A"
            }
        };

        // Câu hỏi cho bài 5: Đại từ và Mạo từ
        var grammar5Questions = new List<CauHoiTracNghiem>
        {
            new CauHoiTracNghiem
            {
                BaiHocID = grammarLessons[4].BaiHocID,
                NoiDung = "___ is my friend. (Anh ấy)",
                PhuongAnA = "He",
                PhuongAnB = "She",
                PhuongAnC = "It",
                PhuongAnD = "They",
                DapAnDung = "A"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = grammarLessons[4].BaiHocID,
                NoiDung = "This is ___ book. (một)",
                PhuongAnA = "a",
                PhuongAnB = "an",
                PhuongAnC = "the",
                PhuongAnD = "no article",
                DapAnDung = "A"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = grammarLessons[4].BaiHocID,
                NoiDung = "I want ___ apple. (một - bắt đầu bằng nguyên âm)",
                PhuongAnA = "a",
                PhuongAnB = "an",
                PhuongAnC = "the",
                PhuongAnD = "no article",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = grammarLessons[4].BaiHocID,
                NoiDung = "That book is ___. (của tôi)",
                PhuongAnA = "me",
                PhuongAnB = "my",
                PhuongAnC = "mine",
                PhuongAnD = "I",
                DapAnDung = "C"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = grammarLessons[4].BaiHocID,
                NoiDung = "Can you help ___? (tôi)",
                PhuongAnA = "I",
                PhuongAnB = "me",
                PhuongAnC = "my",
                PhuongAnD = "mine",
                DapAnDung = "B"
            }
        };

        // ===== KHÓA HỌC 2: TỪ VỰNG CƠ BẢN =====
        var vocabularyCourse = new KhoaHoc
        {
            TenKhoaHoc = "Từ Vựng Cơ Bản",
            MoTa = "Mở rộng vốn từ vựng với các chủ đề quen thuộc: gia đình, trường học, màu sắc, số đếm.",
            DoKho = 1
        };

        var vocabularyLessons = new List<BaiHoc>
        {
            new BaiHoc { TenBaiHoc = "Gia đình và Người thân", ThuTu = 1 },
            new BaiHoc { TenBaiHoc = "Trường học và Học tập", ThuTu = 2 },
            new BaiHoc { TenBaiHoc = "Màu sắc và Hình dạng", ThuTu = 3 },
            new BaiHoc { TenBaiHoc = "Số đếm và Số thứ tự", ThuTu = 4 },
            new BaiHoc { TenBaiHoc = "Động vật và Thiên nhiên", ThuTu = 5 }
        };

        vocabularyCourse.BaiHocs = vocabularyLessons;
        await context.KhoaHocs.AddAsync(vocabularyCourse);
        await context.SaveChangesAsync();

        // Câu hỏi cho bài 1: Gia đình
        var vocab1Questions = new List<CauHoiTracNghiem>
        {
            new CauHoiTracNghiem
            {
                BaiHocID = vocabularyLessons[0].BaiHocID,
                NoiDung = "'Father' trong tiếng Việt là:",
                PhuongAnA = "Mẹ",
                PhuongAnB = "Bố",
                PhuongAnC = "Anh",
                PhuongAnD = "Chị",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = vocabularyLessons[0].BaiHocID,
                NoiDung = "Nghĩa của từ 'sister' là:",
                PhuongAnA = "Anh trai",
                PhuongAnB = "Em trai",
                PhuongAnC = "Chị gái",
                PhuongAnD = "Em gái",
                DapAnDung = "C"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = vocabularyLessons[0].BaiHocID,
                NoiDung = "What does 'grandmother' mean?",
                PhuongAnA = "Bà",
                PhuongAnB = "Ông",
                PhuongAnC = "Cô",
                PhuongAnD = "Chú",
                DapAnDung = "A"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = vocabularyLessons[0].BaiHocID,
                NoiDung = "Chọn từ đúng: 'My ___ is my father's brother.'",
                PhuongAnA = "uncle",
                PhuongAnB = "aunt",
                PhuongAnC = "cousin",
                PhuongAnD = "nephew",
                DapAnDung = "A"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = vocabularyLessons[0].BaiHocID,
                NoiDung = "'Parents' có nghĩa là:",
                PhuongAnA = "Bố mẹ",
                PhuongAnB = "Ông bà",
                PhuongAnC = "Anh chị",
                PhuongAnD = "Bạn bè",
                DapAnDung = "A"
            }
        };

        // Câu hỏi cho bài 2: Trường học
        var vocab2Questions = new List<CauHoiTracNghiem>
        {
            new CauHoiTracNghiem
            {
                BaiHocID = vocabularyLessons[1].BaiHocID,
                NoiDung = "'Student' có nghĩa là:",
                PhuongAnA = "Giáo viên",
                PhuongAnB = "Học sinh",
                PhuongAnC = "Hiệu trưởng",
                PhuongAnD = "Bạn học",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = vocabularyLessons[1].BaiHocID,
                NoiDung = "Nghĩa của từ 'homework' là:",
                PhuongAnA = "Bài tập về nhà",
                PhuongAnB = "Bài kiểm tra",
                PhuongAnC = "Sách giáo khoa",
                PhuongAnD = "Bút chì",
                DapAnDung = "A"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = vocabularyLessons[1].BaiHocID,
                NoiDung = "What is a 'classroom'?",
                PhuongAnA = "Phòng học",
                PhuongAnB = "Thư viện",
                PhuongAnC = "Phòng ăn",
                PhuongAnD = "Sân chơi",
                DapAnDung = "A"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = vocabularyLessons[1].BaiHocID,
                NoiDung = "'Teacher' trong tiếng Việt là:",
                PhuongAnA = "Học sinh",
                PhuongAnB = "Giáo viên",
                PhuongAnC = "Bạn học",
                PhuongAnD = "Hiệu trưởng",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = vocabularyLessons[1].BaiHocID,
                NoiDung = "I use a ___ to write. (bút)",
                PhuongAnA = "book",
                PhuongAnB = "pen",
                PhuongAnC = "desk",
                PhuongAnD = "chair",
                DapAnDung = "B"
            }
        };

        // Câu hỏi cho bài 3: Màu sắc
        var vocab3Questions = new List<CauHoiTracNghiem>
        {
            new CauHoiTracNghiem
            {
                BaiHocID = vocabularyLessons[2].BaiHocID,
                NoiDung = "'Red' có nghĩa là:",
                PhuongAnA = "Xanh",
                PhuongAnB = "Đỏ",
                PhuongAnC = "Vàng",
                PhuongAnD = "Trắng",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = vocabularyLessons[2].BaiHocID,
                NoiDung = "What color is the sky?",
                PhuongAnA = "Red",
                PhuongAnB = "Green",
                PhuongAnC = "Blue",
                PhuongAnD = "Yellow",
                DapAnDung = "C"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = vocabularyLessons[2].BaiHocID,
                NoiDung = "'White' trong tiếng Việt là:",
                PhuongAnA = "Đen",
                PhuongAnB = "Trắng",
                PhuongAnC = "Xám",
                PhuongAnD = "Nâu",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = vocabularyLessons[2].BaiHocID,
                NoiDung = "A circle is a ___. (hình tròn)",
                PhuongAnA = "color",
                PhuongAnB = "shape",
                PhuongAnC = "number",
                PhuongAnD = "animal",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = vocabularyLessons[2].BaiHocID,
                NoiDung = "'Square' có nghĩa là:",
                PhuongAnA = "Hình tròn",
                PhuongAnB = "Hình vuông",
                PhuongAnC = "Hình tam giác",
                PhuongAnD = "Hình chữ nhật",
                DapAnDung = "B"
            }
        };

        // Câu hỏi cho bài 4: Số đếm
        var vocab4Questions = new List<CauHoiTracNghiem>
        {
            new CauHoiTracNghiem
            {
                BaiHocID = vocabularyLessons[3].BaiHocID,
                NoiDung = "'Ten' trong tiếng Việt là:",
                PhuongAnA = "8",
                PhuongAnB = "9",
                PhuongAnC = "10",
                PhuongAnD = "11",
                DapAnDung = "C"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = vocabularyLessons[3].BaiHocID,
                NoiDung = "What is 'twenty'?",
                PhuongAnA = "10",
                PhuongAnB = "20",
                PhuongAnC = "30",
                PhuongAnD = "40",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = vocabularyLessons[3].BaiHocID,
                NoiDung = "'First' có nghĩa là:",
                PhuongAnA = "Thứ nhất",
                PhuongAnB = "Thứ hai",
                PhuongAnC = "Thứ ba",
                PhuongAnD = "Thứ tư",
                DapAnDung = "A"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = vocabularyLessons[3].BaiHocID,
                NoiDung = "Chọn từ đúng: 'I am the ___ in my family.' (thứ hai)",
                PhuongAnA = "first",
                PhuongAnB = "second",
                PhuongAnC = "third",
                PhuongAnD = "fourth",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = vocabularyLessons[3].BaiHocID,
                NoiDung = "'Hundred' có nghĩa là:",
                PhuongAnA = "10",
                PhuongAnB = "100",
                PhuongAnC = "1000",
                PhuongAnD = "10000",
                DapAnDung = "B"
            }
        };

        // Câu hỏi cho bài 5: Động vật
        var vocab5Questions = new List<CauHoiTracNghiem>
        {
            new CauHoiTracNghiem
            {
                BaiHocID = vocabularyLessons[4].BaiHocID,
                NoiDung = "'Cat' trong tiếng Việt là:",
                PhuongAnA = "Chó",
                PhuongAnB = "Mèo",
                PhuongAnC = "Chuột",
                PhuongAnD = "Thỏ",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = vocabularyLessons[4].BaiHocID,
                NoiDung = "What is 'dog'?",
                PhuongAnA = "Mèo",
                PhuongAnB = "Chó",
                PhuongAnC = "Chim",
                PhuongAnD = "Cá",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = vocabularyLessons[4].BaiHocID,
                NoiDung = "'Bird' có nghĩa là:",
                PhuongAnA = "Chim",
                PhuongAnB = "Cá",
                PhuongAnC = "Bướm",
                PhuongAnD = "Ong",
                DapAnDung = "A"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = vocabularyLessons[4].BaiHocID,
                NoiDung = "A ___ lives in water. (con cá)",
                PhuongAnA = "bird",
                PhuongAnB = "cat",
                PhuongAnC = "fish",
                PhuongAnD = "dog",
                DapAnDung = "C"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = vocabularyLessons[4].BaiHocID,
                NoiDung = "'Tree' có nghĩa là:",
                PhuongAnA = "Hoa",
                PhuongAnB = "Cỏ",
                PhuongAnC = "Cây",
                PhuongAnD = "Lá",
                DapAnDung = "C"
            }
        };

        // ===== KHÓA HỌC 3: HỘI THOẠI HÀNG NGÀY =====
        var conversationCourse = new KhoaHoc
        {
            TenKhoaHoc = "Hội Thoại Hàng Ngày",
            MoTa = "Học cách giao tiếp trong các tình huống hàng ngày: chào hỏi, mua sắm, hỏi đường, đặt hàng.",
            DoKho = 2
        };

        var conversationLessons = new List<BaiHoc>
        {
            new BaiHoc { TenBaiHoc = "Chào hỏi và Giới thiệu", ThuTu = 1 },
            new BaiHoc { TenBaiHoc = "Mua sắm tại Cửa hàng", ThuTu = 2 },
            new BaiHoc { TenBaiHoc = "Hỏi và Chỉ đường", ThuTu = 3 },
            new BaiHoc { TenBaiHoc = "Đặt món tại Nhà hàng", ThuTu = 4 },
            new BaiHoc { TenBaiHoc = "Điện thoại và Email", ThuTu = 5 }
        };

        conversationCourse.BaiHocs = conversationLessons;
        await context.KhoaHocs.AddAsync(conversationCourse);
        await context.SaveChangesAsync();

        // Câu hỏi cho bài 1: Chào hỏi
        var conv1Questions = new List<CauHoiTracNghiem>
        {
            new CauHoiTracNghiem
            {
                BaiHocID = conversationLessons[0].BaiHocID,
                NoiDung = "Cách chào hỏi lịch sự khi gặp người lạ:",
                PhuongAnA = "Hey!",
                PhuongAnB = "Hello, nice to meet you",
                PhuongAnC = "What's up?",
                PhuongAnD = "Yo!",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = conversationLessons[0].BaiHocID,
                NoiDung = "Khi ai đó hỏi 'How are you?', bạn nên trả lời:",
                PhuongAnA = "I'm fine, thank you",
                PhuongAnB = "Nothing",
                PhuongAnC = "Go away",
                PhuongAnD = "I don't know",
                DapAnDung = "A"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = conversationLessons[0].BaiHocID,
                NoiDung = "Cách tự giới thiệu:",
                PhuongAnA = "This is me",
                PhuongAnB = "My name is...",
                PhuongAnC = "I am name",
                PhuongAnD = "Name me",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = conversationLessons[0].BaiHocID,
                NoiDung = "Khi chào tạm biệt, bạn nói:",
                PhuongAnA = "Hello",
                PhuongAnB = "Goodbye",
                PhuongAnC = "Thank you",
                PhuongAnD = "Please",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = conversationLessons[0].BaiHocID,
                NoiDung = "'See you later' có nghĩa là:",
                PhuongAnA = "Hẹn gặp lại",
                PhuongAnB = "Chào buổi sáng",
                PhuongAnC = "Cảm ơn",
                PhuongAnD = "Xin lỗi",
                DapAnDung = "A"
            }
        };

        // Câu hỏi cho bài 2: Mua sắm
        var conv2Questions = new List<CauHoiTracNghiem>
        {
            new CauHoiTracNghiem
            {
                BaiHocID = conversationLessons[1].BaiHocID,
                NoiDung = "Khi muốn hỏi giá, bạn nói:",
                PhuongAnA = "How much is it?",
                PhuongAnB = "How many?",
                PhuongAnC = "How are you?",
                PhuongAnD = "How old?",
                DapAnDung = "A"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = conversationLessons[1].BaiHocID,
                NoiDung = "'I would like to buy...' có nghĩa là:",
                PhuongAnA = "Tôi muốn mua",
                PhuongAnB = "Tôi không thích",
                PhuongAnC = "Tôi không mua",
                PhuongAnD = "Tôi đã mua",
                DapAnDung = "A"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = conversationLessons[1].BaiHocID,
                NoiDung = "Khi trả tiền tại cửa hàng, bạn nói:",
                PhuongAnA = "Can I pay?",
                PhuongAnB = "Here you are",
                PhuongAnC = "Thank you",
                PhuongAnD = "Hello",
                DapAnDung = "A"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = conversationLessons[1].BaiHocID,
                NoiDung = "'Do you have...?' dùng để:",
                PhuongAnA = "Hỏi giá",
                PhuongAnB = "Hỏi có hay không",
                PhuongAnC = "Mua hàng",
                PhuongAnD = "Cảm ơn",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = conversationLessons[1].BaiHocID,
                NoiDung = "'Receipt' có nghĩa là:",
                PhuongAnA = "Tiền",
                PhuongAnB = "Hóa đơn",
                PhuongAnC = "Cửa hàng",
                PhuongAnD = "Sản phẩm",
                DapAnDung = "B"
            }
        };

        // Câu hỏi cho bài 3: Hỏi đường
        var conv3Questions = new List<CauHoiTracNghiem>
        {
            new CauHoiTracNghiem
            {
                BaiHocID = conversationLessons[2].BaiHocID,
                NoiDung = "Khi muốn hỏi đường, bạn nói:",
                PhuongAnA = "Where is the...?",
                PhuongAnB = "What is the...?",
                PhuongAnC = "Who is the...?",
                PhuongAnD = "When is the...?",
                DapAnDung = "A"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = conversationLessons[2].BaiHocID,
                NoiDung = "'Turn left' có nghĩa là:",
                PhuongAnA = "Rẽ phải",
                PhuongAnB = "Rẽ trái",
                PhuongAnC = "Đi thẳng",
                PhuongAnD = "Quay lại",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = conversationLessons[2].BaiHocID,
                NoiDung = "'Go straight' có nghĩa là:",
                PhuongAnA = "Rẽ phải",
                PhuongAnB = "Rẽ trái",
                PhuongAnC = "Đi thẳng",
                PhuongAnD = "Quay lại",
                DapAnDung = "C"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = conversationLessons[2].BaiHocID,
                NoiDung = "Chọn câu đúng: 'Excuse me, can you tell me ___ to the station?'",
                PhuongAnA = "how",
                PhuongAnB = "where",
                PhuongAnC = "the way",
                PhuongAnD = "what",
                DapAnDung = "C"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = conversationLessons[2].BaiHocID,
                NoiDung = "'It's on the right' có nghĩa là:",
                PhuongAnA = "Ở bên trái",
                PhuongAnB = "Ở bên phải",
                PhuongAnC = "Ở giữa",
                PhuongAnD = "Ở phía sau",
                DapAnDung = "B"
            }
        };

        // Câu hỏi cho bài 4: Nhà hàng
        var conv4Questions = new List<CauHoiTracNghiem>
        {
            new CauHoiTracNghiem
            {
                BaiHocID = conversationLessons[3].BaiHocID,
                NoiDung = "Khi đặt bàn, bạn nói:",
                PhuongAnA = "I want to reserve a table",
                PhuongAnB = "I want food",
                PhuongAnC = "Give me table",
                PhuongAnD = "Table please",
                DapAnDung = "A"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = conversationLessons[3].BaiHocID,
                NoiDung = "'Menu' có nghĩa là:",
                PhuongAnA = "Thực đơn",
                PhuongAnB = "Bàn ăn",
                PhuongAnC = "Nhà hàng",
                PhuongAnD = "Món ăn",
                DapAnDung = "A"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = conversationLessons[3].BaiHocID,
                NoiDung = "Khi gọi món, bạn nói:",
                PhuongAnA = "I would like to order...",
                PhuongAnB = "I want...",
                PhuongAnC = "Give me...",
                PhuongAnD = "I need...",
                DapAnDung = "A"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = conversationLessons[3].BaiHocID,
                NoiDung = "'The bill, please' có nghĩa là:",
                PhuongAnA = "Xin mời ăn",
                PhuongAnB = "Cho tôi hóa đơn",
                PhuongAnC = "Cảm ơn",
                PhuongAnD = "Xin lỗi",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = conversationLessons[3].BaiHocID,
                NoiDung = "'Waiter' có nghĩa là:",
                PhuongAnA = "Khách hàng",
                PhuongAnB = "Bồi bàn",
                PhuongAnC = "Đầu bếp",
                PhuongAnD = "Chủ nhà hàng",
                DapAnDung = "B"
            }
        };

        // Câu hỏi cho bài 5: Điện thoại
        var conv5Questions = new List<CauHoiTracNghiem>
        {
            new CauHoiTracNghiem
            {
                BaiHocID = conversationLessons[4].BaiHocID,
                NoiDung = "Khi nghe điện thoại, bạn nói:",
                PhuongAnA = "Hello, who are you?",
                PhuongAnB = "Hello, this is...",
                PhuongAnC = "Hello, I am...",
                PhuongAnD = "Hello, my name...",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = conversationLessons[4].BaiHocID,
                NoiDung = "'Can I speak to...?' có nghĩa là:",
                PhuongAnA = "Tôi có thể nói chuyện với...",
                PhuongAnB = "Tôi không thể nói",
                PhuongAnC = "Xin lỗi",
                PhuongAnD = "Cảm ơn",
                DapAnDung = "A"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = conversationLessons[4].BaiHocID,
                NoiDung = "Khi muốn để lại tin nhắn:",
                PhuongAnA = "Can I leave a message?",
                PhuongAnB = "I want message",
                PhuongAnC = "Give me message",
                PhuongAnD = "Message please",
                DapAnDung = "A"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = conversationLessons[4].BaiHocID,
                NoiDung = "'Email address' có nghĩa là:",
                PhuongAnA = "Địa chỉ nhà",
                PhuongAnB = "Địa chỉ email",
                PhuongAnC = "Số điện thoại",
                PhuongAnD = "Tên",
                DapAnDung = "B"
            },
            new CauHoiTracNghiem
            {
                BaiHocID = conversationLessons[4].BaiHocID,
                NoiDung = "Khi kết thúc cuộc gọi:",
                PhuongAnA = "Goodbye",
                PhuongAnB = "Hello",
                PhuongAnC = "Thank you",
                PhuongAnD = "Please",
                DapAnDung = "A"
            }
        };

        // ===== LƯU TẤT CẢ CÂU HỎI =====
        var allQuestions = new List<CauHoiTracNghiem>();
        allQuestions.AddRange(grammar1Questions);
        allQuestions.AddRange(grammar2Questions);
        allQuestions.AddRange(grammar3Questions);
        allQuestions.AddRange(grammar4Questions);
        allQuestions.AddRange(grammar5Questions);
        allQuestions.AddRange(vocab1Questions);
        allQuestions.AddRange(vocab2Questions);
        allQuestions.AddRange(vocab3Questions);
        allQuestions.AddRange(vocab4Questions);
        allQuestions.AddRange(vocab5Questions);
        allQuestions.AddRange(conv1Questions);
        allQuestions.AddRange(conv2Questions);
        allQuestions.AddRange(conv3Questions);
        allQuestions.AddRange(conv4Questions);
        allQuestions.AddRange(conv5Questions);

        await context.CauHoiTracNghiems.AddRangeAsync(allQuestions);
        await context.SaveChangesAsync();
    }
}

