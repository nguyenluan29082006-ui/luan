# ============================================================
#  push_backlog_to_jira.ps1
#  Cap nhat mo ta + status cho 11 Issues Jira tu BACKLOG.md
#  Chay: .\push_backlog_to_jira.ps1
# ============================================================

$JIRA_URL  = "https://nguyenluan29082006.atlassian.net"
$EMAIL     = "nguyenluan29082006@gmail.com"
$API_TOKEN = "ATATT3xFfGF0cEk1CgEUJYYLGF462ecYIQt0Q3CwaUZlmaRkmvsEmuNah8v8khard3EYszNb00FhbZnlc6Z8pSLVWHaWK-BjuRBAqGeug54WCR2uJ81NA1Obb2fE1lplj8AHH-P9TBfhiXnY6npesHCTdwnXwkSFiQo8-lDvmC4gOGnzEfgIpnI=FC0D56C7"

$bytes   = [System.Text.Encoding]::UTF8.GetBytes("${EMAIL}:${API_TOKEN}")
$AUTH    = "Basic " + [Convert]::ToBase64String($bytes)
$HEADERS = @{ "Authorization" = $AUTH; "Content-Type" = "application/json" }

# Helper ADF (Atlassian Document Format)
function Text($t)          { @{ type = "text"; text = $t } }
function Para($texts)      { @{ type = "paragraph"; content = @($texts) } }
function Heading($t, $lv)  { @{ type = "heading"; attrs = @{ level = $lv }; content = @(Text $t) } }
function BulletList($items){
    $li = $items | ForEach-Object { @{ type = "listItem"; content = @(Para @(Text $_)) } }
    @{ type = "bulletList"; content = @($li) }
}

# ============================================================
# Danh sach 11 Issues voi noi dung day du
# ============================================================
$issues = @(
    @{
        key    = "ITUQ-1"
        points = 5
        content = @(
            Heading "FR-01: Xac Thuc & Quan Ly Tai Khoan (Auth & Identity)" 2
            Para @(Text "Ma: FR-01 | Epic: Backend | Story Points: 5 | Priority: High | Status: Done")
            Para @(Text "La mot nguoi dung (Khach/Reader/Author/Admin), toi muon dang ky va dang nhap vao he thong de duoc cap JWT Token va truy cap cac tinh nang theo vai tro.")
            Heading "Acceptance Criteria" 3
            BulletList @(
                "[DONE] POST /api/auth/register - Dang ky tai khoan moi voi email, password, fullname"
                "[DONE] POST /api/auth/login - Dang nhap, nhan JWT Bearer Token (expires 7 ngay)"
                "[DONE] GET /api/auth/me - Lay thong tin profile nguoi dung hien tai"
                "[DONE] POST /api/auth/become-author - Nang cap quyen Reader len Author"
                "[DONE] Password hash bang BCrypt, khong luu plaintext"
                "[DONE] JWT Token chua Claims: UserId, Email, Role"
                "[DONE] Endpoint nhay cam duoc bao ve bang [Authorize(Roles=...)]"
                "[DONE] Tai khoan bi khoa (Status = banned) khong the dang nhap"
            )
        )
    }
    @{
        key    = "ITUQ-2"
        points = 5
        content = @(
            Heading "FR-02: Quan Ly Truyen & The Loai (Stories & Genres)" 2
            Para @(Text "Ma: FR-02 | Epic: Backend | Story Points: 5 | Priority: High | Status: Done")
            Para @(Text "La mot Doc gia, toi muon tim kiem va duyet truyen theo the loai de nhanh chong tim duoc tac pham phu hop.")
            Heading "Acceptance Criteria" 3
            BulletList @(
                "[DONE] GET /api/stories - Danh sach truyen voi filter: genreId, search, type, status, page, pageSize"
                "[DONE] GET /api/stories/{id} - Chi tiet truyen kem danh sach chuong"
                "[DONE] GET /api/stories/featured - Top truyen noi bat (nhieu view nhat)"
                "[DONE] POST /api/stories - Tac gia tao truyen moi (status: pending)"
                "[DONE] GET /api/genres - Danh sach toan bo the loai"
                "[DONE] Slug tu dong sinh tu ten truyen (unique)"
                "[DONE] Truyen moi tao co ApprovalStatus = pending, chua hien thi voi doc gia"
                "[DONE] Ho tro phan trang (Pagination)"
            )
        )
    }
    @{
        key    = "ITUQ-3"
        points = 8
        content = @(
            Heading "FR-03: Quan Ly Chuong & Trinh Doc Truyen (Chapters & Reading)" 2
            Para @(Text "Ma: FR-03 | Epic: Backend | Story Points: 8 | Priority: High | Status: Done")
            Para @(Text "La mot Doc gia, toi muon doc tung chuong truyen va dieu huong qua lai giua cac chuong de co trai nghiem doc lien mach.")
            Heading "Acceptance Criteria" 3
            BulletList @(
                "[DONE] GET /api/chapters/{id} - Noi dung chuong, prevChapterId, nextChapterId"
                "[DONE] POST /api/chapters - Tac gia them chuong moi cho truyen cua minh"
                "[DONE] Tu dong tang ViewCount moi lan doc chuong"
                "[DONE] Tu dong tang Views cua truyen me"
                "[DONE] So thu tu chuong (ChapterNumber) tu dong neu khong nhap"
                "[DONE] Tac gia chi duoc them chuong cho truyen cua chinh minh (hoac Admin)"
            )
        )
    }
    @{
        key    = "ITUQ-4"
        points = 3
        content = @(
            Heading "FR-04: Tu Sach & Lich Su Doc (Favorites & Reading History)" 2
            Para @(Text "Ma: FR-04 | Epic: Backend | Story Points: 3 | Priority: Medium | Status: Done")
            Para @(Text "La mot Doc gia da dang nhap, toi muon luu truyen vao tu sach va ghi lai chuong dang doc de de dang tiep tuc doc sau.")
            Heading "Acceptance Criteria" 3
            BulletList @(
                "[DONE] POST /api/favorites/toggle/{storyId} - Them/Xoa khoi tu sach (toggle)"
                "[DONE] GET /api/favorites - Danh sach truyen trong tu sach"
                "[DONE] GET /api/favorites/check/{storyId} - Kiem tra truyen co trong tu khong"
                "[DONE] POST /api/history - Luu chuong dang doc"
                "[DONE] GET /api/history - Lich su doc, sort theo thoi gian moi nhat"
                "[DONE] Moi truyen chi luu 1 chuong doc do (upsert)"
            )
        )
    }
    @{
        key    = "ITUQ-5"
        points = 3
        content = @(
            Heading "FR-05: Binh Luan & Bao Cao Vi Pham (Comments & Reports)" 2
            Para @(Text "Ma: FR-05 | Epic: Backend | Story Points: 3 | Priority: Medium | Status: Done")
            Para @(Text "La mot Doc gia, toi muon binh luan duoi truyen va bao cao noi dung vi pham de chia se cam nghi va giup he thong an toan hon.")
            Heading "Acceptance Criteria" 3
            BulletList @(
                "[DONE] GET /api/comments/story/{storyId} - Danh sach binh luan theo truyen"
                "[DONE] POST /api/comments - Gui binh luan moi"
                "[DONE] DELETE /api/comments/{id} - Xoa binh luan (chu binh luan hoac Admin)"
                "[DONE] POST /api/reports - Gui bao cao vi pham (comment/chapter)"
                "[DONE] Admin co the xoa binh luan vi pham thong qua report"
            )
        )
    }
    @{
        key    = "ITUQ-6"
        points = 5
        content = @(
            Heading "FR-06: Quan Tri He Thong & Phe Duyet Truyen (Admin Portal API)" 2
            Para @(Text "Ma: FR-06 | Epic: Backend | Story Points: 5 | Priority: High | Status: Done")
            Para @(Text "La mot Quan tri vien, toi muon kiem duyet truyen, quan ly tai khoan va xu ly bao cao de dam bao chat luong noi dung tren nen tang.")
            Heading "Acceptance Criteria" 3
            BulletList @(
                "[DONE] GET /api/admin/pending-stories - Danh sach truyen cho duyet"
                "[DONE] POST /api/admin/approve-story/{id} - Phe duyet truyen"
                "[DONE] POST /api/admin/reject-story/{id} - Tu choi truyen kem ly do"
                "[DONE] POST /api/admin/users/{id}/toggle-lock - Khoa/mo khoa tai khoan"
                "[DONE] Admin khong the tu khoa tai khoan minh"
                "[DONE] Tat ca Admin API duoc bao ve bang [Authorize(Roles=admin)]"
            )
        )
    }
    @{
        key    = "ITUQ-7"
        points = 5
        content = @(
            Heading "FR-07: Thong Ke & Bang Xep Hang (Dashboard & Leaderboard)" 2
            Para @(Text "Ma: FR-07 | Epic: Backend | Story Points: 5 | Priority: Medium | Status: Done")
            Para @(Text "La mot Admin, toi muon xem so lieu tong quan he thong va bang xep hang de nam bat tinh hinh hoat dong cua nen tang.")
            Heading "Acceptance Criteria" 3
            BulletList @(
                "[DONE] Dashboard hien thi: Tong Users, Truyen, Chuong, Luot xem, Cho duyet"
                "[DONE] Top truyen nhieu luot xem nhat (Top 10)"
                "[DONE] Top truyen duoc yeu thich nhat"
                "[DONE] Thong ke so luong truyen theo the loai"
            )
        )
    }
    @{
        key    = "ITUQ-8"
        points = 8
        content = @(
            Heading "FR-08: Frontend Giao Dien Doc Gia (Reader Web MVC)" 2
            Para @(Text "Ma: FR-08 | Epic: Frontend | Story Points: 8 | Priority: High | Status: Done")
            Para @(Text "La mot Doc gia, toi muon mot giao dien dep, responsive de doc truyen bao gom trang chu, tim kiem, chi tiet truyen va trinh doc.")
            Heading "Acceptance Criteria" 3
            BulletList @(
                "[DONE] Trang chu Home/Index - Hero banner, Top truyen hot, Truyen moi cap nhat, Danh muc the loai"
                "[DONE] Trang kham pha Home/Genres - Tim kiem, loc theo the loai/loai/tinh trang"
                "[DONE] Trang chi tiet truyen Story/Detail - Bia truyen, mo ta, danh sach chuong, binh luan"
                "[DONE] Trinh doc truyen Reader/Read - Noi dung chuong, dieu huong Prev/Next, Dark/Light mode, tuy chinh co chu"
                "[DONE] Trang tu sach Home/Favorites - Danh sach truyen yeu thich + Lich su doc"
                "[DONE] Giao dien Glassmorphism, Dark mode, responsive mobile"
                "[DONE] Tu dong luu lich su doc khi chuyen chuong"
            )
        )
    }
    @{
        key    = "ITUQ-9"
        points = 5
        content = @(
            Heading "FR-09: Frontend Studio Tac Gia (Author Studio)" 2
            Para @(Text "Ma: FR-09 | Epic: Frontend | Story Points: 5 | Priority: Medium | Status: Done")
            Para @(Text "La mot Tac gia, toi muon mot studio sang tac truc quan de tao truyen moi, dang tai chuong va theo doi tac pham cua minh.")
            Heading "Acceptance Criteria" 3
            BulletList @(
                "[DONE] Dashboard tac gia Author/Index - Danh sach truyen cua minh voi stats"
                "[DONE] Form tao truyen moi - Ten, mo ta, anh bia, the loai, loai hinh"
                "[DONE] Form them chuong - Tieu de chuong, so thu tu, noi dung soan thao"
                "[DONE] Xoa truyen (chi truyen cua minh)"
                "[DONE] Hien thi trang thai kiem duyet: Pending / Approved / Rejected + ly do tu choi"
                "[DONE] Chi Author va Admin moi truy cap duoc Studio"
            )
        )
    }
    @{
        key    = "ITUQ-10"
        points = 5
        content = @(
            Heading "FR-10: Frontend Cong Quan Tri Admin (Admin Dashboard UI)" 2
            Para @(Text "Ma: FR-10 | Epic: Frontend | Story Points: 5 | Priority: High | Status: Done")
            Para @(Text "La mot Admin, toi muon mot bang dieu khien quan tri truc quan de duyet truyen, quan ly nguoi dung va xu ly vi pham nhanh chong.")
            Heading "Acceptance Criteria" 3
            BulletList @(
                "[DONE] Tong quan Dashboard - 6 KPI cards (Users, Truyen, Chuong, Views, Cho duyet, Reports)"
                "[DONE] Tab 'Duyet Truyen' - Danh sach truyen Pending, nut Approve/Reject 1-click kem ly do"
                "[DONE] Tab 'Quan ly Users' - Danh sach users, doi role, ban/unban tai khoan"
                "[DONE] Tab 'Bao cao vi pham' - Danh sach reports, Resolve hoac Dismiss"
                "[DONE] Tab 'Quan ly The loai' - Them/Xoa the loai"
                "[DONE] Flash message thong bao ket qua thao tac"
                "[DONE] Chi Admin moi truy cap duoc Dashboard"
            )
        )
    }
    @{
        key    = "ITUQ-11"
        points = 3
        content = @(
            Heading "FR-11: Kiem Thu Tich Hop RESTful API & Viet Swagger Docs" 2
            Para @(Text "Ma: FR-11 | Epic: Testing/QA | Story Points: 3 | Priority: Low | Status: Done")
            Para @(Text "La mot thanh vien nhom, toi muon tai lieu hoa va kiem thu day du toan bo API de dam bao chat luong va de dang demo voi giang vien.")
            Heading "Acceptance Criteria" 3
            BulletList @(
                "[DONE] File ServerCSharp.http - Test cases cho tat ca 30+ endpoints"
                "[DONE] Swagger UI tai /swagger - Mo ta day du API voi Bearer Auth"
                "[DONE] Test flow: Register -> Login -> Get token -> Call protected APIs"
                "[DONE] Test phan quyen: Reader / Author / Admin"
                "[DONE] API response time < 300ms cho cac endpoint doc du lieu"
            )
        )
    }
)

# ============================================================
# Thuc thi cap nhat Jira
# ============================================================
Write-Host "`n===== BAT DAU CAP NHAT JIRA - 11 ISSUES =====" -ForegroundColor Cyan
Write-Host "Jira: $JIRA_URL/jira/software/projects/ITUQ/boards`n" -ForegroundColor Yellow

$success = 0
$failed  = 0

foreach ($issue in $issues) {
    $url  = "$JIRA_URL/rest/api/3/issue/$($issue.key)"
    $body = @{
        fields = @{
            description = @{
                type    = "doc"
                version = 1
                content = $issue.content
            }
        }
    } | ConvertTo-Json -Depth 20

    try {
        Invoke-RestMethod -Uri $url -Method Put -Headers $HEADERS -Body $body -ErrorAction Stop | Out-Null
        Write-Host "  [OK]   $($issue.key) - Cap nhat mo ta thanh cong" -ForegroundColor Green
        $success++
    } catch {
        Write-Host "  [FAIL] $($issue.key) - Loi: $($_.Exception.Message)" -ForegroundColor Red
        $failed++
    }
    Start-Sleep -Milliseconds 400
}

Write-Host "`n===== KET QUA =====" -ForegroundColor Cyan
Write-Host "  Thanh cong : $success issues" -ForegroundColor Green
if ($failed -gt 0) {
    Write-Host "  That bai   : $failed issues" -ForegroundColor Red
}
Write-Host "`nKiem tra Jira: $JIRA_URL/jira/software/projects/ITUQ/boards" -ForegroundColor Yellow
