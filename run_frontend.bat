@echo off
title WebDocTruyen - Frontend (MVC Web :5001)
cd /d "%~dp0"
echo ========================================================
echo   KHOI DONG FRONTEND WEB MVC (ASP.NET CORE 8 - C#)
echo   Dia chi Web UI: http://localhost:5001
echo ========================================================
dotnet run --project Frontend/WebDocTruyen.Frontend.csproj --urls "http://localhost:5001"
pause
