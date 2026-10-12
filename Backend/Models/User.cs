using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ServerCSharp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [JsonIgnore]
        public string Password { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Role { get; set; } = "reader"; // 'admin', 'author', 'reader'

        [MaxLength(20)]
        public string Status { get; set; } = "active"; // 'active', 'banned'

        public string? Avatar { get; set; }
        public string? Bio { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [JsonIgnore]
        public List<Story> Stories { get; set; } = new();

        [JsonIgnore]
        public List<Favorite> Favorites { get; set; } = new();

        [JsonIgnore]
        public List<ReadingHistory> ReadingHistories { get; set; } = new();

        [JsonIgnore]
        public List<Comment> Comments { get; set; } = new();

        [JsonIgnore]
        public List<Report> Reports { get; set; } = new();
    }
}
