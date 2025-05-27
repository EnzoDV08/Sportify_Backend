namespace SportifyApi.DTOs
{
    // Data Transfer Object for Profile
    // Used to send and receive profile data without exposing the full entity
    public class ProfileDto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }
        public string? Description { get; set; }
        public List<AchievementDto> Achievements { get; set; } = new();
   }
}