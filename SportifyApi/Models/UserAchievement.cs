using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SportifyApi.Models
{
    public class UserAchievement
    {
        [Key]
        public int UserAchievementId { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }

        [JsonIgnore] 
        public User User { get; set; } = null!;

        [ForeignKey("AchievementId")]
        public int AchievementId { get; set; }

        public Achievement Achievement { get; set; } = null!;

        [ForeignKey("EventId")]
        public int? EventId { get; set; }

        [JsonIgnore] 
        public Event? Event { get; set; }

        public DateTime DateAwarded { get; set; } = DateTime.UtcNow;
    }
}
