using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerCSharp.Data;
using ServerCSharp.Models;

namespace ServerCSharp.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalUsers = await _context.Users.CountAsync();
            var totalStories = await _context.Stories.CountAsync();
            var totalChapters = await _context.Chapters.CountAsync();
            var pendingStoriesCount = await _context.Stories.CountAsync(s => s.ApprovalStatus == "pending");
            var pendingReportsCount = await _context.Reports.CountAsync(r => r.Status == "pending");
            var totalViews = await _context.Stories.SumAsync(s => s.Views);

            var pendingStories = await _context.Stories
                .Include(s => s.Author)
                .Include(s => s.Chapters)
                .Include(s => s.StoryGenres).ThenInclude(sg => sg.Genre)
                .Where(s => s.ApprovalStatus == "pending")
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            var reports = await _context.Reports
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            var users = await _context.Users.OrderByDescending(u => u.CreatedAt).ToListAsync();
            var genres = await _context.Genres.Include(g => g.StoryGenres).ToListAsync();

            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalStories = totalStories;
            ViewBag.TotalChapters = totalChapters;
            ViewBag.PendingStoriesCount = pendingStoriesCount;
            ViewBag.PendingReportsCount = pendingReportsCount;
            ViewBag.TotalViews = totalViews;

            ViewBag.PendingStories = pendingStories;
            ViewBag.Reports = reports;
            ViewBag.Users = users;
            ViewBag.Genres = genres;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApproveStory(int id)
        {
            var story = await _context.Stories.FindAsync(id);
            if (story != null)
            {
                story.ApprovalStatus = "approved";
                story.RejectionReason = null;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Truyện \"{story.Title}\" đã được phê duyệt và xuất bản thành công!";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RejectStory(int id, string reason)
        {
            var story = await _context.Stories.FindAsync(id);
            if (story != null)
            {
                story.ApprovalStatus = "rejected";
                story.RejectionReason = string.IsNullOrWhiteSpace(reason) ? "Nội dung chưa đạt tiêu chuẩn kiểm duyệt." : reason.Trim();
                await _context.SaveChangesAsync();
                TempData["InfoMessage"] = $"Đã từ chối truyện \"{story.Title}\" kèm lý do phản hồi cho tác giả.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ResolveReport(int id, string action)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report != null)
            {
                if (action == "delete_target")
                {
                    if (report.TargetType == "comment")
                    {
                        var comment = await _context.Comments.FindAsync(report.TargetId);
                        if (comment != null) _context.Comments.Remove(comment);
                    }
                    else if (report.TargetType == "chapter")
                    {
                        var chapter = await _context.Chapters.FindAsync(report.TargetId);
                        if (chapter != null) _context.Chapters.Remove(chapter);
                    }
                }

                report.Status = "resolved";
                report.ResolutionNotes = action == "delete_target" ? "Đã gỡ bỏ nội dung vi phạm." : "Đã xử lý xong.";
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã giải quyết báo cáo vi phạm thành công!";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DismissReport(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report != null)
            {
                report.Status = "dismissed";
                report.ResolutionNotes = "Bác bỏ báo cáo: Không có vi phạm.";
                await _context.SaveChangesAsync();
                TempData["InfoMessage"] = "Đã bác bỏ báo cáo vi phạm.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserRole(int id, string role)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.Role = role;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Đã cập nhật quyền tài khoản {user.Email} thành {role}!";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleUserBan(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null && user.Role != "admin")
            {
                user.Status = user.Status == "banned" ? "active" : "banned";
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = user.Status == "banned" ? "Đã khóa tài khoản thành công" : "Đã mở khóa tài khoản thành công";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateGenre(string name, string slug, string? icon)
        {
            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(slug))
            {
                _context.Genres.Add(new Genre
                {
                    Name = name.Trim(),
                    Slug = slug.Trim().ToLower(),
                    Icon = icon ?? "📖"
                });
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Tạo thể loại mới thành công!";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre != null)
            {
                _context.Genres.Remove(genre);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã xóa thể loại!";
            }
            return RedirectToAction("Index");
        }
    }
}
