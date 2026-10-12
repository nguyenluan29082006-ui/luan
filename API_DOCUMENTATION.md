# HỆ THỐNG RESTful WEB API - DỰ ÁN WEB ĐỌC TRUYỆN (ASP.NET Core 8)

Hệ thống Backend REST API được xây dựng theo chuẩn kiến trúc dịch vụ phân tầng hiện đại trên nền tảng **C# / ASP.NET Core 8**, hỗ trợ xác thực **JWT Bearer Token**, cơ sở dữ liệu **SQLite (EF Core 8)**, và tài liệu hóa tự động bằng **Swagger (OpenAPI)**.

---

## 1. THÔNG TIN KHỞI CHẠY & SWAGGER UI

| Thông tin | Giá trị |
| :--- | :--- |
| **Backend API Host** | `http://localhost:5000` |
| **Giao diện Swagger UI trực quan** | [http://localhost:5000/swagger](http://localhost:5000/swagger) |
| **OpenAPI Spec (JSON)** | [http://localhost:5000/swagger/v1/swagger.json](http://localhost:5000/swagger/v1/swagger.json) |
| **File kiểm thử 1-click (REST Client)** | [`c:\cnpmnc\ServerCSharp.http`](file:///c:/cnpmnc/ServerCSharp.http) |

### Khởi chạy Backend qua Terminal:
```bash
dotnet run --project Backend/WebDocTruyen.Backend.csproj --urls "http://localhost:5000"
```

---

## 2. TÀI KHOẢN MẪU DÙNG THỬ (DEMO ACCOUNTS)

| Vai trò (Role) | Email | Mật khẩu | Quyền hạn tiêu biểu |
| :--- | :--- | :--- | :--- |
| **Quản trị viên (Admin)** | `admin@doctruyen.vn` | `admin123` | Toàn quyền hệ thống, duyệt truyện, quản lý thành viên, xử lý vi phạm |
| **Tác giả (Author)** | `tacgia@doctruyen.vn` | `author123` | Đăng tác phẩm mới, xuất bản chương, cập nhật truyện |
| **Độc giả (Reader)** | `docgia@doctruyen.vn` | `reader123` | Đọc truyện, lưu tủ sách yêu thích, lưu tiến trình đọc, bình luận, báo cáo |

> **Cách lấy Token**: Gửi request `POST /api/auth/login` với email và password tương ứng. Copy chuỗi `token` trong kết quả trả về và điền vào nút **Authorize** (`Bearer {token}`) trên Swagger UI hoặc header `Authorization: Bearer {token}`.

---

## 3. DANH MỤC CÁC MODULE API ĐÃ TRIỂN KHAI

Hệ thống bao gồm **10 Module API toàn diện** với đầy đủ các nghiệp vụ:

### 3.1. Xác thực & Phân quyền (Auth - `/api/auth`)
* `POST /api/auth/register`: Đăng ký tài khoản độc giả hoặc tác giả mới.
* `POST /api/auth/login`: Đăng nhập, trả về JWT Token và thông tin User.
* `GET /api/auth/me`: Lấy thông tin tài khoản đang đăng nhập (yêu cầu Bearer Token).
* `POST /api/auth/become-author`: Nâng cấp tài khoản người dùng hiện tại lên vai trò Tác giả.
* `POST /api/auth/forgot-password`: Đặt lại mật khẩu tài khoản.

### 3.2. Quản lý Truyện (Stories - `/api/stories`)
* `GET /api/stories`: Lấy danh sách truyện có phân trang (`page`, `limit`), lọc theo thể loại (`genre`), loại truyện (`type`), tìm kiếm (`search`), sắp xếp (`sort`: `views`, `rating`, `latest`, `chapters`).
* `GET /api/stories/featured`: Lấy danh sách truyện tiêu biểu / nổi bật nhất.
* `GET /api/stories/{idOrSlug}`: Xem chi tiết truyện theo ID hoặc Slug (kèm danh sách toàn bộ các chương).
* `POST /api/stories`: Tác giả tạo truyện mới (Yêu cầu quyền `author` hoặc `admin`).
* `PUT /api/stories/{id}`: Tác giả chỉnh sửa truyện của mình (tên, giới thiệu, ảnh bìa, trạng thái).
* `DELETE /api/stories/{id}`: Xóa truyện (Tác giả sở hữu hoặc Quản trị viên).

### 3.3. Quản lý Chương & Đọc truyện (Chapters - `/api/chapters`)
* `GET /api/chapters/{id}`: Đọc chi tiết nội dung chương truyện (tự động tăng view, cung cấp ID chương trước `prevChapterId`, ID chương sau `nextChapterId` và danh sách mục lục chương).
* `POST /api/stories/{storyId}/chapters`: Tác giả thêm chương mới vào truyện.

### 3.4. Thể loại truyện (Genres - `/api/genres`)
* `GET /api/genres`: Lấy danh sách toàn bộ thể loại (kèm số lượng truyện đang có trong mỗi thể loại).
* `GET /api/genres/{id}`: Xem chi tiết thể loại và danh sách các tác phẩm thuộc thể loại đó.
* `POST /api/genres`: Quản trị viên thêm thể loại mới (Admin).
* `DELETE /api/genres/{id}`: Quản trị viên xóa thể loại (Admin).

### 3.5. Tương tác: Tủ sách & Lịch sử đọc (Interactions - `/api/favorites`, `/api/history`)
* `POST /api/favorites/toggle/{storyId}`: Thêm hoặc bỏ truyện khỏi tủ truyện yêu thích của độc giả (1-click Toggle Bookmark).
* `GET /api/favorites`: Lấy danh sách tủ truyện yêu thích của tài khoản đang đăng nhập.
* `POST /api/history`: Lưu hoặc cập nhật tiến trình đọc (đang đọc tới chương nào của truyện).
* `GET /api/history`: Lấy toàn bộ lịch sử đọc truyện của độc giả kèm ngày giờ đọc gần nhất.

### 3.6. Cộng đồng: Bình luận & Báo cáo vi phạm (Community - `/api/comments`, `/api/reports`)
* `GET /api/comments?storyId={id}`: Lấy danh sách các bình luận của một truyện hoặc một chương.
* `POST /api/comments`: Độc giả gửi bình luận (hỗ trợ bình luận toàn truyện hoặc trong chương cụ thể).
* `DELETE /api/comments/{id}`: Xóa bình luận (Người tạo bình luận hoặc Admin).
* `POST /api/reports`: Gửi báo cáo vi phạm nội dung (báo cáo truyện, chương hoặc bình luận phản cảm).

### 3.7. Hệ thống Thông báo (Notifications - `/api/notifications`)
* `GET /api/notifications`: Lấy danh sách thông báo gửi đến người dùng (thông báo chương mới, duyệt truyện, hệ thống).
* `GET /api/notifications/unread-count`: Đếm nhanh số lượng thông báo chưa đọc (dùng hiển thị Badge đỏ trên UI).
* `PUT /api/notifications/{id}/read`: Đánh dấu 1 thông báo là đã đọc.
* `PUT /api/notifications/read-all`: Đánh dấu toàn bộ thông báo là đã đọc.
* `POST /api/notifications`: Admin gửi thông báo mới đến một người dùng cụ thể.

### 3.8. Hồ sơ Người dùng (Users & Profile - `/api/users`)
* `GET /api/users/profile`: Xem hồ sơ cá nhân đầy đủ kèm các chỉ số thống kê (tổng truyện đã đăng, số truyện yêu thích, số chương đã đọc, tổng số bình luận).
* `PUT /api/users/profile`: Chỉnh sửa họ tên, tiểu sử (bio), ảnh đại diện (avatar).
* `PUT /api/users/change-password`: Đổi mật khẩu tài khoản (xác minh mật khẩu cũ).
* `GET /api/users/{id}`: Xem trang hồ sơ công khai của một tác giả / độc giả và danh sách truyện đã công khai.

### 3.9. Thống kê & Bảng xếp hạng (Stats & Leaderboard - `/api/stats`)
* `GET /api/stats/dashboard`: Bảng tổng quan số liệu hệ thống (tổng user, tổng truyện, tổng chương, tổng lượt xem, tổng bình luận, số truyện chờ duyệt, số báo cáo chờ giải quyết).
* `GET /api/stats/top-views`: Bảng xếp hạng Top truyện có lượt đọc nhiều nhất (Top Views).
* `GET /api/stats/top-favorites`: Bảng xếp hạng Top truyện được đưa vào tủ sách nhiều nhất (Top Bookmarks).
* `GET /api/stats/top-authors`: Bảng xếp hạng các tác giả nổi bật nhất dựa trên tổng số lượt xem và số tác phẩm.

### 3.10. Quản trị hệ thống (Admin - `/api/admin`)
* `GET /api/admin/stats`: Báo cáo chỉ số quản trị tổng thể.
* `GET /api/admin/stories/pending`: Danh sách các truyện tác giả vừa đăng đang chờ kiểm duyệt.
* `PUT /api/admin/stories/{id}/review`: Phê duyệt hoặc từ chối xuất bản truyện (kèm lý do nếu từ chối).
* `GET /api/admin/reports`: Danh sách báo cáo vi phạm cần xử lý.
* `PUT /api/admin/reports/{id}/handle`: Xử lý báo cáo (đánh dấu giải quyết `resolved`, gỡ bỏ đối tượng vi phạm hoặc bỏ qua `dismissed`).
* `GET /api/admin/users`: Danh sách toàn bộ tài khoản người dùng trong hệ thống.
* `PUT /api/admin/users/{id}/role`: Cập nhật phân quyền người dùng (`admin`, `author`, `reader`).
* `PUT /api/admin/users/{id}/toggle-ban`: Khóa hoặc mở khóa tài khoản vi phạm.

---

## 4. HƯỚNG DẪN TEST API 1-CLICK TRÊN VS CODE / ANTIGRAVITY

1. Cài đặt Extension **REST Client** trên VS Code / Cursor / Antigravity IDE (nếu chưa có).
2. Mở file **`ServerCSharp.http`** tại thư mục gốc dự án.
3. Bấm vào chữ **`Send Request`** ngay phía trên mỗi câu lệnh API. Token sẽ được tự động trích xuất và gán vào các request tiếp theo!
