# Hệ Thống Web Đọc Truyện (100% Thuần C# .NET 8)

Dự án được phân tách rõ ràng thành hai phân hệ độc lập **Frontend** và **Backend** theo chuẩn kiến trúc phần mềm nâng cao, sử dụng duy nhất nền tảng **C# và .NET 8**.

---

## 1. Cấu Trúc Dự Án

```
c:\cnpmnc\
├── WebDocTruyen.sln             # Solution .NET kết nối Backend & Frontend
├── WebDocTruyen.slnx            # Solution format mới của .NET
│
├── Backend/                     # [PHÂN HỆ BACKEND - REST API & DATABASE]
│   ├── Controllers/             # Các API Controller (Stories, Chapters, Auth, Admin, ...)
│   ├── Data/                    # AppDbContext, DbInitializer (Entity Framework Core 8)
│   ├── Models/                  # Entity Models (User, Story, Chapter, Genre, ...)
│   ├── DTOs/                    # Data Transfer Objects
│   ├── database.db              # SQLite Database
│   ├── Program.cs               # Cấu hình Swagger, CORS, JWT, EF Core (Port 5000)
│   └── WebDocTruyen.Backend.csproj
│
├── Frontend/                    # [PHÂN HỆ FRONTEND - GIAO DIỆN WEB C# MVC]
│   ├── Controllers/             # MVC Controllers điều hướng và render Views
│   ├── Views/                   # Razor Views (.cshtml) cho Độc giả, Tác giả, Quản trị viên
│   ├── wwwroot/                 # CSS Design System Cyber Indigo & Glassmorphism
│   ├── Program.cs               # Cấu hình MVC, Cookie Auth, Session (Port 5001)
│   └── WebDocTruyen.Frontend.csproj
│
├── run_backend.bat              # Chạy riêng Backend (Cổng 5000)
├── run_frontend.bat             # Chạy riêng Frontend (Cổng 5001)
└── run_all.bat                  # 1-Click chạy đồng thời cả Backend và Frontend
```

---

## 2. Cách Khởi Chạy Nhanh

### Cách 1: Sử dụng Script 1-Click (Khuyên dùng)
Nhấp đúp chuột vào file **`run_all.bat`** tại thư mục gốc `c:\cnpmnc`.
Hệ thống sẽ tự động bật cả Backend, Frontend và mở trình duyệt web.

### Cách 2: Chạy qua Terminal / Lệnh dotnet
Mở 2 cửa sổ terminal:
- **Terminal 1 (Backend)**:
  ```bash
  dotnet run --project Backend/WebDocTruyen.Backend.csproj --urls "http://localhost:5000"
  ```
- **Terminal 2 (Frontend)**:
  ```bash
  dotnet run --project Frontend/WebDocTruyen.Frontend.csproj --urls "http://localhost:5001"
  ```

---

## 3. Địa Chỉ Truy Cập

| Phân hệ | Cổng | Đường dẫn | Chức năng |
| :--- | :---: | :--- | :--- |
| **Frontend Web** | `5001` | [http://localhost:5001](http://localhost:5001) | Giao diện đọc truyện, Studio tác giả, Cổng Admin |
| **Backend API** | `5000` | [http://localhost:5000](http://localhost:5000) | Tự động chuyển hướng đến Swagger UI |
| **Swagger UI** | `5000` | [http://localhost:5000/swagger](http://localhost:5000/swagger) | Thử nghiệm trực quan các RESTful API |

---

## 4. Tài Khoản Demo Sẵn Có

Hệ thống đã nạp sẵn dữ liệu mẫu cho cả 3 vai trò (có thể bấm chuyển nhanh 1-click ngay trên thanh Menu hoặc đăng nhập):

| Vai trò | Email | Mật khẩu | Quyền hạn |
| :--- | :--- | :--- | :--- |
| **Quản trị viên (Admin)** | `admin@doctruyen.vn` | `admin123` | Quản trị hệ thống, duyệt truyện, xử lý báo cáo vi phạm |
| **Tác giả (Author)** | `tacgia@doctruyen.vn` | `author123` | Đăng truyện mới, quản lý chương, theo dõi trạng thái duyệt |
| **Độc giả (Reader)** | `docgia@doctruyen.vn` | `reader123` | Đọc truyện, tùy biến giao diện đọc, lưu tủ truyện yêu thích, bình luận |

---

## 5. Tài Liệu API & Kiểm Thử RESTful
- Chi tiết toàn bộ 10 module API: Xem file [`API_DOCUMENTATION.md`](file:///c:/cnpmnc/API_DOCUMENTATION.md).
- File test nhanh 1-click (REST Client / VS Code): [`ServerCSharp.http`](file:///c:/cnpmnc/ServerCSharp.http).
- Giao diện Swagger UI trực quan: [http://localhost:5000/swagger](http://localhost:5000/swagger).

