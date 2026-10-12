param (
    [string]$JiraDomain = "nguyenluan29082006.atlassian.net",
    [string]$Email = "nguyenluan29082006@gmail.com",
    [string]$ProjectKey = "ITUQ",
    [string]$CsvPath = "$PSScriptRoot\jira_tasks_import.csv",
    [string]$ApiToken = ""
)

if (-not (Test-Path $CsvPath)) {
    Write-Error "Không tìm thấy file CSV tại: $CsvPath"
    exit 1
}

if ([string]::IsNullOrWhiteSpace($ApiToken)) {
    Write-Host "==========================================================" -ForegroundColor Cyan
    Write-Host " ĐẨY TIẾN TRÌNH LÊN JIRA (Nhóm: 5linhdanhthue - Dự án: $ProjectKey)" -ForegroundColor Cyan
    Write-Host "==========================================================" -ForegroundColor Cyan
    Write-Host "Để tự động đẩy qua Jira API, bạn cần 1 Atlassian API Token:" -ForegroundColor Yellow
    Write-Host "1. Mở link: https://id.atlassian.com/manage-profile/security/api-tokens" -ForegroundColor Gray
    Write-Host "2. Bấm 'Create API token' -> Đặt tên bất kỳ (vd: JiraPush) -> Copy mã token" -ForegroundColor Gray
    Write-Host ""
    $ApiToken = Read-Host "Nhập API Token của bạn (paste vào đây)" -AsSecureString
    $BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($ApiToken)
    $ApiToken = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)
}

if ([string]::IsNullOrWhiteSpace($ApiToken)) {
    Write-Error "API Token không được để trống!"
    exit 1
}

# Tạo Base64 Auth Header
$authString = "${Email}:${ApiToken}"
$authBytes = [System.Text.Encoding]::UTF8.GetBytes($authString)
$base64Auth = [System.Convert]::ToBase64String($authBytes)

$headers = @{
    "Authorization" = "Basic $base64Auth"
    "Content-Type"  = "application/json"
    "Accept"        = "application/json"
}

Write-Host "`nĐang đọc danh sách công việc từ: $CsvPath..." -ForegroundColor Green
$tasks = Import-Csv -Path $CsvPath -Encoding UTF8

# Lấy danh sách Sprint hiện tại để gán tự động
$sprintId = $null
try {
    $boardUrl = "https://$JiraDomain/rest/agile/1.0/board"
    $boards = Invoke-RestMethod -Uri $boardUrl -Method Get -Headers $headers -ErrorAction SilentlyContinue
    $targetBoard = $boards.values | Where-Object { $_.location.projectKey -eq $ProjectKey } | Select-Object -First 1
    if ($targetBoard) {
        $sprintsUrl = "https://$JiraDomain/rest/agile/1.0/board/$($targetBoard.id)/sprint"
        $sprints = Invoke-RestMethod -Uri $sprintsUrl -Method Get -Headers $headers -ErrorAction SilentlyContinue
        $activeSprint = $sprints.values | Where-Object { $_.state -in @("active", "future") } | Select-Object -First 1
        if ($activeSprint) {
            $sprintId = $activeSprint.id
            Write-Host "Tìm thấy Sprint: $($activeSprint.name) (ID: $sprintId)" -ForegroundColor Cyan
        }
    }
} catch {
    Write-Host "Bỏ qua bước lấy Sprint tự động (sẽ tạo trực tiếp vào Backlog)." -ForegroundColor Gray
}

$createdKeys = @()

foreach ($item in $tasks) {
    Write-Host "Đang tạo: $($item.Summary)..." -NoNewline

    $issueType = if ($item."Issue Type" -eq "Task") { "Task" } else { "Story" }
    
    # Chuẩn bị payload chuẩn Atlassian Document Format (ADF) v3
    $body = @{
        fields = @{
            project = @{
                key = $ProjectKey
            }
            summary = $item.Summary
            issuetype = @{
                name = $issueType
            }
            description = @{
                type = "doc"
                version = 1
                content = @(
                    @{
                        type = "paragraph"
                        content = @(
                            @{
                                type = "text"
                                text = $item.Description
                            }
                        )
                    }
                )
            }
        }
    } | ConvertTo-Json -Depth 10

    try {
        $response = Invoke-RestMethod -Uri "https://$JiraDomain/rest/api/3/issue" -Method Post -Headers $headers -Body ([System.Text.Encoding]::UTF8.GetBytes($body))
        Write-Host " [Thành công -> $($response.key)]" -ForegroundColor Green
        $createdKeys += $response.key
    } catch {
        Write-Host " [Lỗi: $($_.Exception.Message)]" -ForegroundColor Red
    }
}

# Nếu có Sprint, di chuyển các issue mới tạo vào Sprint
if ($sprintId -and $createdKeys.Count -gt 0) {
    Write-Host "`nĐang đưa $($createdKeys.Count) task vào Sprint 1..." -ForegroundColor Cyan
    try {
        $moveBody = @{
            issues = $createdKeys
        } | ConvertTo-Json
        Invoke-RestMethod -Uri "https://$JiraDomain/rest/agile/1.0/sprint/$sprintId/issue" -Method Post -Headers $headers -Body $moveBody | Out-Null
        Write-Host "Đã chuyển thành công toàn bộ task vào Sprint 1!" -ForegroundColor Green
    } catch {
        Write-Host "Không thể tự động gán vào Sprint, task đã nằm ở Backlog." -ForegroundColor Yellow
    }
}

Write-Host "`n==========================================================" -ForegroundColor Green
Write-Host " HOÀN TẤT! Đã đẩy $($createdKeys.Count) công việc lên Jira thành công!" -ForegroundColor Green
Write-Host " Xem ngay tại: https://$JiraDomain/jira/your-work" -ForegroundColor Cyan
Write-Host "==========================================================" -ForegroundColor Green
