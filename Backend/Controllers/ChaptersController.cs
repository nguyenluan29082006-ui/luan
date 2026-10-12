using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerCSharp.Data;
using ServerCSharp.DTOs;

namespace ServerCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Chapters - Đọc & Quản Lý Chương")]
    public class ChaptersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ChaptersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChapter(int id)
        {
            var chapter = await _context.Chapters
                .Include(c => c.Story)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (chapter == null || chapter.Story == null)
            {
                return NotFound(new { message = "Không tìm thấy chương truyện" });
            }

            // Increment views
            chapter.Views++;
            await _context.SaveChangesAsync();

            var allChapters = await _context.Chapters
                .Where(c => c.StoryId == chapter.StoryId)
                .OrderBy(c => c.ChapterNumber)
                .ToListAsync();

            var currentIndex = allChapters.FindIndex(c => c.Id == chapter.Id);
            int? prevId = currentIndex > 0 ? allChapters[currentIndex - 1].Id : null;
            int? nextId = currentIndex < allChapters.Count - 1 ? allChapters[currentIndex + 1].Id : null;

            return Ok(new ChapterDetailResponse
            {
                Chapter = new ChapterDto
                {
                    Id = chapter.Id,
                    StoryId = chapter.StoryId,
                    ChapterNumber = chapter.ChapterNumber,
                    Title = chapter.Title,
                    Content = chapter.Content,
                    Views = chapter.Views,
                    CreatedAt = chapter.CreatedAt
                },
                Story = new StorySimpleDto
                {
                    Id = chapter.Story.Id,
                    Title = chapter.Story.Title,
                    Slug = chapter.Story.Slug,
                    Type = chapter.Story.Type,
                    AuthorName = chapter.Story.AuthorName
                },
                PrevChapterId = prevId,
                NextChapterId = nextId,
                ChapterList = allChapters.Select(c => new ChapterSimpleDto
                {
                    Id = c.Id,
                    ChapterNumber = c.ChapterNumber,
                    Title = c.Title,
                    CreatedAt = c.CreatedAt
                }).ToList()
            });
        }
    }
}
