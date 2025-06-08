namespace SportifyApi.DTOs
{
    // Data Transfer Object for Profile
    public class ProfileDto
    {
        public int UserId { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Location { get; set; }
        public string? Interests { get; set; }
        public string? FavoriteSports { get; set; }
        public string? Availability { get; set; }
        public string? Bio { get; set; }
        public string? PhoneNumber { get; set; }
        public string? SocialMediaLink { get; set; }
        public string? Gender { get; set; }
        public int? Age { get; set; }
        public int TotalPoints { get; set; }
    }
}
