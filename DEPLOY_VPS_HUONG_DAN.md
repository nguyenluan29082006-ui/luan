# HƯỚNG DẪN TRIỂN KHAI HỆ THỐNG (DEPLOY LÊN VPS & LIVE DEMO)

Tài liệu này cung cấp 2 giải pháp hoàn chỉnh để bạn đáp ứng yêu cầu **"Deploy lên VPS để bạn trợ giáo kiểm tra"**:

---

## CÁCH 1: TRIỂN KHAI CHÍNH THỨC LÊN MÁY CHỦ VPS (LINUX / UBUNTU)

Nếu nhóm bạn đã có VPS (hoặc mua VPS giá rẻ như CloudFly, Vietnix, Linode, DigitalOcean, Oracle Cloud):

### Bước 1: Cài đặt Docker & Git trên VPS (nếu chưa có)
```bash
sudo apt update && sudo apt install -y docker.io docker-compose git
sudo systemctl enable docker
sudo systemctl start docker
```

### Bước 2: Kéo mã nguồn về VPS
```bash
git clone <LINK_REPO_GITHUB_CUA_BAN>
cd cnpmnc
```

### Bước 3: Khởi chạy toàn bộ hệ thống bằng 1 lệnh duy nhất
```bash
docker compose up -d --build
```

### Bước 4: Mở Firewall trên VPS cho 2 cổng 5000 và 5001
```bash
sudo ufw allow 5000/tcp
sudo ufw allow 5001/tcp
```

👉 **Địa chỉ bàn giao cho Trợ giáo:**
- **Frontend Website:** `http://<IP_VPS_CUA_BAN>:5001`
- **Backend Swagger:** `http://<IP_VPS_CUA_BAN>:5000/swagger`

---

## CÁCH 2: GIẢI PHÁP "CỨU CÁNH" KHẨN CẤP QUA TUNNEL (NẾU CHƯA CÓ VPS)

Nếu nhóm **chưa kịp thuê VPS** hoặc VPS chưa cấu hình kịp, bạn có thể tạo đường hầm (Public Tunnel) từ máy tính local của bạn ra Internet. Trợ giáo dùng 4G hoặc máy tính của trợ giáo vẫn bấm vào link test bình thường!

### Lựa chọn A: Dùng Ngrok (Rất phổ biến, miễn phí)
1. Tải ngrok từ [ngrok.com](https://ngrok.com) hoặc cài bằng terminal:
   ```powershell
   winget install ngrok
   ```
2. Đăng ký tài khoản miễn phí trên ngrok để lấy AuthToken:
   ```bash
   ngrok config add-authtoken <TOKEN_CUA_BAN>
   ```
3. Chạy hệ thống local qua file `run_all.bat`.
4. Mở cửa sổ cmd mới và gõ lệnh mở cổng Frontend:
   ```bash
   ngrok http 5001
   ```
5. Ngrok sẽ sinh ra 1 link HTTPS (ví dụ: `https://abc-xyz.ngrok-free.app`). Bật link này lên điện thoại hoặc gửi cho trợ giáo test trực tiếp!

### Lựa chọn B: Dùng Cloudflare Tunnel (Miễn phí 100%, không cần đăng ký tài khoản)
Chạy lệnh bằng PowerShell:
```powershell
winget install Cloudflare.cloudflared
cloudflared tunnel --url http://localhost:5001
```
Cloudflare sẽ tạo ngay một link HTTPS công khai dạng `https://...trycloudflare.com`.
