# ============================================================
#  push_backlog_to_jira.ps1
#  1. Cap nhat mo ta (description) tu BACKLOG.md
#  2. Chuyen trang thai tat ca issues -> Done
#  Chay: .\push_backlog_to_jira.ps1
# ============================================================

$JIRA_URL  = "https://nguyenluan29082006.atlassian.net"
$EMAIL     = "nguyenluan29082006@gmail.com"
$API_TOKEN = "ATATT3xFfGF0cEk1CgEUJYYLGF462ecYIQt0Q3CwaUZlmaRkmvsEmuNah8v8khard3EYszNb00FhbZnlc6Z8pSLVWHaWK-BjuRBAqGeug54WCR2uJ81NA1Obb2fE1lplj8AHH-P9TBfhiXnY6npesHCTdwnXwkSFiQo8-lDvmC4gOGnzEfgIpnI=FC0D56C7"

$bytes   = [System.Text.Encoding]::UTF8.GetBytes("${EMAIL}:${API_TOKEN}")
$AUTH    = "Basic " + [Convert]::ToBase64String($bytes)
$HEADERS = @{ "Authorization" = $AUTH; "Content-Type" = "application/json" }

# ============================================================
# Helper ADF (Atlassian Document Format)
# ============================================================
function Text($t)          { @{ type = "text"; text = $t } }
function Para($texts)      { @{ type = "paragraph"; content = @($texts) } }
function Heading($t, $lv)  { @{ type = "heading"; attrs = @{ level = $lv }; content = @(Text $t) } }
function BulletList($items){
    $li = $items | ForEach-Object { @{ type = "listItem"; content = @(Para @(Text $_)) } }
    @{ type = "bulletList"; content = @($li) }
}

# ============================================================
# Ham: Lay transition ID cua "Done" cho 1 issue
# ============================================================
function Get-DoneTransitionId($issueKey) {
    $url = "$JIRA_URL/rest/api/3/issue/$issueKey/transitions"
    try {
        $resp = Invoke-RestMethod -Uri $url -Method Get -Headers $HEADERS -ErrorAction Stop
        $done = $resp.transitions | Where-Object { $_.name -in @("Done", "Closed", "Resolved", "Complete") } | Select-Object -First 1
        if ($done) { return $done.id }
        # Fallback: lay transition cuoi cung (thuong la Done)
        return ($resp.transitions | Select-Object -Last 1).id
    } catch {
        return $null
    }
}

# ============================================================
# Ham: Chuyen trang thai issue sang Done
# ============================================================
function Set-IssueDone($issueKey) {
    $transitionId = Get-DoneTransitionId $issueKey
    if (-not $transitionId) {
        Write-Host "    [WARN] Khong tim duoc transition Done cho $issueKey" -ForegroundColor Yellow
        return $false
    }

    # Thu chuyen trang thai (co the can qua nhieu buoc)
    # Jira Scrum thuong co: To Do -> In Progress -> Done
    # Nen thu het cac transitions cho den khi la Done
    $maxSteps = 5
    for ($i = 0; $i -lt $maxSteps; $i++) {
        # Kiem tra trang thai hien tai
        try {
            $issue = Invoke-RestMethod -Uri "$JIRA_URL/rest/api/3/issue/$issueKey" -Method Get -Headers $HEADERS -ErrorAction Stop
            $currentStatus = $issue.fields.status.name
            if ($currentStatus -in @("Done", "Closed", "Resolved", "Complete")) {
                return $true
            }
        } catch { break }

        # Lay transitions hien tai
        try {
            $resp = Invoke-RestMethod -Uri "$JIRA_URL/rest/api/3/issue/$issueKey/transitions" -Method Get -Headers $HEADERS -ErrorAction Stop
            $nextTransition = $resp.transitions | Where-Object { $_.name -in @("Done", "Closed", "Resolved", "Complete", "In Progress", "In Review") } | Select-Object -First 1
            if (-not $nextTransition) {
                $nextTransition = $resp.transitions | Select-Object -Last 1
            }
            if (-not $nextTransition) { break }

            $body = @{ transition = @{ id = $nextTransition.id } } | ConvertTo-Json -Depth 5
            Invoke-RestMethod -Uri "$JIRA_URL/rest/api/3/issue/$issueKey/transitions" -Method Post -Headers $HEADERS -Body $body -ErrorAction Stop | Out-Null
            Start-Sleep -Milliseconds 200
        } catch { break }
    }

    # Kiem tra lan cuoi
    try {
        $issue = Invoke-RestMethod -Uri "$JIRA_URL/rest/api/3/issue/$issueKey" -Method Get -Headers $HEADERS -ErrorAction Stop
        $currentStatus = $issue.fields.status.name
        return ($currentStatus -in @("Done", "Closed", "Resolved", "Complete"))
    } catch { return $false }
}

# ============================================================
# Danh sach 11 Issues voi noi dung Acceptance Criteria day du
# ============================================================
$issues = @(
    @{
        key    = "ITUQ-1"
        summary = "API Xac thuc & Quan Ly Tai Khoan (Auth & Identity)"
        points = 5
        priority = "High"
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
            Heading "File lien quan" 3
            BulletList @(
                "Controllers/AuthController.cs - API endpoints"
                "Controllers/AccountController.cs - MVC login/logout"
                "Views/Account/Login.cshtml - Giao dien dang nhap"
                "Data/AppDbContext.cs - Users table"
            )
        )
    }
    @{
        key    = "ITUQ-2"
        summary = "API Quan Ly Truyen & The Loai (Stories & Genres)"
        points = 5
        priority = "High"
        content = @(
            Heading "FR-02: Quan Ly Truyen & The Loai (Stories & Genres)" 2
            Para @(Text "Ma: FR-02 | Epic: Backend | Story Points: 5 | Priority: High | Status: Done")
            Para @(Text "La mot Doc gia, toi muon tim kiem va duyet truyen theo the loai de nhanh chong tim duoc tac pham phu hop.")
            Heading "Acceptance Criteria" 3
            BulletList @(
                "[DONE] GET /api/stories - Danh sach truyen voi filter: genreId, search, type, status, page, pageSize"
                "[DONE] GET /api/stories/{id} - Chi tiet truyen kem danh sach chuong"
                "[DONE] GET /api/stories/featured - Top truyen noi bat (nhieu view nhat)"
                "[DONE] POST /api/stories - Tac gia tao truyen moi (ApprovalStatus: pending)"
                "[DONE] GET /api/genres - Danh sach toan bo the loai"
                "[DONE] Slug tu dong sinh tu ten truyen (unique, chong trung)"
                "[DONE] Truyen moi tao co ApprovalStatus = pending, chua hien thi voi doc gia"
                "[DONE] Ho tro phan trang (Pagination) tranh qua tai"
            )
            Heading "File lien quan" 3
            BulletList @(
                "Controllers/StoriesController.cs - Stories & Genres API"
                "Controllers/HomeController.cs - MVC browse page"
                "Views/Home/Index.cshtml - Trang chu"
                "Views/Home/Genres.cshtml - Trang kham pha"
                "Models/Story.cs, Models/Genre.cs"
            )
        )
    }
    @{
        key    = "ITUQ-3"
        summary = "API Quan Ly Chuong & Trinh doc truyen (Chapters & Reading)"
        points = 8
        priority = "High"
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
            Heading "File lien quan" 3
            BulletList @(
                "Controllers/ChaptersController.cs - Chapters API"
                "Controllers/ReaderController.cs - MVC reading page"
                "Views/Reader/Read.cshtml - Trinh doc truyen"
                "Models/Chapter.cs"
            )
        )
    }
    @{
        key    = "ITUQ-4"
        summary = "API Tu sach & Lich su doc (Favorites & Reading History)"
        points = 3
        priority = "Medium"
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
            Heading "File lien quan" 3
            BulletList @(
                "Controllers/InteractionsController.cs - Favorites & History API"
                "Controllers/HomeController.cs - Favorites MVC page"
                "Views/Home/Favorites.cshtml - Trang tu sach"
                "Models/Favorite.cs, Models/ReadingHistory.cs"
            )
        )
    }
    @{
        key    = "ITUQ-5"
        summary = "API Binh luan & Bao cao vi pham (Comments & Reports)"
        points = 3
        priority = "Medium"
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
            Heading "File lien quan" 3
            BulletList @(
                "Controllers/CommentsAndReportsController.cs - Comments & Reports API"
                "Controllers/StoryController.cs - MVC comment actions"
                "Views/Story/Detail.cshtml - Section binh luan"
                "Models/Comment.cs, Models/Report.cs"
            )
        )
    }
    @{
        key    = "ITUQ-6"
        summary = "API Quan tri he thong & Phe duyet truyen (Admin Portal API)"
        points = 5
        priority = "High"
        content = @(
            Heading "FR-06: Quan Tri He Thong & Phe Duyet Truyen (Admin Portal API)" 2
            Para @(Text "Ma: FR-06 | Epic: Backend | Story Points: 5 | Priority: High | Status: Done")
            Para @(Text "La mot Quan tri vien, toi muon kiem duyet truyen, quan ly tai khoan va xu ly bao cao de dam bao chat luong noi dung tren nen tang.")
            Heading "Acceptance Criteria" 3
            BulletList @(
                "[DONE] Duyet truyen: ApproveStory() - chuyen ApprovalStatus = approved"
                "[DONE] Tu choi truyen: RejectStory() - luu RejectionReason cho tac gia biet"
                "[DONE] Khoa/mo khoa tai khoan: ToggleUserBan() - Status = banned/active"
                "[DONE] Xu ly bao cao: ResolveReport() - xoa noi dung vi pham"
                "[DONE] Bac bo bao cao: DismissReport() - danh dau dismissed"
                "[DONE] Doi role nguoi dung: ChangeUserRole() - reader/author/admin"
                "[DONE] Admin khong the tu khoa tai khoan minh"
                "[DONE] Tat ca action duoc bao ve bang [Authorize(Roles=admin)]"
            )
            Heading "File lien quan" 3
            BulletList @(
                "Controllers/AdminController.cs - Tat ca Admin actions"
                "Views/Admin/Index.cshtml - Admin Dashboard UI"
            )
        )
    }
    @{
        key    = "ITUQ-7"
        summary = "API Thong ke & Bang xep hang (Dashboard & Leaderboard)"
        points = 5
        priority = "Medium"
        content = @(
            Heading "FR-07: Thong Ke & Bang Xep Hang (Dashboard & Leaderboard)" 2
            Para @(Text "Ma: FR-07 | Epic: Backend | Story Points: 5 | Priority: Medium | Status: Done")
            Para @(Text "La mot Admin, toi muon xem so lieu tong quan he thong va bang xep hang de nam bat tinh hinh hoat dong cua nen tang.")
            Heading "Acceptance Criteria" 3
            BulletList @(
                "[DONE] Tong so Users (CountAsync)"
                "[DONE] Tong so Truyen, Chuong"
                "[DONE] Tong luot xem toan he thong (SumAsync Views)"
                "[DONE] So truyen cho duyet (ApprovalStatus = pending)"
                "[DONE] So bao cao chua xu ly (Status = pending)"
                "[DONE] Danh sach truyen nhieu view nhat (OrderByDescending Views)"
            )
            Heading "File lien quan" 3
            BulletList @(
                "Controllers/AdminController.cs - Dashboard stats (ViewBag)"
                "Views/Admin/Index.cshtml - Hien thi KPI cards va leaderboard"
            )
        )
    }
    @{
        key    = "ITUQ-8"
        summary = "Frontend Giao dien Doc gia (Reader Web MVC)"
        points = 8
        priority = "High"
        content = @(
            Heading "FR-08: Frontend Giao Dien Doc Gia (Reader Web MVC)" 2
            Para @(Text "Ma: FR-08 | Epic: Frontend | Story Points: 8 | Priority: High | Status: Done")
            Para @(Text "La mot Doc gia, toi muon mot giao dien dep, responsive de doc truyen bao gom trang chu, tim kiem, chi tiet truyen va trinh doc.")
            Heading "Acceptance Criteria" 3
            BulletList @(
                "[DONE] Trang chu Home/Index - Hero banner, Top truyen hot (Top 6), Truyen moi cap nhat (8 truyen), Danh muc the loai"
                "[DONE] Trang kham pha Home/Genres - Tim kiem, loc theo the loai/loai/tinh trang/sort"
                "[DONE] Trang chi tiet truyen Story/Detail - Bia truyen, mo ta, danh sach chuong, binh luan, nut yeu thich"
                "[DONE] Trinh doc truyen Reader/Read - Noi dung chuong, nut Prev/Next, Dark/Light mode, tang/giam co chu"
                "[DONE] Trang tu sach Home/Favorites - Danh sach truyen yeu thich + Lich su doc"
                "[DONE] Giao dien Glassmorphism (Cyber Indigo + Dark mode)"
                "[DONE] Responsive mobile (media queries)"
                "[DONE] Tu dong luu lich su doc qua API /api/history khi chuyen chuong"
            )
            Heading "File lien quan" 3
            BulletList @(
                "Views/Home/Index.cshtml - Trang chu (17KB)"
                "Views/Home/Genres.cshtml - Kham pha"
                "Views/Home/Favorites.cshtml - Tu sach"
                "Views/Story/Detail.cshtml - Chi tiet truyen (23KB)"
                "Views/Reader/Read.cshtml - Trinh doc (11KB)"
                "Views/Shared/_Layout.cshtml - Layout chung"
                "wwwroot/css/ - CSS Glassmorphism"
            )
        )
    }
    @{
        key    = "ITUQ-9"
        summary = "Frontend Studio Tac gia (Author Studio)"
        points = 5
        priority = "Medium"
        content = @(
            Heading "FR-09: Frontend Studio Tac Gia (Author Studio)" 2
            Para @(Text "Ma: FR-09 | Epic: Frontend | Story Points: 5 | Priority: Medium | Status: Done")
            Para @(Text "La mot Tac gia, toi muon mot studio sang tac truc quan de tao truyen moi, dang tai chuong va theo doi tac pham cua minh.")
            Heading "Acceptance Criteria" 3
            BulletList @(
                "[DONE] Dashboard tac gia Author/Index - Danh sach truyen cua minh voi stats (views, chuong)"
                "[DONE] Form tao truyen moi - Ten, mo ta, anh bia (URL), the loai (multi-select), loai hinh (novel/comic)"
                "[DONE] Form them chuong - Tieu de, so thu tu, noi dung van ban"
                "[DONE] Xoa truyen - Chi tac gia chu so huu hoac Admin"
                "[DONE] Hien thi badge trang thai: Pending (vang) / Approved (xanh) / Rejected (do) + ly do tu choi"
                "[DONE] Chi Author va Admin moi truy cap duoc Studio ([Authorize(Roles=author,admin)])"
            )
            Heading "File lien quan" 3
            BulletList @(
                "Controllers/AuthorController.cs - CreateStory, CreateChapter, DeleteStory"
                "Views/Author/Index.cshtml - Author Studio UI (12KB)"
            )
        )
    }
    @{
        key    = "ITUQ-10"
        summary = "Frontend Cong Quan tri Admin (Admin Dashboard UI)"
        points = 5
        priority = "High"
        content = @(
            Heading "FR-10: Frontend Cong Quan Tri Admin (Admin Dashboard UI)" 2
            Para @(Text "Ma: FR-10 | Epic: Frontend | Story Points: 5 | Priority: High | Status: Done")
            Para @(Text "La mot Admin, toi muon mot bang dieu khien quan tri truc quan de duyet truyen, quan ly nguoi dung va xu ly vi pham nhanh chong.")
            Heading "Acceptance Criteria" 3
            BulletList @(
                "[DONE] KPI Cards - Tong Users, Truyen, Chuong, Views, Cho duyet, Reports"
                "[DONE] Tab 'Duyet Truyen' - Danh sach Pending, nut Approve/Reject 1-click, nhap ly do tu choi"
                "[DONE] Tab 'Quan ly Users' - Danh sach tat ca users, doi Role, Ban/Unban"
                "[DONE] Tab 'Bao cao vi pham' - Danh sach Reports, Resolve (xoa noi dung) hoac Dismiss"
                "[DONE] Tab 'Quan ly The loai' - Them the loai moi (ten + slug + icon), Xoa the loai"
                "[DONE] Flash TempData - Thong bao SuccessMessage/InfoMessage/ErrorMessage sau moi action"
                "[DONE] Chi Admin moi truy cap ([Authorize(Roles=admin)])"
            )
            Heading "File lien quan" 3
            BulletList @(
                "Controllers/AdminController.cs - Tat ca Admin actions (188 lines)"
                "Views/Admin/Index.cshtml - Admin Dashboard UI (21KB)"
            )
        )
    }
    @{
        key    = "ITUQ-11"
        summary = "Kiem thu tich hop RESTful API & Viet Swagger Docs"
        points = 3
        priority = "Low"
        content = @(
            Heading "FR-11: Kiem Thu Tich Hop RESTful API & Viet Swagger Docs" 2
            Para @(Text "Ma: FR-11 | Epic: Testing/QA | Story Points: 3 | Priority: Low | Status: Done")
            Para @(Text "La mot thanh vien nhom, toi muon tai lieu hoa va kiem thu day du toan bo API de dam bao chat luong va de dang demo voi giang vien.")
            Heading "Acceptance Criteria" 3
            BulletList @(
                "[DONE] File ServerCSharp.http - Test cases cho 30+ endpoints (Auth, Stories, Chapters, Favorites, Comments, Reports, Admin)"
                "[DONE] Swagger UI tai http://localhost:5000/swagger - Mo ta day du API voi Bearer Auth"
                "[DONE] Test flow: Register -> Login -> nhan JWT -> goi cac protected API"
                "[DONE] Test phan quyen: Reader khong vao duoc Author/Admin routes"
                "[DONE] API response time < 300ms cho cac endpoint doc du lieu"
                "[DONE] Swagger config: AddSecurityDefinition Bearer + AddSecurityRequirement"
            )
            Heading "File lien quan" 3
            BulletList @(
                "ServerCSharp.http - REST Client test file (10KB, 30+ test cases)"
                "Program.cs - Swagger/OpenAPI config"
                "API_DOCUMENTATION.md - Tai lieu API day du"
            )
        )
    }
)

# ============================================================
# THUC THI: Cap nhat description + chuyen status -> Done
# ============================================================
Write-Host "`n=========================================" -ForegroundColor Cyan
Write-Host "  JIRA BACKLOG SYNC - WebDocTruyen ITUQ" -ForegroundColor Cyan
Write-Host "  11 Issues | Tat ca -> Done" -ForegroundColor Cyan
Write-Host "=========================================`n" -ForegroundColor Cyan

$successDesc   = 0
$successStatus = 0
$failedDesc    = 0
$failedStatus  = 0

foreach ($issue in $issues) {
    Write-Host "Processing $($issue.key): $($issue.summary)" -ForegroundColor White

    # --- BUOC 1: Cap nhat description ---
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
        Write-Host "  [OK] Mo ta cap nhat thanh cong" -ForegroundColor Green
        $successDesc++
    } catch {
        Write-Host "  [FAIL] Mo ta: $($_.Exception.Message)" -ForegroundColor Red
        $failedDesc++
    }

    Start-Sleep -Milliseconds 300

    # --- BUOC 2: Chuyen status sang Done ---
    $isDone = Set-IssueDone $issue.key
    if ($isDone) {
        Write-Host "  [OK] Status -> Done" -ForegroundColor Green
        $successStatus++
    } else {
        Write-Host "  [WARN] Khong the chuyen sang Done (co the da Done roi)" -ForegroundColor Yellow
        $failedStatus++
    }

    Start-Sleep -Milliseconds 400
    Write-Host ""
}

# ============================================================
# KET QUA CUOI
# ============================================================
Write-Host "=========================================" -ForegroundColor Cyan
Write-Host "  KET QUA CUOI CUNG" -ForegroundColor Cyan
Write-Host "=========================================`n" -ForegroundColor Cyan
Write-Host "  Mo ta cap nhat : $successDesc/11 thanh cong" -ForegroundColor Green
Write-Host "  Status -> Done : $successStatus/11 thanh cong" -ForegroundColor Green
if (($failedDesc + $failedStatus) -gt 0) {
    Write-Host "  Loi           : $($failedDesc + $failedStatus)" -ForegroundColor Red
}
Write-Host "`nKiem tra Jira Board:" -ForegroundColor Yellow
Write-Host "  $JIRA_URL/jira/software/projects/ITUQ/boards" -ForegroundColor Yellow
