using LanguageApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LanguageApp.Api.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<LanguageAppDbContext>();

        if (!await context.Database.CanConnectAsync())
        {
            return;
        }

        if (!await context.KhoaHocs.AnyAsync())
        {
            var baiHoc1 = new BaiHoc { TenBaiHoc = "Danh từ & Động từ", ThuTu = 1 };
            var baiHoc2 = new BaiHoc { TenBaiHoc = "Thì Hiện Tại Đơn", ThuTu = 2 };
            var baiHoc3 = new BaiHoc { TenBaiHoc = "Thì Hiện Tại Tiếp Diễn", ThuTu = 3 };
            var baiHoc4 = new BaiHoc { TenBaiHoc = "Chào hỏi NPC", ThuTu = 1 };
            var baiHoc5 = new BaiHoc { TenBaiHoc = "Mua sắm tại Cửa hàng", ThuTu = 2 };
            var baiHoc6 = new BaiHoc { TenBaiHoc = "Xin chỉ đường", ThuTu = 3 };

            var starterCourses = new[]
            {
                new KhoaHoc
                {
                    TenKhoaHoc = "Thám Hiểm Ngữ Pháp",
                    MoTa = "Khởi động hành trình với các cấu trúc câu và thì cơ bản.",
                    DoKho = 1,
                    BaiHocs = { baiHoc1, baiHoc2, baiHoc3 }
                },
                new KhoaHoc
                {
                    TenKhoaHoc = "Hội Thoại Thị Trấn",
                    MoTa = "Thực hành giao tiếp tại Quảng trường Thị trấn Học Thuật.",
                    DoKho = 2,
                    BaiHocs = { baiHoc4, baiHoc5, baiHoc6 }
                }
            };

            await context.KhoaHocs.AddRangeAsync(starterCourses);
            await context.SaveChangesAsync();

            // Seed câu hỏi cho bài học 1
            var cauHoi1_1 = new CauHoiTracNghiem
            {
                BaiHocID = baiHoc1.BaiHocID,
                NoiDung = "Từ nào sau đây là danh từ?",
                PhuongAnA = "run",
                PhuongAnB = "book",
                PhuongAnC = "quickly",
                PhuongAnD = "beautiful",
                DapAnDung = "B"
            };
            var cauHoi1_2 = new CauHoiTracNghiem
            {
                BaiHocID = baiHoc1.BaiHocID,
                NoiDung = "Từ nào sau đây là động từ?",
                PhuongAnA = "table",
                PhuongAnB = "happy",
                PhuongAnC = "jump",
                PhuongAnD = "red",
                DapAnDung = "C"
            };
            var cauHoi1_3 = new CauHoiTracNghiem
            {
                BaiHocID = baiHoc1.BaiHocID,
                NoiDung = "Chọn câu đúng:",
                PhuongAnA = "I book read",
                PhuongAnB = "I read book",
                PhuongAnC = "Read I book",
                PhuongAnD = "Book read I",
                DapAnDung = "B"
            };

            // Seed câu hỏi cho bài học 2
            var cauHoi2_1 = new CauHoiTracNghiem
            {
                BaiHocID = baiHoc2.BaiHocID,
                NoiDung = "Chọn dạng đúng của động từ: I ___ to school every day.",
                PhuongAnA = "go",
                PhuongAnB = "goes",
                PhuongAnC = "going",
                PhuongAnD = "went",
                DapAnDung = "A"
            };
            var cauHoi2_2 = new CauHoiTracNghiem
            {
                BaiHocID = baiHoc2.BaiHocID,
                NoiDung = "She ___ her homework every evening.",
                PhuongAnA = "do",
                PhuongAnB = "does",
                PhuongAnC = "did",
                PhuongAnD = "doing",
                DapAnDung = "B"
            };

            // Seed câu hỏi cho bài học 4 (Chào hỏi)
            var cauHoi4_1 = new CauHoiTracNghiem
            {
                BaiHocID = baiHoc4.BaiHocID,
                NoiDung = "Cách chào hỏi lịch sự khi gặp người lạ:",
                PhuongAnA = "Hey!",
                PhuongAnB = "Hello, nice to meet you",
                PhuongAnC = "What's up?",
                PhuongAnD = "Yo!",
                DapAnDung = "B"
            };
            var cauHoi4_2 = new CauHoiTracNghiem
            {
                BaiHocID = baiHoc4.BaiHocID,
                NoiDung = "Khi ai đó hỏi 'How are you?', bạn nên trả lời:",
                PhuongAnA = "I'm fine, thank you",
                PhuongAnB = "Nothing",
                PhuongAnC = "Go away",
                PhuongAnD = "I don't know",
                DapAnDung = "A"
            };

            await context.CauHoiTracNghiems.AddRangeAsync(
                cauHoi1_1, cauHoi1_2, cauHoi1_3,
                cauHoi2_1, cauHoi2_2,
                cauHoi4_1, cauHoi4_2
            );
        }

        if (!await context.PhanThuongs.AnyAsync())
        {
            var rewards = new[]
            {
                new PhanThuong
                {
                    TenPhanThuong = "Áo choàng Học Giả",
                    LoaiPhanThuong = "Cosmetic",
                    MoTa = "Trang phục giúp bạn nổi bật tại Quảng trường.",
                    Gia = 120,
                    AssetURL = "https://example.com/assets/robe.png"
                },
                new PhanThuong
                {
                    TenPhanThuong = "Sổ tay Ghi nhớ",
                    LoaiPhanThuong = "Utility",
                    MoTa = "Giúp gia tăng khả năng ghi nhớ khi học bài.",
                    Gia = 90,
                    AssetURL = "https://example.com/assets/notebook.png"
                },
                new PhanThuong
                {
                    TenPhanThuong = "Vé Arcade Xịn",
                    LoaiPhanThuong = "Utility",
                    MoTa = "Nhận thêm 10% 💎 trong lượt chơi tiếp theo.",
                    Gia = 150,
                    AssetURL = "https://example.com/assets/ticket.png"
                }
            };

            await context.PhanThuongs.AddRangeAsync(rewards);
        }

        if (!await context.PhuHuynhs.AnyAsync())
        {
            var firstStudent = await context.HocSinhs.FirstOrDefaultAsync();
            if (firstStudent is not null)
            {
                var parent = new PhuHuynh
                {
                    TenPhuHuynh = "Phụ huynh mẫu",
                    Email = "parent@example.com",
                    ZaloID = "zalo-demo"
                };

                parent.HocSinhs.Add(firstStudent);
                await context.PhuHuynhs.AddAsync(parent);
            }
        }

        await context.SaveChangesAsync();
    }
}

