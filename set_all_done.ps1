# ============================================================
#  set_all_done.ps1
#  Chuyen tat ca 11 Issues ITUQ sang trang thai "Done"
#  Transition ID = 31 (Done)
# ============================================================

$JIRA_URL  = "https://nguyenluan29082006.atlassian.net"
$EMAIL     = "nguyenluan29082006@gmail.com"
$API_TOKEN = "ATATT3xFfGF0cEk1CgEUJYYLGF462ecYIQt0Q3CwaUZlmaRkmvsEmuNah8v8khard3EYszNb00FhbZnlc6Z8pSLVWHaWK-BjuRBAqGeug54WCR2uJ81NA1Obb2fE1lplj8AHH-P9TBfhiXnY6npesHCTdwnXwkSFiQo8-lDvmC4gOGnzEfgIpnI=FC0D56C7"

$bytes   = [System.Text.Encoding]::UTF8.GetBytes("${EMAIL}:${API_TOKEN}")
$AUTH    = "Basic " + [Convert]::ToBase64String($bytes)
$HEADERS = @{ "Authorization" = $AUTH; "Content-Type" = "application/json" }

# Transition ID = 31 = "Done" (da xac nhan qua debug)
$DONE_TRANSITION_ID = "31"

$keys = @("ITUQ-1","ITUQ-2","ITUQ-3","ITUQ-4","ITUQ-5","ITUQ-6","ITUQ-7","ITUQ-8","ITUQ-9","ITUQ-10","ITUQ-11")

Write-Host "`n===== CHUYEN TAT CA ISSUES SANG DONE =====" -ForegroundColor Cyan

$ok = 0; $fail = 0

foreach ($key in $keys) {
    # Kiem tra trang thai hien tai
    try {
        $issue = Invoke-RestMethod -Uri "$JIRA_URL/rest/api/3/issue/$key" -Method Get -Headers $HEADERS -ErrorAction Stop
        $currentStatus = $issue.fields.status.name

        if ($currentStatus -eq "Done") {
            Write-Host "  [SKIP] $key - Da la Done roi" -ForegroundColor DarkGray
            $ok++
            continue
        }

        Write-Host "  $key (hien tai: $currentStatus) -> Done..." -NoNewline

        # Goi transition
        $body = @{ transition = @{ id = $DONE_TRANSITION_ID } } | ConvertTo-Json -Depth 3
        Invoke-RestMethod -Uri "$JIRA_URL/rest/api/3/issue/$key/transitions" -Method Post -Headers $HEADERS -Body $body -ErrorAction Stop | Out-Null

        # Xac nhan lai
        Start-Sleep -Milliseconds 300
        $check = Invoke-RestMethod -Uri "$JIRA_URL/rest/api/3/issue/$key" -Method Get -Headers $HEADERS -ErrorAction Stop
        if ($check.fields.status.name -eq "Done") {
            Write-Host " [OK]" -ForegroundColor Green
            $ok++
        } else {
            Write-Host " [STATUS: $($check.fields.status.name)]" -ForegroundColor Yellow
            $ok++
        }
    } catch {
        Write-Host "  [FAIL] $key - $($_.Exception.Message)" -ForegroundColor Red
        $fail++
    }

    Start-Sleep -Milliseconds 300
}

Write-Host "`n===== KET QUA =====" -ForegroundColor Cyan
Write-Host "  Thanh cong: $ok/11" -ForegroundColor Green
if ($fail -gt 0) { Write-Host "  That bai  : $fail" -ForegroundColor Red }
Write-Host "`nBoard: $JIRA_URL/jira/software/projects/ITUQ/boards" -ForegroundColor Yellow
