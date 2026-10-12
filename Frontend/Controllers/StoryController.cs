using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerCSharp.Data;
using ServerCSharp.Models;

namespace ServerCSharp.Controllers
{
    public class StoryController : Controller
    {
        private readonly AppDbContext _context;

        public StoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Detail(int id)
        {
            var story = await _context.Stories
                .Include(s => s.StoryGenres).ThenInclude(sg => sg.Genre)
                .Include(s => s.Chapters)
                .Include(s => s.Comments).ThenInclude(c => c.User)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (story == null) return NotFound();

            // Increment views
            story.Views++;
            await _context.SaveChangesAsync();

            bool isFavorited = false;
            Chapter? resumeChapter = null;

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    isFavorited = await _context.Favorites.AnyAsync(f => f.UserId == userId && f.StoryId == id);
                    var history = await _context.ReadingHistories.Include(h => h.Chapter).FirstOrDefaultAsync(h => h.UserId == userId && h.StoryId == id);
                    if (history != null) resumeChapter = history.Chapter;
                }
            }

            ViewBag.IsFavorited = isFavorited;
            ViewBag.ResumeChapter = resumeChapter;
            ViewBag.FirstChapter = story.Chapters.OrderBy(c => c.ChapterNumber).FirstOrDefault();

            return View(story);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int id)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = $"/Story/Detail/{id}" });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId)) return Unauthorized();

            var existing = await _context.Favorites.FirstOrDefaultAsync(f => f.UserId == userId && f.StoryId == id);
            if (existing != null)
            {
                _context.Favorites.Remove(existing);
                TempData["InfoMessage"] = "Đã xóa khỏi tủ truyện yêu thích";
            }
            else
            {
                _context.Favorites.Add(new Favorite { UserId = userId, StoryId = id });
                TempData["SuccessMessage"] = "Đã lưu vào tủ truyện yêu thích!";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Detail", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int storyId, string content)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = $"/Story/Detail/{storyId}" });
            }

            if (!string.IsNullOrWhiteSpace(content))
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    _context.Comments.Add(new Comment
                    {
                        UserId = userId,
                        StoryId = storyId,
                        Content = content.Trim()
                    });
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Đã đăng bình luận!";
                }
            }

            return RedirectToAction("Detail", new { id = storyId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComment(int id, int storyId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (int.TryParse(userIdClaim, out int userId))
            {
                var comment = await _context.Comments.FindAsync(id);
                if (comment != null && (comment.UserId == userId || role == "admin"))
                {
                    _context.Comments.Remove(comment);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Đã xóa bình luận!";
                }
            }

            return RedirectToAction("Detail", new { id = storyId });
        }

        [HttpPost]
        public async Task<IActionResult> Report(int targetId, string targetType, string reason, string? details, int storyId)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = $"/Story/Detail/{storyId}" });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                _context.Reports.Add(new Report
                {
                    UserId = userId,
                    TargetType = targetType,
                    TargetId = targetId,
                    Reason = reason,
                    Details = details ?? "",
                    Status = "pending"
                });
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Báo cáo vi phạm của bạn đã được gửi tới Ban Quản Trị để xử lý. Cảm ơn bạn!";
            }

            return RedirectToAction("Detail", new { id = storyId });
        }
    }
}
