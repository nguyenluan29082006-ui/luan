#!/bin/sh
set -e

# Đảm bảo thư mục data tồn tại
mkdir -p /app/data

# Khởi tạo database nếu chưa có
if [ ! -f /app/data/database.db ]; then
  cp /app/backend/database.db /app/data/database.db 2>/dev/null || true
fi

# Chạy Backend ngầm trên cổng 5000 (để phục vụ API nội bộ)
cd /app/backend && dotnet WebDocTruyen.Backend.dll --urls "http://127.0.0.1:5000" &

# Đợi Backend khởi động
sleep 2

# Chạy Frontend trên cổng được Render cấp qua biến PORT (mặc định 10000)
PORT_NUM="${PORT:-10000}"
export ASPNETCORE_URLS="http://0.0.0.0:${PORT_NUM}"
export ConnectionStrings__DefaultConnection="Data Source=/app/data/database.db"
export BackendApi__BaseUrl="http://127.0.0.1:5000"

echo "WebDocTruyen is starting on port ${PORT_NUM}..."
cd /app/frontend && dotnet WebDocTruyen.Frontend.dll
