using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerCSharp.Data;
using ServerCSharp.Models;

namespace ServerCSharp.Controllers
{
    [Authorize(Roles = "author,admin")]
    public class AuthorController : Controller
    {
        private readonly AppDbContext _context;

        public AuthorController(AppDbContext context)
        {
            _context = context;
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out int id) ? id : 0;
        }

        private string Slugify(string text)
        {
            string str = text.ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in str)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    if (c == 'đ') sb.Append('d');
                    else sb.Append(c);
                }
            }
            str = Regex.Replace(sb.ToString(), @"[^a-z0-9\s-]", "");
            str = Regex.Replace(str, @"\s+", "-").Trim('-');
            return str;
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            var stories = await _context.Stories
                .Include(s => s.StoryGenres).ThenInclude(sg => sg.Genre)
                .Include(s => s.Chapters)
                .Where(s => s.AuthorId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            var genres = await _context.Genres.ToListAsync();

            ViewBag.AllGenres = genres;
            return View(stories);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStory(string title, string description, string? coverUrl, string type, List<int>? genreIds)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description))
            {
                TempData["ErrorMessage"] = "Vui lòng nhập tên truyện và giới thiệu";
                return RedirectToAction("Index");
            }

            var userId = GetCurrentUserId();
            var user = await _context.Users.FindAsync(userId);

            string baseSlug = Slugify(title);
            string slug = baseSlug;
            int counter = 1;
            while (await _context.Stories.AnyAsync(s => s.Slug == slug))
            {
                slug = $"{baseSlug}-{counter++}";
            }

            var story = new Story
            {
                Title = title.Trim(),
                Slug = slug,
                AuthorId = userId,
                AuthorName = user != null ? user.Name : "Tác giả",
                Description = description.Trim(),
                CoverUrl = string.IsNullOrWhiteSpace(coverUrl)
                    ? "https://images.unsplash.com/photo-1543002588-bfa74002ed7e?w=500&auto=format&fit=crop&q=80"
                    : coverUrl.Trim(),
                Type = type == "comic" ? "comic" : "novel",
                Status = "ongoing",
                ApprovalStatus = "pending", // Pending approval by Admin
                Views = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Stories.Add(story);
            await _context.SaveChangesAsync();

            if (genreIds != null && genreIds.Count > 0)
            {
                foreach (var gId in genreIds)
                {
                    _context.StoryGenres.Add(new StoryGenre { StoryId = story.Id, GenreId = gId });
                }
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = "Đăng truyện thành công! Tác phẩm đang chờ Ban Quản Trị phê duyệt.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateChapter(int storyId, int? chapterNumber, string title, string content)
        {
            var userId = GetCurrentUserId();
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            var story = await _context.Stories.FindAsync(storyId);
            if (story == null || (story.AuthorId != userId && role != "admin"))
            {
                return Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content))
            {
                TempData["ErrorMessage"] = "Vui lòng nhập tiêu đề chương và nội dung";
                return RedirectToAction("Index");
            }

            var count = await _context.Chapters.CountAsync(c => c.StoryId == storyId);
            int number = chapterNumber.HasValue ? chapterNumber.Value : (count + 1);

            var chapter = new Chapter
            {
                StoryId = storyId,
                ChapterNumber = number,
                Title = title.Trim(),
                Content = content.Trim(),
                Views = 0,
                CreatedAt = DateTime.UtcNow
            };

            _context.Chapters.Add(chapter);
            story.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Đã thêm Chương {chapter.ChapterNumber} thành công!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStory(int id)
        {
            var userId = GetCurrentUserId();
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            var story = await _context.Stories.FindAsync(id);
            if (story != null && (story.AuthorId == userId || role == "admin"))
            {
                _context.Stories.Remove(story);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã xóa bộ truyện thành công";
            }

            return RedirectToAction("Index");
        }
    }
}
