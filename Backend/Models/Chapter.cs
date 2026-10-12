using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ServerCSharp.Models
{
    public class Chapter
    {
        public int Id { get; set; }

        public int StoryId { get; set; }

        [ForeignKey("StoryId")]
        public Story? Story { get; set; }

        public int ChapterNumber { get; set; }

        [Required]
        [MaxLength(250)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public int Views { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public List<ReadingHistory> ReadingHistories { get; set; } = new();

        [JsonIgnore]
        public List<Comment> Comments { get; set; } = new();
    }
}
