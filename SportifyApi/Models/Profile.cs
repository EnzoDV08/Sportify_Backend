using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportifyApi.Models
{
    public class Profile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("ProfilePicture")]
        public string? ProfilePicture { get; set; }

        [Column("Location")]
        public string? Location { get; set; } = string.Empty;

        [Column("Interests")]
        public string? Interests { get; set; } = string.Empty;

        [Column("FavoriteSports")]
        public string? FavoriteSports { get; set; } = string.Empty;

        [Column("Availability")]
        public string? Availability { get; set; } = string.Empty;

        [Column("Bio")]
        public string? Bio { get; set; } = string.Empty;

        [Column("PhoneNumber")]
        public string? PhoneNumber { get; set; } = string.Empty;

        [Column("SocialMediaLink")]
        public string? SocialMediaLink { get; set; } = string.Empty;

        [Column("Gender")]
        public string? Gender { get; set; } = string.Empty;

        [Column("Age")]
        public int? Age { get; set; }
    }
}
