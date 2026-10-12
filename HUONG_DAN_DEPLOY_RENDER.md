# HƯỚNG DẪN DEPLOY WEB ĐỌC TRUYỆN LÊN CLOUD MIỄN PHÍ (RENDER.COM)

> **Mục tiêu**: Đưa website chạy 24/7 trên Internet với domain HTTPS bảo mật hoàn toàn miễn phí, ai cũng có thể bấm vào link để đọc truyện bất kỳ lúc nào mà không cần bạn bật máy tính!

---

## 🚀 TẠI SAO CHỌN RENDER.COM?
1. **Miễn phí 100%**: Không cần thẻ tín dụng (No Credit Card required).
2. **Tự động liên kết GitHub**: Render sẽ tự động build từ repository `nguyenluan29082006-ui/luan`.
3. **Có sẵn link HTTPS**: Được cấp domain miễn phí dạng `https://...onrender.com`.

---

## 🛠️ CÁCH TRIỂN KHAI (CHỈ MẤT 3 PHÚT)

### BƯỚC 1: ĐĂNG NHẬP VÀO RENDER
1. Truy cập vào website: [https://render.com](https://render.com)
2. Bấm nút **GET STARTED** hoặc **Sign In**.
3. Chọn **Sign in with GitHub** và ủy quyền bằng tài khoản GitHub `nguyenluan29082006-ui`.

---

### BƯỚC 2: DEPLOY BACKEND (REST API & SWAGGER)
1. Tại Dashboard Render, bấm nút **New +** ở góc trên bên phải -> Chọn **Web Service**.
2. Chọn mục **Build and deploy from a Git repository** -> Bấm **Next**.
3. Chọn repository **`luan`** (hoặc `nguyenluan29082006-ui/luan`).
4. Điền các thông tin như sau:
   - **Name**: `webdoctruyen-backend`
   - **Region**: `Singapore (Southeast Asia)` *(cho tốc độ truy cập tại VN nhanh nhất)*
   - **Language**: `Docker`
   - **Dockerfile Path**: `Dockerfile.backend`
   - **Instance Type**: Chọn gói **Free** ($0/month)
5. Kéo xuống phần **Environment Variables**, bấm **Add Environment Variable**:
   - `ASPNETCORE_URLS` = `http://+:5000`
   - `ASPNETCORE_ENVIRONMENT` = `Production`
6. Bấm nút **Create Web Service**.
7. Chờ 1-2 phút để Render build xong. Sau khi hoàn tất, bạn sẽ nhận được một đường link Backend dạng:
   👉 `https://webdoctruyen-backend-xxxx.onrender.com`
   *(Bạn có thể vào `/swagger` ở cuối link để xem tài liệu API)*.

---

### BƯỚC 3: DEPLOY FRONTEND (GIAO DIỆN WEB ĐỌC TRUYỆN)
1. Bấm lại nút **New +** -> Chọn **Web Service**.
2. Tiếp tục chọn repository **`luan`**.
3. Điền các thông tin:
   - **Name**: `webdoctruyen` *(hoặc tên bạn thích)*
   - **Region**: `Singapore (Southeast Asia)`
   - **Language**: `Docker`
   - **Dockerfile Path**: `Dockerfile.frontend`
   - **Instance Type**: Chọn gói **Free** ($0/month)
4. Kéo xuống phần **Environment Variables**, thêm 3 biến sau:
   - `ASPNETCORE_URLS` = `http://+:5001`
   - `ASPNETCORE_ENVIRONMENT` = `Production`
   - `BackendApi__BaseUrl` = *(Dán link của Backend bạn vừa tạo ở Bước 2 vào đây, ví dụ: `https://webdoctruyen-backend-xxxx.onrender.com`)*
5. Bấm nút **Create Web Service**.
6. Render sẽ build và chạy website. Sau 1-2 phút, bạn sẽ có đường link chính thức của website:
   👉 **`https://webdoctruyen.onrender.com`**

---

## 🎉 BÀN GIAO VÀ KIỂM TRA
- Gửi đường link `https://webdoctruyen.onrender.com` cho bạn bè, trợ giáo hoặc giảng viên.
- Mọi người dùng điện thoại hay laptop đều có thể truy cập, đọc truyện, đăng ký tài khoản, đăng nhập bình thường!
- **Tài khoản dùng thử**:
  - Admin: `admin` / `admin123`
  - Độc giả: `user1` / `user123`
