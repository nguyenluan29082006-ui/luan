using System;
using System.Collections.Generic;

namespace ServerCSharp.DTOs
{
    // Auth DTOs
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "reader";
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public string? Bio { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class AuthResponse
    {
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public UserDto User { get; set; } = new();
    }

    // Story DTOs
    public class StoryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? CoverUrl { get; set; }
        public string Type { get; set; } = "novel";
        public string Status { get; set; } = "ongoing";
        public string ApprovalStatus { get; set; } = "pending";
        public string? RejectionReason { get; set; }
        public int Views { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int ChapterCount { get; set; }
        public int FavoritesCount { get; set; }
        public List<GenreDto> Genres { get; set; } = new();
    }

    public class GenreDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public int StoryCount { get; set; }
    }

    public class StoryDetailDto : StoryDto
    {
        public List<ChapterSimpleDto> Chapters { get; set; } = new();
    }

    public class ChapterSimpleDto
    {
        public int Id { get; set; }
        public int ChapterNumber { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class ChapterDetailResponse
    {
        public ChapterDto Chapter { get; set; } = new();
        public StorySimpleDto Story { get; set; } = new();
        public int? PrevChapterId { get; set; }
        public int? NextChapterId { get; set; }
        public List<ChapterSimpleDto> ChapterList { get; set; } = new();
    }

    public class ChapterDto
    {
        public int Id { get; set; }
        public int StoryId { get; set; }
        public int ChapterNumber { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Views { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class StorySimpleDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Type { get; set; } = "novel";
        public string AuthorName { get; set; } = string.Empty;
    }

    // Story Create/Update Requests
    public class CreateStoryRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? CoverUrl { get; set; }
        public string Type { get; set; } = "novel";
        public List<int>? GenreIds { get; set; }
    }

    public class UpdateStoryRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? CoverUrl { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
        public List<int>? GenreIds { get; set; }
    }

    public class CreateChapterRequest
    {
        public int? ChapterNumber { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }

    // Interactions
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public UserDto User { get; set; } = new();
        public ChapterSimpleDto? Chapter { get; set; }
    }

    public class AddCommentRequest
    {
        public int StoryId { get; set; }
        public int? ChapterId { get; set; }
        public string Content { get; set; } = string.Empty;
    }

    public class ReportRequest
    {
        public string TargetType { get; set; } = string.Empty;
        public int TargetId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string? Details { get; set; }
    }

    public class SaveHistoryRequest
    {
        public int StoryId { get; set; }
        public int ChapterId { get; set; }
    }

    // Admin
    public class ReviewStoryRequest
    {
        public string Action { get; set; } = string.Empty; // 'approved' or 'rejected'
        public string? RejectionReason { get; set; }
    }

    public class HandleReportRequest
    {
        public string? Status { get; set; } // 'resolved' or 'dismissed'
        public string? Action { get; set; } // 'delete_target' or 'none'
        public string? ResolutionNotes { get; set; }
    }

    public class UpdateRoleRequest
    {
        public string Role { get; set; } = string.Empty;
    }

    public class CreateGenreRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Icon { get; set; }
    }
}
