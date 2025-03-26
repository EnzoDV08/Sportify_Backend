namespace SportifyApi.DTOs
{
    public class ProfileDto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }
        public string? Description { get; set; }
    }
}