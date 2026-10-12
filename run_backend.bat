@echo off
title WebDocTruyen - Backend (Web API :5000)
cd /d "%~dp0"
echo ========================================================
echo   KHOI DONG BACKEND WEB API (ASP.NET CORE 8 - C#)
echo   Dia chi API: http://localhost:5000
echo   Swagger UI : http://localhost:5000/swagger
echo ========================================================
dotnet run --project Backend/WebDocTruyen.Backend.csproj --urls "http://localhost:5000"
pause
