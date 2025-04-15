using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportifyApi.Models
{
    public class UserAchievement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int AchievementId { get; set; }

        public int? EventId { get; set; }

        public int? AwardedByAdminId { get; set; }

        public DateTime AwardedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = null!;
        public Achievement Achievement { get; set; } = null!;
        public Event? Event { get; set; }
        public Admin? AwardedByAdmin { get; set; }
    }
}
