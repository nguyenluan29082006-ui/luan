using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerCSharp.Data;
using ServerCSharp.Models;

namespace ServerCSharp.Controllers
{
    public class ReaderController : Controller
    {
        private readonly AppDbContext _context;

        public ReaderController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Read(int id)
        {
            var chapter = await _context.Chapters
                .Include(c => c.Story)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (chapter == null || chapter.Story == null) return NotFound();

            // Increment views
            chapter.Views++;

            // Auto save reading history if logged in
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    var existingHistory = await _context.ReadingHistories.FirstOrDefaultAsync(h => h.UserId == userId && h.StoryId == chapter.StoryId);
                    if (existingHistory != null)
                    {
                        existingHistory.ChapterId = chapter.Id;
                        existingHistory.UpdatedAt = DateTime.UtcNow;
                    }
                    else
                    {
                        _context.ReadingHistories.Add(new ReadingHistory
                        {
                            UserId = userId,
                            StoryId = chapter.StoryId,
                            ChapterId = chapter.Id,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();

            var allChapters = await _context.Chapters
                .Where(c => c.StoryId == chapter.StoryId)
                .OrderBy(c => c.ChapterNumber)
                .ToListAsync();

            var currentIndex = allChapters.FindIndex(c => c.Id == chapter.Id);
            Chapter? prev = currentIndex > 0 ? allChapters[currentIndex - 1] : null;
            Chapter? next = currentIndex < allChapters.Count - 1 ? allChapters[currentIndex + 1] : null;

            ViewBag.PrevChapter = prev;
            ViewBag.NextChapter = next;
            ViewBag.AllChapters = allChapters;

            return View(chapter);
        }
    }
}
