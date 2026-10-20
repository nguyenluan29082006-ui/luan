# 📋 PRODUCT BACKLOG — Web Đọc Truyện (ITUQ)

> **Dự án:** WebDocTruyen — Nền tảng đọc truyện đa vai trò  
> **Công nghệ:** ASP.NET Core 8 MVC + REST API, SQLite, JWT, Glassmorphism UI  
> **Jira Project Key:** ITUQ  
> **Cập nhật lần cuối:** 2026-10-20  
> **Trạng thái Sprint:** ✅ Sprint 1 — HOÀN THÀNH

---

## 📊 Tổng quan Sprint

| Tổng Story Points | Hoàn thành | % Hoàn thành | Số Issues |
|:-----------------:|:----------:|:------------:|:---------:|
| **47 SP** | **47 SP** | **100%** | **11** |

---

## 🗂️ DANH SÁCH USER STORIES

---

### [ITUQ-1] API Xác thực & Quản lý Tài khoản (Auth & Identity)

| Trường | Giá trị |
|--------|---------|
| **Loại** | Story |
| **Epic** | Backend |
| **Story Points** | 5 |
| **Độ ưu tiên** | 🔴 High |
| **Trạng thái** | ✅ Done |
| **Component** | Backend |

**Mô tả:**  
Là một **người dùng** (Khách/Reader/Author/Admin), tôi muốn **đăng ký và đăng nhập** vào hệ thống để **được cấp JWT Token và truy cập các tính năng theo vai trò**.

**Acceptance Criteria:**
- [x] `POST /api/auth/register` — Đăng ký tài khoản mới với email, password, fullname
- [x] `POST /api/auth/login` — Đăng nhập, nhận JWT Bearer Token (expires 7 ngày)
- [x] `GET /api/auth/me` — Lấy thông tin profile người dùng hiện tại
- [x] `POST /api/auth/become-author` — Nâng cấp quyền Reader lên Author
- [x] Password hash bằng BCrypt, không lưu plaintext
- [x] JWT Token chứa Claims: UserId, Email, Role
- [x] Endpoint nhạy cảm được bảo vệ bằng [Authorize(Roles="...")]
- [x] Tài khoản bị khóa (Status = banned) không thể đăng nhập

---

### [ITUQ-2] API Quản lý Truyện & Thể loại (Stories & Genres)

| Trường | Giá trị |
|--------|---------|
| **Loại** | Story |
| **Epic** | Backend |
| **Story Points** | 5 |
| **Độ ưu tiên** | 🔴 High |
| **Trạng thái** | ✅ Done |
| **Component** | Backend |

**Mô tả:**  
Là một **Độc giả**, tôi muốn **tìm kiếm và duyệt truyện theo thể loại** để **nhanh chóng tìm được tác phẩm phù hợp**.

**Acceptance Criteria:**
- [x] `GET /api/stories` — Danh sách truyện với filter: genreId, search, type, status, page, pageSize
- [x] `GET /api/stories/{id}` — Chi tiết truyện kèm danh sách chương
- [x] `GET /api/stories/featured` — Top truyện nổi bật (nhiều view nhất)
- [x] `POST /api/stories` — Tác giả tạo truyện mới (status: pending)
- [x] `GET /api/genres` — Danh sách toàn bộ thể loại
- [x] Slug tự động sinh từ tên truyện (unique)
- [x] Truyện mới tạo có ApprovalStatus = pending, chưa hiển thị với độc giả
- [x] Hỗ trợ phân trang (Pagination) tránh quá tải

---

### [ITUQ-3] API Quản lý Chương & Trình đọc truyện (Chapters & Reading)

| Trường | Giá trị |
|--------|---------|
| **Loại** | Story |
| **Epic** | Backend |
| **Story Points** | 8 |
| **Độ ưu tiên** | 🔴 High |
| **Trạng thái** | ✅ Done |
| **Component** | Backend |

**Mô tả:**  
Là một **Độc giả**, tôi muốn **đọc từng chương truyện** và **điều hướng qua lại giữa các chương** để **có trải nghiệm đọc liền mạch**.

**Acceptance Criteria:**
- [x] `GET /api/chapters/{id}` — Nội dung chương, prevChapterId, nextChapterId
- [x] `POST /api/chapters` — Tác giả thêm chương mới cho truyện của mình
- [x] Tự động tăng ViewCount mỗi lần đọc chương
- [x] Tự động tăng Views của truyện mẹ
- [x] Số thứ tự chương (ChapterNumber) tự động nếu không nhập
- [x] Tác giả chỉ được thêm chương cho truyện của chính mình (hoặc Admin)

---

### [ITUQ-4] API Tủ sách & Lịch sử đọc (Favorites & Reading History)

| Trường | Giá trị |
|--------|---------|
| **Loại** | Story |
| **Epic** | Backend |
| **Story Points** | 3 |
| **Độ ưu tiên** | 🟡 Medium |
| **Trạng thái** | ✅ Done |
| **Component** | Backend |

**Mô tả:**  
Là một **Độc giả đã đăng nhập**, tôi muốn **lưu truyện vào tủ sách** và **ghi lại chương đang đọc** để **dễ dàng tiếp tục đọc sau**.

**Acceptance Criteria:**
- [x] `POST /api/favorites/toggle/{storyId}` — Thêm/Xóa khỏi tủ sách (toggle)
- [x] `GET /api/favorites` — Danh sách truyện trong tủ sách
- [x] `GET /api/favorites/check/{storyId}` — Kiểm tra truyện có trong tủ không
- [x] `POST /api/history` — Lưu chương đang đọc
- [x] `GET /api/history` — Lịch sử đọc, sort theo thời gian mới nhất
- [x] Mỗi truyện chỉ lưu 1 chương đọc dở (upsert)

---

### [ITUQ-5] API Bình luận & Báo cáo vi phạm (Comments & Reports)

| Trường | Giá trị |
|--------|---------|
| **Loại** | Story |
| **Epic** | Backend |
| **Story Points** | 3 |
| **Độ ưu tiên** | 🟡 Medium |
| **Trạng thái** | ✅ Done |
| **Component** | Backend |

**Mô tả:**  
Là một **Độc giả**, tôi muốn **bình luận dưới truyện** và **báo cáo nội dung vi phạm** để **chia sẻ cảm nghĩ và giúp hệ thống an toàn hơn**.

**Acceptance Criteria:**
- [x] `GET /api/comments/story/{storyId}` — Danh sách bình luận theo truyện
- [x] `POST /api/comments` — Gửi bình luận mới
- [x] `DELETE /api/comments/{id}` — Xóa bình luận (chủ bình luận hoặc Admin)
- [x] `POST /api/reports` — Gửi báo cáo vi phạm (comment/chapter)
- [x] Admin có thể xóa bình luận vi phạm thông qua report

---

### [ITUQ-6] API Quản trị hệ thống & Phê duyệt truyện (Admin Portal API)

| Trường | Giá trị |
|--------|---------|
| **Loại** | Story |
| **Epic** | Backend |
| **Story Points** | 5 |
| **Độ ưu tiên** | 🔴 High |
| **Trạng thái** | ✅ Done |
| **Component** | Backend |

**Mô tả:**  
Là một **Quản trị viên**, tôi muốn **kiểm duyệt truyện, quản lý tài khoản và xử lý báo cáo** để **đảm bảo chất lượng nội dung trên nền tảng**.

**Acceptance Criteria:**
- [x] `GET /api/admin/pending-stories` — Danh sách truyện chờ duyệt
- [x] `POST /api/admin/approve-story/{id}` — Phê duyệt truyện
- [x] `POST /api/admin/reject-story/{id}` — Từ chối truyện kèm lý do
- [x] `POST /api/admin/users/{id}/toggle-lock` — Khóa/mở khóa tài khoản
- [x] Admin không thể tự khóa tài khoản mình
- [x] Tất cả Admin API được bảo vệ bằng [Authorize(Roles="admin")]

---

### [ITUQ-7] API Thống kê & Bảng xếp hạng (Dashboard & Leaderboard)

| Trường | Giá trị |
|--------|---------|
| **Loại** | Story |
| **Epic** | Backend |
| **Story Points** | 5 |
| **Độ ưu tiên** | 🟡 Medium |
| **Trạng thái** | ✅ Done |
| **Component** | Backend |

**Mô tả:**  
Là một **Admin**, tôi muốn **xem số liệu tổng quan hệ thống và bảng xếp hạng** để **nắm bắt tình hình hoạt động của nền tảng**.

**Acceptance Criteria:**
- [x] Dashboard hiển thị: Tổng Users, Truyện, Chương, Lượt xem, Chờ duyệt
- [x] Top truyện nhiều lượt xem nhất (Top 10)
- [x] Top truyện được yêu thích nhất
- [x] Thống kê số lượng truyện theo thể loại

---

### [ITUQ-8] Frontend Giao diện Độc giả (Reader Web MVC)

| Trường | Giá trị |
|--------|---------|
| **Loại** | Story |
| **Epic** | Frontend |
| **Story Points** | 8 |
| **Độ ưu tiên** | 🔴 High |
| **Trạng thái** | ✅ Done |
| **Component** | Frontend |

**Mô tả:**  
Là một **Độc giả**, tôi muốn **một giao diện đẹp, responsive để đọc truyện** bao gồm **trang chủ, tìm kiếm, chi tiết truyện và trình đọc**.

**Acceptance Criteria:**
- [x] Trang chủ Home/Index — Hero banner, Top truyện hot, Truyện mới cập nhật, Danh mục thể loại
- [x] Trang khám phá Home/Genres — Tìm kiếm, lọc theo thể loại/loại/tình trạng
- [x] Trang chi tiết truyện Story/Detail — Bìa truyện, mô tả, danh sách chương, bình luận
- [x] Trình đọc truyện Reader/Read — Nội dung chương, điều hướng Prev/Next, Dark/Light mode, tùy chỉnh cỡ chữ
- [x] Trang tủ sách Home/Favorites — Danh sách truyện yêu thích + Lịch sử đọc
- [x] Giao diện Glassmorphism, Dark mode, responsive mobile
- [x] Tự động lưu lịch sử đọc khi chuyển chương

---

### [ITUQ-9] Frontend Studio Tác giả (Author Studio)

| Trường | Giá trị |
|--------|---------|
| **Loại** | Story |
| **Epic** | Frontend |
| **Story Points** | 5 |
| **Độ ưu tiên** | 🟡 Medium |
| **Trạng thái** | ✅ Done |
| **Component** | Frontend |

**Mô tả:**  
Là một **Tác giả**, tôi muốn **một studio sáng tác trực quan** để **tạo truyện mới, đăng tải chương và theo dõi tác phẩm của mình**.

**Acceptance Criteria:**
- [x] Dashboard tác giả Author/Index — Danh sách truyện của mình với stats
- [x] Form tạo truyện mới — Tên, mô tả, ảnh bìa, thể loại, loại hình
- [x] Form thêm chương — Tiêu đề chương, số thứ tự, nội dung soạn thảo
- [x] Xóa truyện (chỉ truyện của mình)
- [x] Hiển thị trạng thái kiểm duyệt: Pending / Approved / Rejected + lý do từ chối
- [x] Chỉ Author và Admin mới truy cập được Studio

---

### [ITUQ-10] Frontend Cổng Quản trị Admin (Admin Dashboard UI)

| Trường | Giá trị |
|--------|---------|
| **Loại** | Story |
| **Epic** | Frontend |
| **Story Points** | 5 |
| **Độ ưu tiên** | 🔴 High |
| **Trạng thái** | ✅ Done |
| **Component** | Frontend |

**Mô tả:**  
Là một **Admin**, tôi muốn **một bảng điều khiển quản trị trực quan** để **duyệt truyện, quản lý người dùng và xử lý vi phạm nhanh chóng**.

**Acceptance Criteria:**
- [x] Tổng quan Dashboard — 6 KPI cards (Users, Truyện, Chương, Views, Chờ duyệt, Reports)
- [x] Tab "Duyệt Truyện" — Danh sách truyện Pending, nút Approve/Reject 1-click kèm lý do
- [x] Tab "Quản lý Users" — Danh sách users, đổi role, ban/unban tài khoản
- [x] Tab "Báo cáo vi phạm" — Danh sách reports, Resolve (xóa nội dung) hoặc Dismiss
- [x] Tab "Quản lý Thể loại" — Thêm/Xóa thể loại
- [x] Flash message thông báo kết quả thao tác
- [x] Chỉ Admin mới truy cập được Dashboard

---

### [ITUQ-11] Kiểm thử tích hợp RESTful API & Viết Swagger Docs

| Trường | Giá trị |
|--------|---------|
| **Loại** | Task |
| **Epic** | Testing / QA |
| **Story Points** | 3 |
| **Độ ưu tiên** | 🟢 Low |
| **Trạng thái** | ✅ Done |
| **Component** | Testing |

**Mô tả:**  
Là một **thành viên nhóm**, tôi muốn **tài liệu hóa và kiểm thử đầy đủ toàn bộ API** để **đảm bảo chất lượng và dễ dàng demo với giảng viên**.

**Acceptance Criteria:**
- [x] File ServerCSharp.http — Test cases cho tất cả 30+ endpoints
- [x] Swagger UI tại /swagger — Mô tả đầy đủ API với Bearer Auth
- [x] Test flow: Register → Login → Get token → Call protected APIs
- [x] Test phân quyền: Reader / Author / Admin
- [x] API response time < 300ms cho các endpoint đọc dữ liệu

---

## 📈 Velocity & Definition of Done

### Definition of Done
Một Story được coi là Done khi:
1. Code đã được implement đầy đủ theo Acceptance Criteria
2. Không có lỗi build (0 Errors)
3. API endpoint hoạt động đúng (test qua Swagger/HTTP file)
4. Giao diện hiển thị đúng trên trình duyệt
5. Code đã được commit lên GitHub

### Sprint Velocity
Sprint 1: 47 Story Points — COMPLETED

---

*Tài liệu này được đồng bộ với Jira Project: https://nguyenluan29082006.atlassian.net/jira/software/projects/ITUQ/boards*
