# TÀI LIỆU ĐẶC TẢ YÊU CẦU PHẦN MỀM (SRS)
## HỆ THỐNG WEBSITE ĐỌC TRUYỆN TRỰC TUYẾN (WEBOCTRUYEN)
**Môn học:** Công Nghệ Phần Mềm Nâng Cao (CNPMNC)  
**Nhóm thực hiện:** Nhóm B07 - 5linhdanhthue  
**Dự án:** WebDocTruyen (Project Key: ITUQ)  
**Phiên bản:** 1.0  
**Ngày cập nhật:** 12/10/2026  

---

## 1. TỔNG QUAN TÀI LIỆU

### 1.1. Mục Đích
Tài liệu SRS (Software Requirements Specification) này mô tả toàn diện các yêu cầu chức năng (Functional Requirements), yêu cầu phi chức năng (Non-Functional Requirements), đối tượng người dùng, thiết kế cơ sở dữ liệu và kiến trúc công nghệ của **Hệ thống Web Đọc Truyện Trực Tuyến (WebDocTruyen)**. 

Tài liệu đóng vai trò làm chuẩn mực kỹ thuật để:
- Trợ giáo và Giảng viên đánh giá tiến độ, kiến trúc và mức độ hoàn thiện của hệ thống.
- Đội ngũ phát triển (Nhóm sinh viên) thống nhất nghiệp vụ giữa Backend (API/Database) và Frontend (MVC UI).
- Căn cứ đối chiếu với Sprint Backlog trên hệ thống quản lý Jira.

### 1.2. Phạm Vi Hệ Thống
Hệ thống được phát triển thuần trên nền tảng **C# và .NET 8**, phân tách rõ ràng thành hai phân hệ độc lập:

| Thành phần | Công nghệ | Cổng | Đối tượng sử dụng | Chức năng chính |
| :--- | :--- | :---: | :--- | :--- |
| **Backend (REST API)** | ASP.NET Core 8 Web API, EF Core 8, SQLite, JWT | `5000` | Frontend MVC, Mobile App (mở rộng), Quản trị viên qua Swagger | Cung cấp 10 module REST API, xử lý nghiệp vụ, xác thực JWT, tương tác CSDL. |
| **Frontend Web** | ASP.NET Core 8 MVC (Razor), Vanilla CSS Glassmorphism | `5001` | Độc giả (Reader), Tác giả (Author), Quản trị viên (Admin) | Giao diện đọc truyện responsive, Studio sáng tác truyện, Cổng quản trị Admin Dashboard. |
| **Database** | SQLite Engine (`database.db`) | Local/VPS | Hệ thống lưu trữ | Lưu trữ người dùng, truyện, chương, thể loại, tủ sách, tương tác và thống kê. |

---

## 2. MÔ TẢ TỔNG QUAN HỆ THỐNG

### 2.1. Đối Tượng Sử Dụng (Actors & Phân Quyền)
Hệ thống thiết lập 3 nhóm tác nhân chính dựa trên cơ chế phân quyền (Role-Based Access Control):

1. **Độc giả (Reader):**
   - Đăng ký, đăng nhập tài khoản độc giả cá nhân.
   - Tìm kiếm truyện, lọc theo thể loại, xem danh mục truyện hot, bảng xếp hạng.
   - Đọc nội dung từng chương truyện, tùy chỉnh giao diện đọc (cỡ chữ, chế độ sáng/tối).
   - Thêm truyện vào Tủ sách yêu thích (Favorites), lưu lịch sử chương đang đọc.
   - Gửi bình luận đánh giá truyện/chương, gửi báo cáo vi phạm nội dung xấu.

2. **Tác giả (Author):**
   - Kế thừa toàn bộ quyền của Độc giả.
   - Truy cập giao diện **Author Studio** để tạo truyện mới, tải ảnh bìa, gán thể loại.
   - Soạn thảo và đăng tải các chương truyện mới (Thêm, Sửa, Xóa chương).
   - Theo dõi trạng thái kiểm duyệt của truyện (`Draft`, `Pending`, `Approved`, `Rejected`).
   - Xem thống kê lượt đọc và tương tác của các tác phẩm do mình sáng tác.

3. **Quản trị viên (Admin):**
   - Truy cập **Admin Dashboard UI**.
   - Kiểm duyệt truyện do các tác giả gửi lên (Phê duyệt hoặc Từ chối kèm lý do).
   - Quản lý danh sách người dùng (Khóa/Mở khóa tài khoản vi phạm).
   - Xử lý các báo cáo vi phạm (Reports) từ độc giả.
   - Xem thống kê toàn hệ thống: Tổng số truyện, tổng lượt view, số độc giả, bảng xếp hạng.

### 2.2. Môi Trường Vận Hành
- **Server:** Chạy trên môi trường máy chủ Linux VPS hoặc Windows Server hỗ trợ .NET 8 Runtime.
- **Client:** Trình duyệt web hiện đại (Google Chrome, Microsoft Edge, Safari, Firefox) hỗ trợ HTML5/CSS3.
- **Database Engine:** SQLite 3 tương thích cao, nhỏ gọn, không phụ thuộc service ngoài, dễ dàng backup và di chuyển.

---

## 3. CÁC CHỨC NĂNG CHÍNH (BACKLOG MAPPING)

Hệ thống bao gồm 11 hạng mục công việc chuẩn Scrum được đồng bộ trực tiếp với **Jira Project (Key: ITUQ)**:

| Mã FR / Issue | Tên Chức Năng (Feature) | Epic / Phân Hệ | Story Points | Mức Độ Ưu Tiên |
| :---: | :--- | :--- | :---: | :---: |
| **FR-01** `[ITUQ-1]` | API Xác thực & Quản lý Tài khoản (Auth & Identity) | Backend | 5 | High |
| **FR-02** `[ITUQ-2]` | API Quản lý Truyện & Thể loại (Stories & Genres) | Backend | 5 | High |
| **FR-03** `[ITUQ-3]` | API Quản lý Chương & Trình đọc truyện (Chapters & Reading) | Backend | 8 | High |
| **FR-04** `[ITUQ-4]` | API Tủ sách & Lịch sử đọc (Favorites & History) | Backend | 3 | Medium |
| **FR-05** `[ITUQ-5]` | API Bình luận & Báo cáo vi phạm (Comments & Reports) | Backend | 3 | Medium |
| **FR-06** `[ITUQ-6]` | API Quản trị hệ thống & Phê duyệt truyện (Admin Portal API) | Backend | 5 | High |
| **FR-07** `[ITUQ-7]` | API Thống kê & Bảng xếp hạng (Dashboard & Leaderboard) | Backend | 5 | Medium |
| **FR-08** `[ITUQ-8]` | Frontend Giao diện Độc giả (Reader Web MVC) | Frontend | 8 | High |
| **FR-09** `[ITUQ-9]` | Frontend Studio Tác giả (Author Studio) | Frontend | 5 | Medium |
| **FR-10** `[ITUQ-10]` | Frontend Cổng Quản trị Admin (Admin Dashboard UI) | Frontend | 5 | High |
| **FR-11** `[ITUQ-11]` | Kiểm thử tích hợp RESTful API & Viết Swagger Docs | Testing / QA | 3 | Low |

---

## 4. ĐẶC TẢ CHI TIẾT CÁC CHỨC NĂNG (USE CASE & API SPECIFICATION)

### FR-01: Xác Thực & Quản Lý Tài Khoản (Auth & Identity)
- **Mã:** FR-01 (`ITUQ-1`)
- **Tác nhân:** Mọi người dùng (Khách, Reader, Author, Admin).
- **Mô tả:** Đăng ký tài khoản mới, xác thực thông tin đăng nhập và cấp phát mã JWT Bearer Token an toàn.
- **Input:** `Email`, `Password`, `FullName` (nếu đăng ký).
- **Quy trình xử lý:**
  1. Người dùng gửi yêu cầu qua form MVC hoặc gọi REST endpoint `POST /api/auth/login`.
  2. Backend kiểm tra tài khoản trong bảng `Users`.
  3. Kiểm tra tính hợp lệ của mật khẩu và cờ kích hoạt `IsActive`.
  4. Tạo chuỗi ký JWT Token chứa Claims: `UserId`, `Email`, `Role`.
  5. Trả về Token và thông tin định danh cho Frontend.
- **Output:**
  ```json
  {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
      "id": 1,
      "email": "reader@doctruyen.vn",
      "fullName": "Nguyễn Văn Đọc Giả",
      "role": "Reader"
    }
  }
  ```

---

### FR-02: Quản Lý Truyện & Thể Loại (Stories & Genres)
- **Mã:** FR-02 (`ITUQ-2`)
- **Tác nhân:** Độc giả (Xem/Tìm kiếm), Tác giả (Tạo/Sửa truyện), Admin (Quản lý danh mục thể loại).
- **Mô tả:** Xem danh sách truyện có phân trang, tìm kiếm theo từ khóa, lọc theo thể loại, xem chi tiết truyện.
- **Endpoint:**
  - `GET /api/stories`: Danh sách truyện kèm bộ lọc `genreId`, `searchKeyword`, `page`, `pageSize`.
  - `GET /api/stories/{id}`: Chi tiết truyện và danh sách các chương đã phát hành.
  - `POST /api/stories`: Tác giả tạo truyện mới (trạng thái ban đầu: `Pending` chờ duyệt).
  - `GET /api/genres`: Lấy toàn bộ danh mục thể loại (Tiên Hiệp, Kiếm Hiệp, Ngôn Tình, Đô Thị...).

---

### FR-03: Quản Lý Chương & Trình Đọc Truyện (Chapters & Reading)
- **Mã:** FR-03 (`ITUQ-3`)
- **Tác nhân:** Độc giả, Tác giả.
- **Mô tả:** Tải nội dung chương truyện để đọc, tự động tăng bộ đếm lượt xem (ViewCount), cung cấp điều hướng chương trước/sau (`prevChapterId`, `nextChapterId`).
- **Endpoint:**
  - `GET /api/chapters/{id}`: Trả về tiêu đề, số thứ tự chương, nội dung chữ, và ID chương liền kề.
  - `POST /api/chapters`: Tác giả thêm chương mới cho truyện của mình.

---

### FR-04: Tủ Sách Yêu Thích & Lịch Sử Đọc (Favorites & History)
- **Mã:** FR-04 (`ITUQ-4`)
- **Tác nhân:** Độc giả đã đăng nhập.
- **Mô tả:** Lưu truyện vào tủ sách cá nhân (Bookmark) để đọc lại sau; tự động lưu lại chương mới nhất đang đọc dở.
- **Endpoint:**
  - `POST /api/interactions/favorite/{storyId}`: Thêm/Xóa khỏi danh sách yêu thích.
  - `GET /api/interactions/favorites`: Lấy danh sách các truyện đã bookmark của user hiện tại.

---

### FR-05: Bình Luận & Báo Cáo Vi Phạm (Comments & Reports)
- **Mã:** FR-05 (`ITUQ-5`)
- **Tác nhân:** Độc giả, Quản trị viên.
- **Mô tả:** Độc giả gửi cảm nghĩ đánh giá dưới từng chương/truyện. Nếu phát hiện nội dung có vi phạm, độc giả gửi báo cáo vi phạm (`Report`) để Admin kiểm duyệt.
- **Endpoint:**
  - `GET /api/comments/story/{storyId}`: Lấy danh sách bình luận.
  - `POST /api/comments`: Gửi bình luận mới.
  - `POST /api/reports`: Gửi báo cáo vi phạm nội dung.

---

### FR-06: Quản Trị Hệ Thống & Phê Duyệt Truyện (Admin Portal API)
- **Mã:** FR-06 (`ITUQ-6`)
- **Tác nhân:** Quản trị viên (Admin).
- **Mô tả:** Kiểm duyệt các tác phẩm do tác giả đăng tải trước khi xuất bản rộng rãi; quản lý tài khoản người dùng và xử lý báo cáo vi phạm.
- **Endpoint:**
  - `GET /api/admin/pending-stories`: Danh sách các truyện đang chờ duyệt.
  - `POST /api/admin/approve-story/{id}`: Phê duyệt phát hành truyện (`Status = Approved`).
  - `POST /api/admin/reject-story/{id}`: Từ chối truyện kèm lý do.
  - `POST /api/admin/users/{id}/toggle-lock`: Khóa hoặc kích hoạt lại tài khoản người dùng.

---

### FR-07: Thống Kê & Bảng Xếp Hạng (Dashboard & Leaderboard)
- **Mã:** FR-07 (`ITUQ-7`)
- **Tác nhân:** Admin, Khách truy cập.
- **Mô tả:** Thống kê tổng quan số liệu hệ thống và xếp hạng truyện theo lượt xem (Views), lượt yêu thích (Favorites).
- **Endpoint:**
  - `GET /api/stats/dashboard`: Tổng số truyện, người dùng, tổng lượt đọc, truyện chờ duyệt.
  - `GET /api/stats/leaderboard`: Top truyện hot nhất trong tuần/tháng.

---

### FR-08, 09, 10: Giao Diện Người Dùng Frontend (Razor MVC UI)
- **Giao diện Độc giả:** Thiết kế Responsive chuẩn UX đọc sách, phông chữ dễ đọc, tùy biến cỡ chữ, Dark/Light Mode.
- **Giao diện Tác giả:** Trang soạn thảo trực quan, quản lý danh sách truyện và chương tác giả đã viết.
- **Giao diện Quản trị viên:** Bảng điều khiển trực quan, nút thao tác duyệt nhanh 1-click.

---

## 5. YÊU CẦU PHI CHỨC NĂNG (NON-FUNCTIONAL REQUIREMENTS)

1. **Bảo Mật (Security):**
   - Mật khẩu người dùng được băm an toàn (BCrypt/PBKDF2 SHA-256), không lưu plaintext.
   - Các API nhạy cảm được bảo vệ nghiêm ngặt bằng JWT Bearer Authentication và Attribute `[Authorize(Roles = "...")]`.
   - Cơ chế phòng chống SQL Injection nhờ Entity Framework Core Parameterized Queries.
2. **Hiệu Năng (Performance):**
   - Tốc độ phản hồi của các REST API đọc dữ liệu đạt dưới 300ms.
   - Hỗ trợ phân trang (Pagination) để tối ưu hóa tải trang, tránh quá tải khi dữ liệu truyện lớn.
3. **Độ Tin Cậy & Tính Toàn Vẹn (Reliability & Integrity):**
   - Cơ chế Foreign Key ràng buộc toàn vẹn giữa Truyện, Chương, Bình luận và Thể loại.
   - Hệ cơ sở dữ liệu SQLite dạng file duy nhất giúp việc sao lưu (backup) tức thời.
4. **Khả Năng Sử Dụng (Usability):**
   - Giao diện thân thiện, chuẩn Cyber Indigo & Dark Glassmorphism, tự động co giãn tối ưu trên thiết bị di động.

---

## 6. THIẾT KẾ CƠ SỞ DỮ LIỆU & KIẾN TRÚC HỆ THỐNG

### 6.1. Sơ Đồ Thực Thể CSDL (ERD Tables)
- **`Users`**: `Id`, `Email`, `PasswordHash`, `FullName`, `Role`, `AvatarUrl`, `IsActive`, `CreatedAt`.
- **`Stories`**: `Id`, `Title`, `AuthorId`, `Description`, `CoverImageUrl`, `Status` (Pending/Approved/Rejected), `ViewCount`, `CreatedAt`.
- **`Genres`**: `Id`, `Name`, `Description`.
- **`StoryGenres`**: `StoryId`, `GenreId` (Khóa chính kết hợp).
- **`Chapters`**: `Id`, `StoryId`, `ChapterNumber`, `Title`, `Content`, `CreatedAt`.
- **`Favorites`**: `UserId`, `StoryId`, `LastReadChapterId`, `UpdatedAt`.
- **`Comments`**: `Id`, `UserId`, `StoryId`, `ChapterId`, `Content`, `CreatedAt`.
- **`Reports`**: `Id`, `ReporterId`, `TargetType`, `TargetId`, `Reason`, `Status`, `CreatedAt`.

### 6.2. Kiến Trúc Giải Pháp
```
+-------------------------------------------------------------+
|              CLIENT BROWSERS (Desktop / Mobile)             |
+-------------------------------------------------------------+
                              | (HTTP / Port 5001)
+-----------------------------v-------------------------------+
|         FRONTEND: ASP.NET Core 8 MVC (Controllers & Views)   |
+-------------------------------------------------------------+
                              | (JSON REST API / Port 5000)
+-----------------------------v-------------------------------+
|         BACKEND: ASP.NET Core 8 Web API (JWT Auth + EF Core)|
+-------------------------------------------------------------+
                              |
+-----------------------------v-------------------------------+
|            DATABASE ENGINE: SQLite (database.db)            |
+-------------------------------------------------------------+
```

---

## 7. KẾ HOẠCH BÀN GIAO & TIẾN ĐỘ SPRINT
Toàn bộ 11 hạng mục trong tài liệu SRS này đã được nhóm hiện thực hóa thành các User Stories trên Jira, triển khai thành công mã nguồn Backend và Frontend, sẵn sàng phục vụ công tác kiểm tra đồ án của Bộ môn.
