# KỊCH BẢN THUYẾT TRÌNH BÁO CÁO DÀNH CHO NHÓM TRƯỞNG
## Môn: Công Nghệ Phần Mềm Nâng Cao (CNPMNC)
**Dự án:** Hệ thống Website Đọc Truyện Trực Tuyến (WebDocTruyen)  
**Nhóm:** B07 - 5linhdanhthue (Mã Project Jira: ITUQ)  

---

## 🎯 1. BƯỚC CHUẨN BỊ TRƯỚC KHI BƯỚC LÊN BÀN BÁO CÁO
Mở sẵn trên trình duyệt 6 tab theo đúng thứ tự:
- **Tab 1:** Link Jira Board (Dự án ITUQ - Bảng Sprint & Backlog).
- **Tab 2:** Link GitHub Repository của nhóm (Tab Code và Commit History).
- **Tab 3:** File tài liệu [SRS_WebDocTruyen.md](file:///d:/CNPMNC/cnpmnc/SRS_WebDocTruyen.md) (hoặc xuất ra file PDF).
- **Tab 4:** Swagger UI Backend: `http://localhost:5000/swagger` (hoặc link VPS port 5000).
- **Tab 5:** Frontend Giao diện Web: `http://localhost:5001` (hoặc link VPS port 5001).
- **Tab 6:** Terminal/VPS hoặc Docker running status.

---

## 🗣️ 2. KỊCH BẢN NÓI CHI TIẾT (THEO 7 YÊU CẦU CỦA CÔ)

### 🔹 LỜI CHÀO & MỞ ĐẦU (30 giây)
> *"Em chào Thầy/Cô và bạn Trợ giáo. Em là [Tên bạn], đại diện Nhóm B07 - 5linhdanhthue xin phép báo cáo tiến độ đồ án Hệ thống Web Đọc Truyện Trực Tuyến được xây dựng 100% bằng nền tảng C# và .NET 8. Hôm nay nhóm em đã chuẩn bị đầy đủ 7 hạng mục theo yêu cầu: Backlog, Jira, SRS, GitHub, Backend, Frontend và Môi trường triển khai VPS."*

---

### 🔹 PHẦN 1 & 2: BACKLOG & JIRA (Mở Tab 1 - Jira)
> *"Đầu tiên, về quy trình Agile/Scrum và quản lý công việc:*
> - *Nhóm em đã phân rã toàn bộ hệ thống thành **11 User Stories và Tasks** trên Jira với Project Key là **ITUQ**.*
> - *Mỗi User Story đều được gán rõ Story Points (từ 3 đến 8 điểm), mức độ ưu tiên Priority và Component phân hệ (Backend, Frontend, Testing).*
> - *Ở Sprint hiện tại, nhóm đã hoàn thành toàn bộ các Story cốt lõi từ Xác thực JWT, Quản lý Truyện/Chương, Tủ sách cá nhân, Bình luận cho đến Cổng Admin phê duyệt.*
> - *Toàn bộ công việc đều có phân công thành viên phụ trách rõ ràng trên bảng Scrum Board."*

---

### 🔹 PHẦN 3: TÀI LIỆU ĐẶC TẢ SRS (Mở Tab 3 - File SRS)
> *"Thứ hai, về tài liệu đặc tả yêu cầu phần mềm (SRS):*
> - *Nhóm đã hoàn thiện tài liệu SRS theo chuẩn IEEE 830, chia làm 3 tác nhân chính: **Độc giả (Reader)**, **Tác giả (Author)** và **Quản trị viên (Admin)**.*
> - *Hệ thống gồm 11 nhóm yêu cầu chức năng (FR-01 đến FR-11) khớp 100% với Product Backlog trên Jira.*
> - *Về yêu cầu phi chức năng: Nhóm thiết kế bảo mật bằng JWT Bearer Token, mật khẩu băm BCrypt, cơ chế kiểm soát phân quyền Role-based và tốc độ phản hồi API dưới 300ms.*
> - *CSDL được thiết kế chuẩn hóa gồm 8 bảng thực thể liên kết chặt chẽ bằng khóa ngoại."*

---

### 🔹 PHẦN 4: REPO GITHUB & QUẢN LÝ CODE (Mở Tab 2 - GitHub)
> *"Thứ ba, về kho mã nguồn GitHub:*
> - *Mã nguồn dự án được lưu trữ tại Repository của nhóm.*
> - *Cấu trúc source code được chia thành 2 phân hệ độc lập: thư mục `/Backend` (ASP.NET Core Web API) và `/Frontend` (ASP.NET Core MVC).*
> - *Nhóm có file `.gitignore` loại trừ các file rác build, có lịch sử commit rõ ràng và file README hướng dẫn chạy 1-click."*

---

### 🔹 PHẦN 5: BACKEND (BE) & RESTFUL API (Mở Tab 4 - Swagger UI)
> *"Thứ tư, về phân hệ Backend:*
> - *Backend được viết bằng **ASP.NET Core 8 Web API** kết hợp **Entity Framework Core 8** và SQLite Database.*
> - *Gồm **10 API Controllers** xử lý toàn bộ nghiệp vụ: Auth, Stories, Chapters, Comments, Interactions, Admin và Thống kê Stats.*
> - *Hệ thống đã tích hợp Swagger UI tài liệu hóa trực quan và hỗ trợ test Authorization trực tiếp bằng Bearer Token.*
> - *(Bấm thao tác thử 1 API như `GET /api/stories` hoặc `GET /api/stats/dashboard` trên Swagger để trợ giáo thấy dữ liệu JSON trả về ngay lập tức).*"

---

### 🔹 PHẦN 6: FRONTEND (FE) (Mở Tab 5 - Web UI)
> *"Thứ năm, về phân hệ Frontend:*
> - *Frontend được thiết kế bằng **ASP.NET Core MVC (Razor Views)** kết hợp CSS hiện đại Cyber Indigo & Dark Glassmorphism, chuẩn responsive cho cả máy tính và điện thoại.*
> - *Hệ thống có sẵn tính năng chuyển vai trò 1-click để demo nhanh 3 quyền:*
>   1. **Độc giả:** Có thể tìm kiếm truyện, lọc thể loại, đọc chương truyện với điều hướng Trước/Sau, lưu vào Tủ sách yêu thích và bình luận.
>   2. **Tác giả:** Có giao diện **Author Studio** để tạo truyện mới và thêm nội dung chương.
>   3. **Quản trị viên:** Có giao diện **Admin Dashboard** để xem chỉ số thống kê hệ thống, danh sách kiểm duyệt truyện (`Approve`/`Reject`) và khóa tài khoản vi phạm.*"

---

### 🔹 PHẦN 7: TRIỂN KHAI DEPLOY VPS (Mở Tab 6)
> *"Cuối cùng, về triển khai hệ thống:*
> - *Nhóm đã container hóa toàn bộ hệ thống bằng **Docker và Docker Compose** (`docker-compose.yml`) với multi-stage build tối ưu kích thước.*
> - *Hệ thống đã được cấu hình sẵn sàng chạy độc lập trên máy chủ VPS, phân tách cổng 5000 (Backend API) và cổng 5001 (Frontend Web).*
> - *Đây là link public đang hoạt động trực tiếp, bạn trợ giáo có thể mở máy hoặc điện thoại để truy cập và kiểm tra ngay lập tức."*

---

## ❓ 3. CÁC CÂU HỎI TRỢ GIÁO HAY HỎI XOÁY VÀ CÁCH ĐÁP

| Câu hỏi của Trợ giáo | Cách trả lời chuẩn điểm tối đa |
| :--- | :--- |
| **Q1: "Tại sao nhóm chọn kiến trúc tách riêng Backend API và Frontend MVC thay vì viết chung monolithic?"** | *"Dạ thưa bạn, việc tách riêng REST API giúp hệ thống chuẩn hóa kiến trúc hướng dịch vụ (Service-Oriented). Sau này ngoài Web MVC, nhóm có thể dễ dàng mở rộng thêm ứng dụng Mobile App (Flutter/React Native) dùng chung 1 Backend API mà không cần sửa đổi CSDL hay logic nghiệp vụ."* |
| **Q2: "Hệ thống bảo mật mật khẩu và phân quyền thế nào?"** | *"Dạ mật khẩu người dùng được băm an toàn bằng thuật toán BCrypt không thể giải mã ngược. Khi đăng nhập thành công, Backend cấp phát mã JWT Bearer Token chứa Claims về vai trò (`Reader`, `Author`, `Admin`). Các API quan trọng đều được kiểm tra nghiêm ngặt qua Attribute `[Authorize(Roles = "...")]`."* |
| **Q3: "Tại sao dùng SQLite thay vì SQL Server?"** | *"Dạ SQLite là CSDL quan hệ chuẩn, lưu trữ dưới dạng một file duy nhất (`database.db`), giúp hệ thống khởi chạy tức thì, sao lưu dữ liệu toàn vẹn và triển khai lên Docker/VPS cực kỳ gọn nhẹ mà vẫn đáp ứng đầy đủ cú pháp Entity Framework Core và ACID."* |
| **Q4: "Làm sao biết truyện nào đã duyệt mới được hiển thị cho độc giả?"** | *"Dạ trong Entity `Story` có trường `Status`. Khi tác giả tạo mới, truyện ở trạng thái `Pending`. API danh sách truyện cho độc giả luôn lọc điều kiện `Where(s => s.Status == StoryStatus.Approved)`. Chỉ khi Admin bấm Approve trên trang quản trị thì truyện mới xuất hiện ở trang chủ."* |
