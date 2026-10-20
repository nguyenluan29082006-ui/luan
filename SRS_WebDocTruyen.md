# TÀI LIỆU ĐẶC TẢ YÊU CẦU PHẦN MỀM (SRS)
## Hệ Thống Web Đọc Truyện Trực Tuyến — WebDocTruyen

| Thông tin | Nội dung |
|-----------|----------|
| **Tên dự án** | WebDocTruyen — Nền tảng đọc truyện đa vai trò |
| **Phiên bản** | 1.0.0 |
| **Ngày lập** | 2026-10-20 |
| **Nhóm phát triển** | Nhóm sinh viên — Môn CNPMNC |
| **Jira Project** | ITUQ — https://nguyenluan29082006.atlassian.net |
| **GitHub** | https://github.com/nguyenluan29082006-ui/luan |
| **Trạng thái** | ✅ Hoàn thành — Sprint 1 Done (47 Story Points) |

---

## MỤC LỤC

1. [Tổng quan tài liệu](#1-tổng-quan-tài-liệu)
2. [Mô tả tổng quan hệ thống](#2-mô-tả-tổng-quan-hệ-thống)
3. [Các chức năng chính — Backlog Mapping](#3-các-chức-năng-chính)
4. [Đặc tả chi tiết chức năng](#4-đặc-tả-chi-tiết-chức-năng)
5. [Yêu cầu phi chức năng](#5-yêu-cầu-phi-chức-năng)
6. [Thiết kế cơ sở dữ liệu](#6-thiết-kế-cơ-sở-dữ-liệu)
7. [Kiến trúc hệ thống](#7-kiến-trúc-hệ-thống)
8. [Giao diện người dùng](#8-giao-diện-người-dùng)
9. [Kế hoạch bàn giao](#9-kế-hoạch-bàn-giao)

---

## 1. TỔNG QUAN TÀI LIỆU

### 1.1. Mục Đích

Tài liệu SRS (Software Requirements Specification) này mô tả toàn diện các yêu cầu chức năng, yêu cầu phi chức năng, thiết kế cơ sở dữ liệu, kiến trúc công nghệ và giao diện người dùng của **Hệ thống Web Đọc Truyện Trực Tuyến (WebDocTruyen)**.

Tài liệu phục vụ:
- **Giảng viên / Trợ giáo**: Đánh giá tiến độ, kiến trúc và mức độ hoàn thiện của hệ thống.
- **Nhóm phát triển**: Chuẩn mực kỹ thuật thống nhất giữa các thành viên.
- **Đối chiếu Jira**: Căn cứ so sánh với Sprint Backlog trên Jira Project (Key: ITUQ).

### 1.2. Phạm Vi Hệ Thống

Hệ thống được phát triển thuần trên nền tảng **C# / .NET 8**, tích hợp cả Backend API và Frontend MVC trong một project duy nhất (`WebDocTruyen.csproj`), chạy trên cổng `5000`:

| Thành phần | Công nghệ | Mô tả |
|-----------|-----------|-------|
| **REST API (Backend)** | ASP.NET Core 8 Web API, EF Core 8, SQLite, JWT | 30+ endpoints xử lý nghiệp vụ, xác thực JWT, tương tác CSDL |
| **Web UI (Frontend)** | ASP.NET Core 8 MVC (Razor Views), Vanilla CSS Glassmorphism | Giao diện đọc truyện, Author Studio, Admin Dashboard |
| **Database** | SQLite (`database.db`) | Lưu trữ toàn bộ dữ liệu, file duy nhất, dễ backup |
| **Authentication** | JWT Bearer Token + Cookie Authentication | Dual-scheme: Cookie cho MVC, JWT cho API |
| **API Docs** | Swagger UI (`/swagger`) | Tài liệu & kiểm thử API trực tiếp trên trình duyệt |

### 1.3. Định Nghĩa & Viết Tắt

| Ký hiệu | Ý nghĩa |
|---------|---------|
| FR | Functional Requirement (Yêu cầu chức năng) |
| NFR | Non-Functional Requirement (Yêu cầu phi chức năng) |
| API | Application Programming Interface |
| MVC | Model - View - Controller |
| JWT | JSON Web Token |
| EF Core | Entity Framework Core (ORM) |
| RBAC | Role-Based Access Control (Phân quyền theo vai trò) |
| SRS | Software Requirements Specification |
| CRUD | Create, Read, Update, Delete |

---

## 2. MÔ TẢ TỔNG QUAN HỆ THỐNG

### 2.1. Đối Tượng Sử Dụng (Actors & Phân Quyền RBAC)

Hệ thống thiết lập **3 vai trò** dựa trên cơ chế phân quyền RBAC, lưu trong trường `Role` của bảng `Users`:

#### 👤 Vai trò 1: Độc giả (`role = "reader"`)
- Đăng ký / Đăng nhập tài khoản cá nhân.
- Tìm kiếm, lọc và duyệt truyện theo thể loại, loại hình, tình trạng.
- Đọc nội dung từng chương truyện.
- Tùy chỉnh trình đọc: cỡ chữ (tăng/giảm), chế độ Sáng/Tối.
- Thêm truyện vào Tủ sách yêu thích (Favorites), lưu lịch sử chương đang đọc.
- Gửi bình luận dưới truyện, gửi báo cáo vi phạm nội dung.
- Nâng cấp lên vai trò Tác giả qua `POST /api/auth/become-author`.

#### ✍️ Vai trò 2: Tác giả (`role = "author"`)
- Kế thừa **toàn bộ quyền** của Độc giả.
- Truy cập **Author Studio** (`/Author/Index`) để quản lý tác phẩm.
- Tạo truyện mới: nhập tên, mô tả, ảnh bìa (URL), gán thể loại, chọn loại hình (novel/comic).
- Thêm chương mới cho truyện của mình: tiêu đề, số thứ tự, nội dung văn bản.
- Xóa truyện do mình sở hữu.
- Theo dõi trạng thái kiểm duyệt: `pending` → `approved` / `rejected` (kèm lý do từ chối).

#### 🛡️ Vai trò 3: Quản trị viên (`role = "admin"`)
- Kế thừa quyền Tác giả, thêm quyền quản trị toàn hệ thống.
- Truy cập **Admin Dashboard** (`/Admin/Index`).
- Kiểm duyệt truyện: Phê duyệt (`approved`) hoặc Từ chối (`rejected`) kèm lý do.
- Quản lý tài khoản: Đổi vai trò, Khóa/Mở khóa tài khoản vi phạm.
- Xử lý báo cáo vi phạm: Giải quyết (xóa nội dung) hoặc Bác bỏ.
- Quản lý thể loại: Thêm / Xóa thể loại truyện.
- Xem thống kê toàn hệ thống và bảng xếp hạng.

### 2.2. Môi Trường Vận Hành

| Thành phần | Yêu cầu |
|-----------|---------|
| **Runtime** | .NET 8 Runtime (Windows / Linux / macOS) |
| **Cổng mặc định** | `http://localhost:5000` |
| **Trình duyệt** | Chrome, Edge, Firefox, Safari (hỗ trợ HTML5/CSS3) |
| **Database** | SQLite 3 — file `database.db` trong thư mục gốc |
| **Deploy** | VPS Linux, Docker, Render.com |

---

## 3. CÁC CHỨC NĂNG CHÍNH

### 3.1. Backlog Mapping — Jira ITUQ

| Mã FR | Issue Jira | Tên chức năng | Epic | SP | Ưu tiên | Trạng thái |
|-------|-----------|---------------|------|----|---------|-----------|
| FR-01 | ITUQ-1 | API Xác thực & Quản lý Tài khoản | Backend | 5 | High | ✅ Done |
| FR-02 | ITUQ-2 | API Quản lý Truyện & Thể loại | Backend | 5 | High | ✅ Done |
| FR-03 | ITUQ-3 | API Quản lý Chương & Trình đọc | Backend | 8 | High | ✅ Done |
| FR-04 | ITUQ-4 | API Tủ sách & Lịch sử đọc | Backend | 3 | Medium | ✅ Done |
| FR-05 | ITUQ-5 | API Bình luận & Báo cáo vi phạm | Backend | 3 | Medium | ✅ Done |
| FR-06 | ITUQ-6 | API Quản trị & Phê duyệt truyện | Backend | 5 | High | ✅ Done |
| FR-07 | ITUQ-7 | API Thống kê & Bảng xếp hạng | Backend | 5 | Medium | ✅ Done |
| FR-08 | ITUQ-8 | Frontend Giao diện Độc giả | Frontend | 8 | High | ✅ Done |
| FR-09 | ITUQ-9 | Frontend Author Studio | Frontend | 5 | Medium | ✅ Done |
| FR-10 | ITUQ-10 | Frontend Admin Dashboard UI | Frontend | 5 | High | ✅ Done |
| FR-11 | ITUQ-11 | Kiểm thử API & Swagger Docs | Testing | 3 | Low | ✅ Done |
| | | **TỔNG CỘNG** | | **47 SP** | | **100%** |

---

## 4. ĐẶC TẢ CHI TIẾT CHỨC NĂNG

---

### FR-01: Xác Thực & Quản Lý Tài Khoản (Auth & Identity)

**Mã:** FR-01 | **Issue:** ITUQ-1 | **File:** `Controllers/AuthController.cs`, `Controllers/AccountController.cs`

**Tác nhân:** Mọi người dùng (Khách, Reader, Author, Admin)

**Mô tả:** Hệ thống xác thực hai lớp — Cookie Authentication cho MVC web (người dùng thông thường) và JWT Bearer Token cho REST API (tích hợp bên ngoài, Swagger).

#### Danh sách API Endpoints:

| Method | Endpoint | Mô tả | Auth yêu cầu |
|--------|----------|-------|-------------|
| POST | `/api/auth/register` | Đăng ký tài khoản mới | Không |
| POST | `/api/auth/login` | Đăng nhập, nhận JWT Token | Không |
| GET | `/api/auth/me` | Lấy thông tin profile hiện tại | Bearer Token |
| POST | `/api/auth/become-author` | Nâng cấp Reader → Author | Bearer Token |

#### Danh sách MVC Actions:

| Method | Route | Mô tả |
|--------|-------|-------|
| GET | `/Account/Login` | Hiển thị form đăng nhập |
| POST | `/Account/Login` | Xử lý đăng nhập (Cookie) |
| GET | `/Account/Register` | Hiển thị form đăng ký |
| POST | `/Account/Register` | Xử lý đăng ký |
| POST | `/Account/Logout` | Đăng xuất, xóa Cookie |

#### Luồng xử lý đăng nhập:
1. Người dùng nhập Email + Password vào form.
2. Backend tìm `User` theo `Email` trong bảng `Users`.
3. Xác minh mật khẩu qua `BCrypt.Verify()`.
4. Kiểm tra `Status != "banned"` — nếu bị khóa thì từ chối.
5. Tạo JWT Token chứa Claims: `UserId`, `Email`, `Name`, `Role`.
6. Trả về Token (API) hoặc thiết lập Cookie (MVC).

#### Response mẫu (API Login):
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "name": "Nguyễn Văn A",
    "email": "reader@doctruyen.vn",
    "role": "reader"
  }
}
```

---

### FR-02: Quản Lý Truyện & Thể Loại (Stories & Genres)

**Mã:** FR-02 | **Issue:** ITUQ-2 | **File:** `Controllers/StoriesController.cs`, `Controllers/HomeController.cs`

**Tác nhân:** Độc giả (xem/tìm kiếm), Tác giả (tạo truyện), Admin (quản lý thể loại)

#### Danh sách API Endpoints:

| Method | Endpoint | Mô tả | Auth |
|--------|----------|-------|------|
| GET | `/api/stories` | Danh sách truyện (filter + phân trang) | Không |
| GET | `/api/stories/featured` | Top 6 truyện nhiều view nhất | Không |
| GET | `/api/stories/{id}` | Chi tiết truyện + danh sách chương | Không |
| POST | `/api/stories` | Tạo truyện mới | Author/Admin |
| GET | `/api/genres` | Danh sách toàn bộ thể loại | Không |

#### Query Parameters cho `GET /api/stories`:

| Tham số | Kiểu | Mô tả |
|---------|------|-------|
| `search` | string | Tìm kiếm theo tên truyện hoặc tên tác giả |
| `genreSlug` | string | Lọc theo slug thể loại (vd: `tien-hiep`) |
| `type` | string | Lọc theo loại: `novel` hoặc `comic` |
| `status` | string | Lọc theo tình trạng: `ongoing` hoặc `completed` |
| `page` | int | Số trang (mặc định: 1) |
| `pageSize` | int | Số truyện mỗi trang (mặc định: 12) |

#### Quy tắc nghiệp vụ:
- Truyện mới tạo có `ApprovalStatus = "pending"` — **chưa hiển thị** với Độc giả.
- Chỉ truyện có `ApprovalStatus = "approved"` mới xuất hiện ở trang chủ và tìm kiếm.
- `Slug` tự động sinh từ tên truyện, đảm bảo unique bằng cách thêm suffix số nếu trùng.
- Truyện bị từ chối (`rejected`) hiển thị lý do từ chối (`RejectionReason`) trong Author Studio.

---

### FR-03: Quản Lý Chương & Trình Đọc Truyện (Chapters & Reading)

**Mã:** FR-03 | **Issue:** ITUQ-3 | **File:** `Controllers/ChaptersController.cs`, `Controllers/ReaderController.cs`

**Tác nhân:** Độc giả (đọc chương), Tác giả (thêm chương)

#### Danh sách API Endpoints:

| Method | Endpoint | Mô tả | Auth |
|--------|----------|-------|------|
| GET | `/api/chapters/{id}` | Nội dung chương + điều hướng Prev/Next | Không |
| POST | `/api/chapters` | Tạo chương mới | Author/Admin |

#### Danh sách MVC Actions:

| Method | Route | Mô tả |
|--------|-------|-------|
| GET | `/Reader/Read/{chapterId}` | Trang đọc truyện |

#### Response mẫu `GET /api/chapters/{id}`:
```json
{
  "id": 5,
  "storyId": 1,
  "chapterNumber": 3,
  "title": "Chương 3: Bí Ẩn Động Thiên",
  "content": "Nội dung chương...",
  "views": 1247,
  "prevChapterId": 4,
  "nextChapterId": 6,
  "createdAt": "2026-10-01T08:00:00Z"
}
```

#### Quy tắc nghiệp vụ:
- Mỗi lần gọi `GET /api/chapters/{id}`, `Views` của chương và `Views` của truyện mẹ **tự động tăng 1**.
- `ChapterNumber` tự động tính bằng `(số chương hiện có + 1)` nếu không được nhập.
- Chỉ Tác giả chủ sở hữu hoặc Admin mới được thêm chương cho truyện.

---

### FR-04: Tủ Sách & Lịch Sử Đọc (Favorites & Reading History)

**Mã:** FR-04 | **Issue:** ITUQ-4 | **File:** `Controllers/InteractionsController.cs`, `Controllers/HomeController.cs`

**Tác nhân:** Độc giả đã đăng nhập

#### Danh sách API Endpoints:

| Method | Endpoint | Mô tả | Auth |
|--------|----------|-------|------|
| GET | `/api/favorites` | Danh sách truyện trong tủ sách | Bearer Token |
| GET | `/api/favorites/check/{storyId}` | Kiểm tra truyện đã lưu chưa | Bearer Token |
| POST | `/api/favorites/toggle/{storyId}` | Thêm/Xóa tủ sách (Toggle) | Bearer Token |
| GET | `/api/history` | Lịch sử đọc (sort theo mới nhất) | Bearer Token |
| POST | `/api/history` | Lưu chương đang đọc | Bearer Token |

#### Quy tắc nghiệp vụ:
- Toggle Favorite: Nếu đã có → xóa; Nếu chưa có → thêm.
- Lịch sử đọc dùng cơ chế **Upsert** — mỗi truyện chỉ lưu 1 bản ghi chương đọc gần nhất.
- Khi Độc giả chưa đăng nhập truy cập `/Home/Favorites`, hệ thống redirect về `/Account/Login`.

---

### FR-05: Bình Luận & Báo Cáo Vi Phạm (Comments & Reports)

**Mã:** FR-05 | **Issue:** ITUQ-5 | **File:** `Controllers/CommentsAndReportsController.cs`, `Controllers/StoryController.cs`

**Tác nhân:** Độc giả (gửi bình luận/báo cáo), Admin (xử lý báo cáo)

#### Danh sách API Endpoints:

| Method | Endpoint | Mô tả | Auth |
|--------|----------|-------|------|
| GET | `/api/comments/story/{storyId}` | Danh sách bình luận theo truyện | Không |
| POST | `/api/comments` | Gửi bình luận mới | Bearer Token |
| DELETE | `/api/comments/{id}` | Xóa bình luận | Bearer Token |
| POST | `/api/reports` | Gửi báo cáo vi phạm | Bearer Token |

#### Cấu trúc Request `POST /api/reports`:
```json
{
  "targetType": "comment",
  "targetId": 42,
  "reason": "Nội dung xúc phạm",
  "details": "Bình luận chứa ngôn từ thô tục"
}
```

#### `TargetType` hợp lệ: `"story"`, `"chapter"`, `"comment"`

---

### FR-06: Quản Trị Hệ Thống & Phê Duyệt Truyện (Admin Portal)

**Mã:** FR-06 | **Issue:** ITUQ-6 | **File:** `Controllers/AdminController.cs`

**Tác nhân:** Quản trị viên (Admin) — `[Authorize(Roles = "admin")]`

#### Danh sách MVC Actions:

| Method | Route | Mô tả |
|--------|-------|-------|
| GET | `/Admin/Index` | Dashboard với tất cả tabs |
| POST | `/Admin/ApproveStory/{id}` | Phê duyệt truyện (`approved`) |
| POST | `/Admin/RejectStory/{id}` | Từ chối truyện + lý do (`rejected`) |
| POST | `/Admin/ResolveReport/{id}` | Giải quyết báo cáo (xóa nội dung vi phạm) |
| POST | `/Admin/DismissReport/{id}` | Bác bỏ báo cáo (`dismissed`) |
| POST | `/Admin/ChangeUserRole/{id}` | Đổi vai trò người dùng |
| POST | `/Admin/ToggleUserBan/{id}` | Khóa/Mở khóa tài khoản |
| POST | `/Admin/CreateGenre` | Tạo thể loại mới |
| POST | `/Admin/DeleteGenre/{id}` | Xóa thể loại |

#### Quy tắc nghiệp vụ:
- Admin **không thể tự khóa** tài khoản của chính mình (`user.Role != "admin"`).
- Khi Approve truyện: `ApprovalStatus = "approved"`, xóa `RejectionReason`.
- Khi Reject truyện: `ApprovalStatus = "rejected"`, lưu `RejectionReason` để tác giả xem.
- Khi Resolve report với action `delete_target`: xóa vật thể bị báo cáo khỏi CSDL.
- Kết quả mỗi action được thông báo qua `TempData["SuccessMessage"]` / `TempData["InfoMessage"]`.

---

### FR-07: Thống Kê & Bảng Xếp Hạng (Dashboard & Leaderboard)

**Mã:** FR-07 | **Issue:** ITUQ-7 | **File:** `Controllers/AdminController.cs` (ViewBag data)

**Tác nhân:** Admin (xem Dashboard), Tất cả (xem Leaderboard trang chủ)

#### Số liệu thống kê cung cấp:

| Chỉ số | Nguồn dữ liệu |
|--------|--------------|
| Tổng số người dùng | `Users.CountAsync()` |
| Tổng số truyện | `Stories.CountAsync()` |
| Tổng số chương | `Chapters.CountAsync()` |
| Tổng lượt xem | `Stories.SumAsync(s => s.Views)` |
| Truyện chờ duyệt | `Stories.CountAsync(s => s.ApprovalStatus == "pending")` |
| Báo cáo chờ xử lý | `Reports.CountAsync(r => r.Status == "pending")` |
| Top truyện hot | `Stories.OrderByDescending(s => s.Views).Take(6)` |

---

### FR-08: Frontend Giao Diện Độc Giả (Reader Web MVC)

**Mã:** FR-08 | **Issue:** ITUQ-8

#### Danh sách trang:

| Route | View File | Mô tả |
|-------|-----------|-------|
| `/` hoặc `/Home/Index` | `Views/Home/Index.cshtml` | Trang chủ |
| `/Home/Genres` | `Views/Home/Genres.cshtml` | Trang khám phá / tìm kiếm |
| `/Story/Detail/{id}` | `Views/Story/Detail.cshtml` | Chi tiết truyện |
| `/Reader/Read/{chapterId}` | `Views/Reader/Read.cshtml` | Trình đọc truyện |
| `/Home/Favorites` | `Views/Home/Favorites.cshtml` | Tủ sách + Lịch sử |

#### Trang chủ (`Home/Index`) bao gồm:
- **Hero Banner**: Truyện nổi bật nhất (nhiều view nhất).
- **Top Truyện Hot**: 6 truyện nhiều view nhất.
- **Mới Cập Nhật**: 8 truyện mới nhất theo `CreatedAt`.
- **Danh mục Thể loại**: Tất cả thể loại kèm số lượng truyện.

#### Trình đọc truyện (`Reader/Read`) tính năng:
- Hiển thị nội dung chương dạng văn bản.
- Nút điều hướng **Chương Trước** / **Chương Sau**.
- Nút **Tăng/Giảm cỡ chữ** (lưu vào `localStorage`).
- Nút chuyển **Dark Mode / Light Mode**.
- Tự động lưu lịch sử đọc qua `POST /api/history`.

---

### FR-09: Frontend Author Studio

**Mã:** FR-09 | **Issue:** ITUQ-9 | **File:** `Controllers/AuthorController.cs`, `Views/Author/Index.cshtml`

**Truy cập:** `/Author/Index` — Chỉ `author` và `admin`

#### Tính năng Author Studio:

| Tính năng | Mô tả |
|----------|-------|
| Danh sách truyện | Hiển thị tất cả truyện của mình với Views, số Chương, badge Status |
| Tạo truyện mới | Form: Tên, Mô tả, URL ảnh bìa, Thể loại (multi-select), Loại (novel/comic) |
| Thêm chương | Form: Tiêu đề, Số thứ tự (tùy chọn), Nội dung văn bản |
| Xóa truyện | Chỉ xóa được truyện của mình (Admin xóa được tất cả) |
| Trạng thái kiểm duyệt | Badge màu: Pending (vàng), Approved (xanh lá), Rejected (đỏ) + hiển thị lý do |

---

### FR-10: Frontend Admin Dashboard UI

**Mã:** FR-10 | **Issue:** ITUQ-10 | **File:** `Controllers/AdminController.cs`, `Views/Admin/Index.cshtml`

**Truy cập:** `/Admin/Index` — Chỉ `admin`

#### Cấu trúc Dashboard (4 tabs):

**Tab 1 — Tổng quan:** 6 KPI cards hiển thị số liệu thống kê toàn hệ thống.

**Tab 2 — Duyệt Truyện:**
- Danh sách truyện `ApprovalStatus = "pending"`.
- Nút **Phê duyệt** 1-click.
- Nút **Từ chối** kèm textarea nhập lý do.

**Tab 3 — Quản lý Users:**
- Bảng danh sách tất cả người dùng.
- Dropdown đổi Role (reader/author/admin).
- Nút Ban/Unban tài khoản.

**Tab 4 — Báo cáo vi phạm:**
- Danh sách tất cả Reports.
- Nút **Giải quyết** (xóa nội dung vi phạm).
- Nút **Bác bỏ** (dismiss).

**Tab 5 — Quản lý Thể loại:**
- Form thêm thể loại mới (Tên + Slug + Icon emoji).
- Danh sách thể loại hiện có với nút Xóa.

---

### FR-11: Kiểm Thử & Swagger Docs

**Mã:** FR-11 | **Issue:** ITUQ-11 | **File:** `ServerCSharp.http`, `Program.cs`

#### Phạm vi kiểm thử:

| Nhóm | Endpoints được test |
|------|-------------------|
| Auth | Register, Login, Get Me, Become Author |
| Stories | Get List, Get Featured, Get Detail, Create |
| Chapters | Get Chapter, Create Chapter |
| Favorites | Toggle, Get List, Check |
| History | Save, Get |
| Comments | Get by Story, Add, Delete |
| Reports | Create Report |
| Admin | Approve, Reject, Toggle Ban, Change Role |

#### Swagger UI:
- Truy cập: `http://localhost:5000/swagger`
- Hỗ trợ Bearer Token Authentication.
- Mô tả đầy đủ request/response cho tất cả endpoints.

---

## 5. YÊU CẦU PHI CHỨC NĂNG

### 5.1. Bảo Mật (Security)

| Yêu cầu | Cài đặt trong code |
|---------|-------------------|
| Hash mật khẩu | `BCrypt.Net.BCrypt.HashPassword()` — không lưu plaintext |
| Xác thực API | JWT Bearer Token với `IssuerSigningKey` (HS256) |
| Xác thực MVC | Cookie Authentication — `ExpireTimeSpan = 7 ngày` |
| Phân quyền | `[Authorize(Roles = "admin")]`, `[Authorize(Roles = "author,admin")]` |
| SQL Injection | EF Core Parameterized Queries — không dùng raw SQL |
| CORS | Cùng domain (monolith), không cần CORS config riêng |

### 5.2. Hiệu Năng (Performance)

| Yêu cầu | Mô tả |
|---------|-------|
| Response time | REST API đọc dữ liệu: < 300ms |
| Phân trang | Pagination cho danh sách truyện, tránh load toàn bộ data |
| Lazy Loading | Dùng `Include().ThenInclude()` thay vì N+1 queries |
| Database | SQLite — đủ cho quy mô đồ án sinh viên (~10.000 bản ghi) |

### 5.3. Độ Tin Cậy (Reliability)

| Yêu cầu | Mô tả |
|---------|-------|
| Toàn vẹn dữ liệu | Foreign Key constraints trong EF Core (`OnModelCreating`) |
| Backup | SQLite file `database.db` — backup đơn giản bằng copy file |
| Seed data | `DbInitializer.Initialize()` chạy khi khởi động — tạo dữ liệu mẫu |
| Error handling | `try/catch` trong Controllers, TempData flash messages |

### 5.4. Khả Năng Sử Dụng (Usability)

| Yêu cầu | Mô tả |
|---------|-------|
| Design | Glassmorphism, Cyber Indigo color scheme, Dark mode |
| Responsive | CSS media queries — tương thích mobile/tablet/desktop |
| Font | Google Fonts (Inter, Outfit) |
| Accessibility | Semantic HTML5, alt text cho ảnh |
| UX | Flash messages, loading states, điều hướng rõ ràng |

---

## 6. THIẾT KẾ CƠ SỞ DỮ LIỆU

### 6.1. Sơ Đồ Thực Thể (ERD)

```
Users ─────────────── Stories ─────────────── Chapters
  │ 1                    │ 1                      │
  │                      │                        │
  │ N                    │ N                    N │
Favorites          StoryGenres            ReadingHistories
  │                      │ N                      │
  │                   N  │                        │
  │                   Genres                      │
  │ 1                                          N  │
Comments ──────────────────────────── (StoryId, ChapterId)
  │
Reports
```

### 6.2. Chi Tiết Các Bảng

#### Bảng `Users`
| Cột | Kiểu | Ràng buộc | Mô tả |
|-----|------|-----------|-------|
| `Id` | int | PK, Auto | ID người dùng |
| `Name` | string(100) | Required | Họ tên hiển thị |
| `Email` | string(150) | Required, Unique | Email đăng nhập |
| `Password` | string | Required | BCrypt hash |
| `Role` | string(20) | Default: "reader" | reader / author / admin |
| `Status` | string(20) | Default: "active" | active / banned |
| `Avatar` | string? | Nullable | URL ảnh đại diện |
| `Bio` | string? | Nullable | Giới thiệu bản thân |
| `CreatedAt` | DateTime | Auto | Thời gian tạo |

#### Bảng `Stories`
| Cột | Kiểu | Ràng buộc | Mô tả |
|-----|------|-----------|-------|
| `Id` | int | PK, Auto | ID truyện |
| `Title` | string(250) | Required | Tên truyện |
| `Slug` | string(250) | Required, Unique | URL-friendly name |
| `AuthorId` | int | FK → Users | ID tác giả |
| `AuthorName` | string(100) | | Tên tác giả (cache) |
| `Description` | string | Required | Mô tả truyện |
| `CoverUrl` | string? | Nullable | URL ảnh bìa |
| `Type` | string(20) | Default: "novel" | novel / comic |
| `Status` | string(20) | Default: "ongoing" | ongoing / completed |
| `ApprovalStatus` | string(20) | Default: "pending" | pending / approved / rejected |
| `RejectionReason` | string? | Nullable | Lý do từ chối (Admin) |
| `Views` | int | Default: 0 | Tổng lượt xem |
| `CreatedAt` | DateTime | Auto | Ngày tạo |
| `UpdatedAt` | DateTime | Auto | Lần cập nhật cuối |

#### Bảng `Genres`
| Cột | Kiểu | Ràng buộc | Mô tả |
|-----|------|-----------|-------|
| `Id` | int | PK, Auto | ID thể loại |
| `Name` | string(100) | Required | Tên thể loại |
| `Slug` | string(100) | Required, Unique | URL slug |
| `Icon` | string? | Default: "📖" | Emoji icon |

#### Bảng `StoryGenres` (Many-to-Many)
| Cột | Kiểu | Ràng buộc |
|-----|------|-----------|
| `StoryId` | int | FK → Stories |
| `GenreId` | int | FK → Genres |
| *(StoryId, GenreId)* | | Composite PK |

#### Bảng `Chapters`
| Cột | Kiểu | Ràng buộc | Mô tả |
|-----|------|-----------|-------|
| `Id` | int | PK, Auto | ID chương |
| `StoryId` | int | FK → Stories | ID truyện mẹ |
| `ChapterNumber` | int | Required | Số thứ tự chương |
| `Title` | string | Required | Tiêu đề chương |
| `Content` | string | Required | Nội dung văn bản |
| `Views` | int | Default: 0 | Lượt xem chương |
| `CreatedAt` | DateTime | Auto | Ngày đăng |

#### Bảng `Favorites`
| Cột | Kiểu | Ràng buộc | Mô tả |
|-----|------|-----------|-------|
| `Id` | int | PK, Auto | ID |
| `UserId` | int | FK → Users | ID người dùng |
| `StoryId` | int | FK → Stories | ID truyện |
| `CreatedAt` | DateTime | Auto | Ngày thêm vào tủ |

#### Bảng `ReadingHistories`
| Cột | Kiểu | Ràng buộc | Mô tả |
|-----|------|-----------|-------|
| `Id` | int | PK, Auto | ID |
| `UserId` | int | FK → Users | ID người dùng |
| `StoryId` | int | FK → Stories | ID truyện |
| `ChapterId` | int | FK → Chapters | Chương đang đọc dở |
| `UpdatedAt` | DateTime | Auto | Lần đọc gần nhất |

#### Bảng `Comments`
| Cột | Kiểu | Ràng buộc | Mô tả |
|-----|------|-----------|-------|
| `Id` | int | PK, Auto | ID |
| `UserId` | int | FK → Users | Người bình luận |
| `StoryId` | int | FK → Stories | Truyện được bình luận |
| `ChapterId` | int? | FK → Chapters, Nullable | Chương cụ thể (nếu có) |
| `Content` | string | Required | Nội dung bình luận |
| `CreatedAt` | DateTime | Auto | Thời gian gửi |

#### Bảng `Reports`
| Cột | Kiểu | Ràng buộc | Mô tả |
|-----|------|-----------|-------|
| `Id` | int | PK, Auto | ID |
| `UserId` | int | FK → Users | Người báo cáo |
| `TargetType` | string | Required | story / chapter / comment |
| `TargetId` | int | Required | ID đối tượng bị báo cáo |
| `Reason` | string | Required | Lý do báo cáo |
| `Details` | string? | Nullable | Chi tiết bổ sung |
| `Status` | string | Default: "pending" | pending / resolved / dismissed |
| `ResolutionNotes` | string? | Nullable | Ghi chú xử lý của Admin |
| `CreatedAt` | DateTime | Auto | Thời gian báo cáo |

---

## 7. KIẾN TRÚC HỆ THỐNG

### 7.1. Kiến Trúc Tổng Quan

```
┌─────────────────────────────────────────────────────┐
│           TRÌNH DUYỆT (Desktop / Mobile)            │
│     Chrome / Edge / Firefox / Safari                │
└──────────────────────┬──────────────────────────────┘
                       │ HTTP / Port 5000
┌──────────────────────▼──────────────────────────────┐
│     ASP.NET Core 8 — WebDocTruyen.csproj            │
│  ┌─────────────────┐   ┌──────────────────────────┐ │
│  │  MVC Controllers │   │   API Controllers        │ │
│  │  (Razor Views)   │   │   [ApiController]        │ │
│  │                  │   │   Route: /api/...        │ │
│  │  /Home           │   │   /api/auth              │ │
│  │  /Story          │   │   /api/stories           │ │
│  │  /Reader         │   │   /api/chapters          │ │
│  │  /Author         │   │   /api/favorites         │ │
│  │  /Admin          │   │   /api/history           │ │
│  │  /Account        │   │   /api/comments          │ │
│  └────────┬─────────┘   │   /api/reports           │ │
│           │             └──────────────┬───────────┘ │
│  ┌────────▼─────────────────────────▼─────────────┐ │
│  │        Entity Framework Core 8 (ORM)           │ │
│  │           AppDbContext (SQLite)                 │ │
│  └─────────────────────────────────────────────────┘ │
│  ┌─────────────────────────────────────────────────┐ │
│  │    Authentication: Cookie + JWT Bearer          │ │
│  │    Swagger UI: /swagger                         │ │
│  │    Static Files: /wwwroot                       │ │
│  └─────────────────────────────────────────────────┘ │
└──────────────────────┬──────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────┐
│              SQLite — database.db                    │
│  Users | Stories | Genres | Chapters | Favorites    │
│  ReadingHistories | Comments | Reports | StoryGenres │
└─────────────────────────────────────────────────────┘
```

### 7.2. Công Nghệ Sử Dụng

| Công nghệ | Phiên bản | Mục đích |
|-----------|-----------|---------|
| ASP.NET Core | 8.0 | Web framework chính |
| Entity Framework Core | 8.0.11 | ORM, database migrations |
| SQLite | 3 | Database engine |
| BCrypt.Net-Next | 4.0.3 | Hash mật khẩu |
| JWT Bearer | 8.0.11 | Xác thực API |
| Swashbuckle (Swagger) | 6.6.2 | API documentation |
| Razor Views | .NET 8 | Template engine |
| Vanilla CSS | — | Styling, Glassmorphism |
| Docker | — | Container hóa |

### 7.3. Cấu Trúc Thư Mục Project

```
WebDocTruyen/
├── Controllers/              # MVC + API Controllers
│   ├── AccountController.cs  # Đăng nhập/Đăng ký MVC
│   ├── AdminController.cs    # Admin Dashboard
│   ├── AuthController.cs     # Auth JWT API
│   ├── AuthorController.cs   # Author Studio
│   ├── ChaptersController.cs # Chapters API
│   ├── CommentsAndReportsController.cs
│   ├── HomeController.cs     # Trang chủ, Genres, Favorites
│   ├── InteractionsController.cs # Favorites & History API
│   ├── ReaderController.cs   # Trang đọc truyện
│   ├── StoriesController.cs  # Stories & Genres API
│   └── StoryController.cs    # Story Detail MVC
├── Models/                   # Entity models
│   ├── User.cs
│   ├── Story.cs
│   ├── Chapter.cs
│   ├── Genre.cs
│   ├── StoryGenre.cs
│   └── Interactions.cs       # Favorite, ReadingHistory, Comment, Report
├── Data/
│   ├── AppDbContext.cs        # EF Core DbContext
│   └── DbInitializer.cs      # Seed data khi khởi động
├── DTOs/                     # Data Transfer Objects
├── Views/                    # Razor Views
│   ├── Home/                 # Index, Genres, Favorites
│   ├── Story/                # Detail
│   ├── Reader/               # Read
│   ├── Author/               # Index (Studio)
│   ├── Admin/                # Index (Dashboard)
│   ├── Account/              # Login
│   └── Shared/               # _Layout.cshtml
├── wwwroot/                  # Static files (CSS, JS, images)
├── Program.cs                # App configuration & startup
├── WebDocTruyen.csproj       # Project file
├── database.db               # SQLite database
├── appsettings.json          # Configuration
├── ServerCSharp.http         # REST API test cases
├── BACKLOG.md                # Product Backlog
└── docker-compose.yml        # Docker deployment
```

---

## 8. GIAO DIỆN NGƯỜI DÙNG

### 8.1. Design System

| Yếu tố | Giá trị |
|--------|--------|
| **Phong cách** | Glassmorphism + Cyber Indigo |
| **Màu chủ đạo** | `#6C63FF` (Indigo), `#00D4FF` (Cyan), `#1a1a2e` (Dark bg) |
| **Font chính** | Inter, Outfit (Google Fonts) |
| **Border radius** | 12px – 20px (rounded cards) |
| **Backdrop blur** | `backdrop-filter: blur(20px)` |
| **Dark mode** | Mặc định dark, hỗ trợ toggle light |
| **Responsive** | Mobile-first, breakpoints: 768px, 1024px |

### 8.2. Các Màn Hình Chính

| Màn hình | URL | Mô tả |
|----------|-----|-------|
| Trang chủ | `/` | Hero + Hot + Mới + Thể loại |
| Khám phá | `/Home/Genres` | Filter + Search |
| Chi tiết truyện | `/Story/Detail/{id}` | Info + Chapters + Comments |
| Đọc truyện | `/Reader/Read/{id}` | Nội dung + Dark mode + Font size |
| Tủ sách | `/Home/Favorites` | Favorites + History |
| Đăng nhập | `/Account/Login` | Form login |
| Author Studio | `/Author/Index` | Quản lý tác phẩm |
| Admin Dashboard | `/Admin/Index` | KPI + Tabs quản trị |

---

## 9. KẾ HOẠCH BÀN GIAO

### 9.1. Sprint Summary

| Sprint | Story Points | Thời gian | Trạng thái |
|--------|-------------|-----------|-----------|
| Sprint 1 | 47 SP | Tháng 10/2026 | ✅ HOÀN THÀNH |

### 9.2. Definition of Done

Một User Story được coi là **Done** khi đáp ứng đủ các tiêu chí:

- [x] Code implement đúng Acceptance Criteria trong Jira.
- [x] Build thành công — `0 Errors, 0 Warnings`.
- [x] API endpoint hoạt động đúng, kiểm tra qua Swagger / `ServerCSharp.http`.
- [x] Giao diện Razor View hiển thị đúng trên trình duyệt.
- [x] Phân quyền hoạt động đúng (kiểm tra truy cập trái phép bị chặn).
- [x] Code đã được commit và push lên GitHub `main`.
- [x] Issue trên Jira chuyển sang trạng thái **Done**.

### 9.3. Môi Trường Chạy

#### Chạy Local (Development):
```bash
cd d:\CNPMNC\cnpmnc
dotnet run --urls "http://localhost:5000"
# Truy cập: http://localhost:5000
# Swagger:  http://localhost:5000/swagger
```

#### Tài khoản mẫu (Seed Data):
| Vai trò | Email | Mật khẩu |
|---------|-------|---------|
| Admin | admin@doctruyen.vn | Admin@123 |
| Author | tacgia@doctruyen.vn | Author@123 |
| Reader | docgia@doctruyen.vn | Reader@123 |

### 9.4. Trạng Thái Bàn Giao

| Hạng mục | Trạng thái |
|----------|-----------|
| Source code (GitHub) | ✅ Đã push — branch `main` |
| Jira Backlog | ✅ 11/11 issues — Status: Done |
| Swagger API Docs | ✅ Hoạt động tại `/swagger` |
| SRS Document | ✅ File này |
| Product Backlog | ✅ `BACKLOG.md` |
| REST API Test File | ✅ `ServerCSharp.http` |
| Docker Config | ✅ `docker-compose.yml`, `Dockerfile` |
| Deploy Guide | ✅ `HUONG_DAN_DEPLOY_RENDER.md` |

---

*Tài liệu SRS này được đồng bộ với toàn bộ mã nguồn và Jira Project ITUQ.*  
*GitHub: https://github.com/nguyenluan29082006-ui/luan*  
*Jira Board: https://nguyenluan29082006.atlassian.net/jira/software/projects/ITUQ/boards*