using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportifyApi.Models
{
    // This class represents the profile of a user in the database.
    public class Profile
    {   
        // Primary Key for profile
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("ProfilePicture")]
        public string? ProfilePicture { get; set; } // ? makes the field nullable

        [Column("Location")]
        public string? Location { get; set; } = string.Empty; // Default value to avoid null reference

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

    [Column("TotalPoints")]
    public int TotalPoints { get; set; } = 0; // ✅ Add this line
}
