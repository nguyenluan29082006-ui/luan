@echo off
title WebDocTruyen - Khoi Dong Toan Bo He Thong
cd /d "%~dp0"
echo ========================================================
echo   HE THONG WEB DOC TRUYEN (100%% THUAN C# .NET 8)
echo   Dang khoi dong dong thoi Backend va Frontend...
echo ========================================================
start "WebDocTruyen - Backend (Port 5000)" cmd /k "dotnet run --project Backend/WebDocTruyen.Backend.csproj --urls http://localhost:5000"
ping 127.0.0.1 -n 4 >nul
start "WebDocTruyen - Frontend (Port 5001)" cmd /k "dotnet run --project Frontend/WebDocTruyen.Frontend.csproj --urls http://localhost:5001"
ping 127.0.0.1 -n 3 >nul
echo Dang mo trinh duyet...
start http://localhost:5001
start http://localhost:5000/swagger
echo ========================================================
echo   HE THONG DA KHOI CHAY THANH CONG!
echo   - Backend REST API / Swagger: http://localhost:5000/swagger
echo   - Frontend Giao Dien Web   : http://localhost:5001
echo ========================================================
