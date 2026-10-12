using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using BCrypt.Net;
using ServerCSharp.Models;

namespace ServerCSharp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return; // Database has been seeded
            }

            // 1. Users
            var admin = new User
            {
                Name = "Quản Trị Viên Hệ Thống",
                Email = "admin@doctruyen.vn",
                Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = "admin",
                Status = "active",
                Avatar = "https://images.unsplash.com/photo-1534528741775-53994a69daeb?w=150&auto=format&fit=crop&q=80",
                Bio = "Quản trị viên trưởng phụ trách nội dung và kiểm duyệt hệ thống."
            };

            var author1 = new User
            {
                Name = "Thiên Tằm Thổ Đậu",
                Email = "tacgia@doctruyen.vn",
                Password = BCrypt.Net.BCrypt.HashPassword("author123"),
                Role = "author",
                Status = "active",
                Avatar = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=150&auto=format&fit=crop&q=80",
                Bio = "Tác giả chuyên sáng tác các bộ truyện Tiên Hiệp, Huyền Huyễn kỳ ảo."
            };

            var author2 = new User
            {
                Name = "Mặc Hương Đồng Khứu",
                Email = "tacgia2@doctruyen.vn",
                Password = BCrypt.Net.BCrypt.HashPassword("author123"),
                Role = "author",
                Status = "active",
                Avatar = "https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=150&auto=format&fit=crop&q=80",
                Bio = "Cây bút truyện phiêu lưu huyền ảo và tình cảm sâu sắc."
            };

            var reader1 = new User
            {
                Name = "Nguyễn Văn Đọc",
                Email = "docgia@doctruyen.vn",
                Password = BCrypt.Net.BCrypt.HashPassword("reader123"),
                Role = "reader",
                Status = "active",
                Avatar = "https://images.unsplash.com/photo-1535713875002-d1d0cf377fde?w=150&auto=format&fit=crop&q=80",
                Bio = "Mọt sách nghiện truyện chữ và truyện tranh."
            };

            var reader2 = new User
            {
                Name = "Trần Thị Mơ",
                Email = "reader2@doctruyen.vn",
                Password = BCrypt.Net.BCrypt.HashPassword("reader123"),
                Role = "reader",
                Status = "active",
                Avatar = "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=150&auto=format&fit=crop&q=80",
                Bio = "Độc giả yêu thích thể loại xuyên không, hài hước."
            };

            context.Users.AddRange(admin, author1, author2, reader1, reader2);
            context.SaveChanges();

            // 2. Genres
            var gTienHiep = new Genre { Name = "Tiên Hiệp", Slug = "tien-hiep", Description = "Tu tiên luyện đạo, tầm bảo đoạt mệnh phi thăng", Icon = "✨" };
            var gHuyenHuyen = new Genre { Name = "Huyền Huyễn", Slug = "huyen-huanh", Description = "Thế giới huyền bí, ma pháp võ thuật kỳ ảo", Icon = "🔮" };
            var gKiemHiep = new Genre { Name = "Kiếm Hiệp", Slug = "kiem-hiep", Description = "Ân oán giang hồ võ lâm hiệp khách", Icon = "⚔️" };
            var gTrungSinh = new Genre { Name = "Trùng Sinh", Slug = "trung-sinh", Description = "Sống lại kiếp khác để nghịch thiên cải mệnh", Icon = "🔄" };
            var gDoThi = new Genre { Name = "Đô Thị", Slug = "do-thi", Description = "Bối cảnh xã hội hiện đại nhiều biến cố", Icon = "🏙️" };
            var gHanhDong = new Genre { Name = "Hành Động", Slug = "hanh-dong", Description = "Những pha giao tranh kịch tính gay cấn", Icon = "💥" };
            var gNgonTinh = new Genre { Name = "Ngôn Tình", Slug = "ngon-tinh", Description = "Câu chuyện tình cảm lãng mạn ấm áp", Icon = "❤️" };
            var gHaiHuoc = new Genre { Name = "Hài Hước", Slug = "hai-huoc", Description = "Tình tiết gây cười giải trí nhẹ nhàng", Icon = "😂" };

            context.Genres.AddRange(gTienHiep, gHuyenHuyen, gKiemHiep, gTrungSinh, gDoThi, gHanhDong, gNgonTinh, gHaiHuoc);
            context.SaveChanges();

            // 3. Stories
            var story1 = new Story
            {
                Title = "Đấu Phá Thương Khung",
                Slug = "dau-pha-thuong-khung",
                AuthorId = author1.Id,
                AuthorName = author1.Name,
                Description = "Tại Đấu Khí Đại Lục, không có ma pháp diễm lệ, chỉ có đấu khí đã phát triển đến đỉnh phong! Tiêu Viêm - một thiên tài từng vang danh gia tộc nhưng bỗng chốc mất đi toàn bộ tu vi, trở thành phế vật bị người người khinh miệt. Trong lúc tuyệt vọng nhất, chiếc giới chỉ do người mẹ để lại đã thức tỉnh một linh hồn thần bí, mở ra con đường tu luyện đầy kỳ tích!",
                CoverUrl = "https://images.unsplash.com/photo-1518709268805-4e9042af9f23?w=500&auto=format&fit=crop&q=80",
                Type = "novel",
                Status = "completed",
                ApprovalStatus = "approved",
                Views = 154200
            };

            var story2 = new Story
            {
                Title = "Ta Là Tà Đế: Vạn Giới Du Hành",
                Slug = "ta-la-ta-de-van-gioi-du-hanh",
                AuthorId = author1.Id,
                AuthorName = author1.Name,
                Description = "Tạ Diệm - thanh niên thế kỷ 21 xuyên việt trở thành mỹ nam tử tuyệt sắc của Ma Đạo Yêu Nữ Tông Môn. Vì không muốn trở thành lô đỉnh bị hút cạn tinh hoa, hắn thức tỉnh hệ thống mô phỏng chư thiên vạn giới, bắt đầu hành trình thu hoạch điểm nguồn gốc và nghịch thiên xưng đế!",
                CoverUrl = "https://images.unsplash.com/photo-1578632767115-351597cf2477?w=500&auto=format&fit=crop&q=80",
                Type = "comic",
                Status = "ongoing",
                ApprovalStatus = "approved",
                Views = 98300
            };

            var story3 = new Story
            {
                Title = "Toàn Chức Cao Thủ",
                Slug = "toan-chuc-cao-thu",
                AuthorId = author2.Id,
                AuthorName = author2.Name,
                Description = "Diệp Tu, tuyển thủ đứng đầu tựa game Vinh Quang, bất ngờ bị câu lạc bộ ép buộc thoái lui và phải giao lại tài khoản đẳng cấp thần thánh Nhất Diệp Chi Thu. Rời khỏi thế giới tuyển thủ, hắn vào làm nhân viên ca đêm tại một quán net nhỏ. Khi khu thứ 10 của game khai mở, Diệp Tu cầm chiếc ô Thiên Cơ Tán bước vào chinh phục lại đỉnh cao hoàng kim!",
                CoverUrl = "https://images.unsplash.com/photo-1542751371-adc38448a05e?w=500&auto=format&fit=crop&q=80",
                Type = "novel",
                Status = "ongoing",
                ApprovalStatus = "approved",
                Views = 82150
            };

            var story4 = new Story
            {
                Title = "Thiên Đạo Thư Viện",
                Slug = "thien-dao-thu-vien",
                AuthorId = author2.Id,
                AuthorName = author2.Name,
                Description = "Trương Huyền xuyên việt sang dị giới làm một gã giáo viên mạt hạng sắp bị đuổi học viện. Nhưng trong đầu hắn xuất hiện một toà Thiên Đạo Thư Viện thần bí, chỉ cần nhìn bất cứ thứ gì hay ai đó vận công là lập tức thấy rõ thiếu sót và khiếm khuyết. Kể từ đây, một huyền thoại vạn thế sư biểu ra đời!",
                CoverUrl = "https://images.unsplash.com/photo-1532012164546-f432f2e37b73?w=500&auto=format&fit=crop&q=80",
                Type = "novel",
                Status = "ongoing",
                ApprovalStatus = "approved",
                Views = 64200
            };

            var story5 = new Story
            {
                Title = "Nguyên Tôn: Khởi Đầu Nghịch Cảnh",
                Slug = "nguyen-ton-khoi-dau-nghich-canh",
                AuthorId = author1.Id,
                AuthorName = author1.Name,
                Description = "Thiếu niên Chu Nguyên sinh ra mang Thánh Long khí vận, nhưng thuở ấu thơ đã bị Đại Võ cướp đoạt, trở thành kẻ có thân thể yếu nhược, kinh mạch bế tắc. Không chịu khuất phục trước số phận cay đắng, Chu Nguyên dấn thân vào con đường nghịch thiên đoạt lại vinh quang.",
                CoverUrl = "https://images.unsplash.com/photo-1514533450685-4493e01d1fdc?w=500&auto=format&fit=crop&q=80",
                Type = "novel",
                Status = "ongoing",
                ApprovalStatus = "pending", // Pending approval for Admin
                Views = 0
            };

            var story6 = new Story
            {
                Title = "Huyết Sát Ma Tôn",
                Slug = "huyet-sat-ma-ton",
                AuthorId = author1.Id,
                AuthorName = author1.Name,
                Description = "Tu ma tàn sát vạn giới, lấy máu chứng đạo.",
                CoverUrl = "https://images.unsplash.com/photo-1509198397868-475647b2a1e5?w=500&auto=format&fit=crop&q=80",
                Type = "novel",
                Status = "ongoing",
                ApprovalStatus = "rejected",
                RejectionReason = "Nội dung chứa miêu tả bạo lực quá mức không phù hợp tiêu chuẩn kiểm duyệt. Tác giả vui lòng sửa đổi văn phong chương mở đầu.",
                Views = 0
            };

            context.Stories.AddRange(story1, story2, story3, story4, story5, story6);
            context.SaveChanges();

            // Story Genres
            context.StoryGenres.AddRange(
                new StoryGenre { StoryId = story1.Id, GenreId = gTienHiep.Id },
                new StoryGenre { StoryId = story1.Id, GenreId = gHuyenHuyen.Id },
                new StoryGenre { StoryId = story1.Id, GenreId = gHanhDong.Id },
                new StoryGenre { StoryId = story2.Id, GenreId = gTrungSinh.Id },
                new StoryGenre { StoryId = story2.Id, GenreId = gHuyenHuyen.Id },
                new StoryGenre { StoryId = story2.Id, GenreId = gHaiHuoc.Id },
                new StoryGenre { StoryId = story3.Id, GenreId = gDoThi.Id },
                new StoryGenre { StoryId = story3.Id, GenreId = gHanhDong.Id },
                new StoryGenre { StoryId = story4.Id, GenreId = gHuyenHuyen.Id },
                new StoryGenre { StoryId = story4.Id, GenreId = gHaiHuoc.Id },
                new StoryGenre { StoryId = story5.Id, GenreId = gTienHiep.Id },
                new StoryGenre { StoryId = story6.Id, GenreId = gTienHiep.Id }
            );
            context.SaveChanges();

            // 4. Chapters
            var chap1_s1 = new Chapter
            {
                StoryId = story1.Id,
                ChapterNumber = 1,
                Title = "Chương 1: Phế vật Tiêu gia",
                Content = @"“Đấu chi khí, tam đoạn!”
Nhìn năm chữ to lớn sáng rực nhưng đầy chói mắt trên ma thạch bia, thiếu niên mặt không chút biểu cảm, khóe môi khẽ cong lên một nét tự giễu, hai bàn tay nắm chặt giấu trong tay áo run rẩy nhẹ.

“Tiêu Viêm, Đấu chi khí: Tam đoạn! Cấp bậc: Cấp thấp!”

Bên cạnh ma thạch bia, một gã tráng hán trung niên nhìn lướt qua bảng kết quả, lạnh lùng xướng to lên trước toàn thể tộc nhân Tiêu gia.

Lời vừa dứt, quảng trường to lớn lập tức nổi lên một trận xôn xao to nhỏ. Vô số ánh mắt châm biếm, tiếc nuối và cười nhạo đều đồng loạt phóng về phía thân ảnh gầy gò của thiếu niên.

“Ba đoạn? Chậc chậc, quả nhiên vẫn đứng yên tại chỗ! Vị thiên tài lừng danh một thời của Ô Thản Thành chúng ta xem ra thực sự đã thành đồ bỏ đi rồi.”
“Ai, nếu không phải tộc trưởng là cha hắn, loại phế vật này sớm đã bị trục xuất khỏi gia tộc rồi, làm sao còn tư cách chiếm dụng linh dược tu luyện?”
“Năm xưa lúc hắn mười một tuổi đột phá Đấu Giả, cả gia tộc đều sùng bái hắn như thần minh, nào ngờ ba năm sau lại thụt lùi đến thê thảm mức này...”

Nghe những lời bàn tán cay nghiệt xung quanh, Tiêu Viêm hít một hơi thật sâu. Hắn không nói một lời, xoay người chậm rãi bước xuống đài cao. Trên ngón tay hắn, một chiếc nhẫn màu đen cổ xưa bình lặng nằm đó, phản chiếu ánh sáng lấp lánh như đang thức tỉnh..."
            };

            var chap2_s1 = new Chapter
            {
                StoryId = story1.Id,
                ChapterNumber = 2,
                Title = "Chương 2: Dược Lão Thức Tỉnh",
                Content = @"Đêm khuya thanh vắng, trên đỉnh ngọn đồi phía sau Tiêu gia.

Tiêu Viêm ngồi xếp bằng trên một tảng đá lớn, ngước nhìn lên bầu trời đầy sao. Đêm nay, gió đêm mang theo từng cơn lạnh buốt thổi qua vạt áo hắn.

“Ba năm rồi... Rốt cuộc vì sao?” Hắn siết chặt nắm tay, đập mạnh xuống phiến đá.

Bỗng nhiên, từ chiếc nhẫn đen trên ngón tay hắn phát ra một luồng sáng nhu hòa màu lam nhạt. Một làn khói trắng nhẹ nhàng bốc lên, ngưng tụ giữa không trung thành một thân ảnh lão giả râu tóc bạc phơ, ánh mắt tinh anh nhìn Tiêu Viêm đầy vẻ thú vị.

“Ha ha, tiểu tử, ba năm qua nhờ có đấu khí thuần khiết của ngươi mà lão phu mới có thể khôi phục một tia linh hồn đấy!”

Tiêu Viêm giật nảy mình, lập tức bật dậy thủ thế cảnh giác: “Ngươi là ai? Tại sao lại ở trong giới chỉ của mẫu thân ta? Ngươi vừa nói đấu khí của ta biến mất là do ngươi hút lấy?”

Lão giả vuốt râu mỉm cười: “Tiểu tử chớ nóng vội. Lão phu tự xưng Dược Lão. Ngươi mất đi ba năm đấu khí, nhưng đổi lại lão phu sẽ truyền thụ cho ngươi thuật Luyện Dược vô thượng cùng công pháp đủ để ngươi giẫm đạp toàn bộ thiên tài Đấu Khí Đại Lục dưới chân, ngươi thấy vụ làm ăn này thế nào?”"
            };

            var chap3_s1 = new Chapter
            {
                StoryId = story1.Id,
                ChapterNumber = 3,
                Title = "Chương 3: Hôn Ước Nạp Lan",
                Content = @"Sáng hôm sau, Tiêu gia đại sảnh náo nhiệt lạ thường.

Khách quý đến từ Vân Lam Tông - tông môn thế lực số một Gia Mã Đế Quốc đã đích thân đến viếng thăm. Đi đầu là một thiếu nữ kiêu kỳ mặc trường bào nguyệt bạch, dung nhan thanh tú diễm lệ, bên hông đeo trường kiếm thanh phong. Đó chính là Nạp Lan Yên Nhiên.

“Tiêu thúc thúc, lần này Yên Nhiên mạo muội đến đây là muốn thương lượng chuyện từ hôn cùng Tiêu Viêm công tử!”

Lời nói vang lên dõng dạc giữa đại sảnh khiến toàn thể trưởng lão Tiêu gia biến sắc. Tiêu Chiến - tộc trưởng Tiêu gia, sắc mặt tái xanh, chiếc chén trà trong tay vỡ nát thành từng mảnh bột phấn vụn.

Đúng lúc này, Tiêu Viêm hiên ngang bước vào sảnh, ánh mắt rực sáng kiên định: “Nạp Lan Yên Nhiên, hôm nay không phải ngươi từ hôn ta, mà là Tiêu Viêm ta bỏ ngươi! Ba năm sau, ta sẽ đích thân lên Vân Lam Tông, cùng ngươi giải quyết mối ân oán này! Ba mươi năm Hà Đông, ba mươi năm Hà Tây, đừng khinh thiếu niên nghèo!”"
            };

            var comicImages = JsonSerializer.Serialize(new[]
            {
                "https://images.unsplash.com/photo-1534447677768-be436bb09401?w=800&auto=format&fit=crop&q=80",
                "https://images.unsplash.com/photo-1563089145-599997674d42?w=800&auto=format&fit=crop&q=80",
                "https://images.unsplash.com/photo-1607604276583-eef5d076aa5f?w=800&auto=format&fit=crop&q=80"
            });

            var chap1_s2 = new Chapter
            {
                StoryId = story2.Id,
                ChapterNumber = 1,
                Title = "Chapter 1: Xuyên Không Vào Ma Môn",
                Content = comicImages
            };

            var chap1_s5 = new Chapter
            {
                StoryId = story5.Id,
                ChapterNumber = 1,
                Title = "Chương 1: Khí vận Thánh Long bị đoạt",
                Content = "Thiếu niên Chu Nguyên quỳ gối trước tổ miếu Đại Chu vương triều, nhìn lên trời cao mây đen vần vũ. Từng luồng khí tức ngột ngạt bao phủ lấy toàn bộ hoàng thành. Hôm nay, thiếu niên quyết tâm phá bỏ xiềng xích vận mệnh!"
            };

            context.Chapters.AddRange(chap1_s1, chap2_s1, chap3_s1, chap1_s2, chap1_s5);
            context.SaveChanges();

            // 5. Favorites & History
            context.Favorites.AddRange(
                new Favorite { UserId = reader1.Id, StoryId = story1.Id },
                new Favorite { UserId = reader1.Id, StoryId = story2.Id }
            );

            context.ReadingHistories.Add(
                new ReadingHistory { UserId = reader1.Id, StoryId = story1.Id, ChapterId = chap2_s1.Id }
            );
            context.SaveChanges();

            // 6. Comments
            var c1 = new Comment
            {
                UserId = reader1.Id,
                StoryId = story1.Id,
                ChapterId = chap1_s1.Id,
                Content = "Đoạn 'Ba mươi năm Hà Đông, ba mươi năm Hà Tây' đọc lại bao nhiêu lần vẫn nổi da gà!"
            };

            var offensiveComment = new Comment
            {
                UserId = reader2.Id,
                StoryId = story1.Id,
                ChapterId = chap1_s1.Id,
                Content = "Tác giả viết như dở hơi, nhân vật chính vô dụng xem bực cả mình, vote 1 sao tẩy chay!"
            };

            context.Comments.AddRange(c1, offensiveComment);
            context.SaveChanges();

            // 7. Reports
            context.Reports.AddRange(
                new Report
                {
                    UserId = reader1.Id,
                    TargetType = "comment",
                    TargetId = offensiveComment.Id,
                    Reason = "Ngôn từ xúc phạm, kích động hiềm khích và công kích cá nhân",
                    Details = "Bình luận chứa thái độ thù hằn vô căn cứ, đề nghị quản trị viên ẩn hoặc gỡ bỏ.",
                    Status = "pending"
                },
                new Report
                {
                    UserId = reader2.Id,
                    TargetType = "chapter",
                    TargetId = chap2_s1.Id,
                    Reason = "Nội dung bị lỗi hiển thị",
                    Details = "Đoạn cuối chương 2 có vẻ bị ngắt giữa chừng, đề nghị tác giả cập nhật thêm.",
                    Status = "pending"
                }
            );
            context.SaveChanges();
        }
    }
}
