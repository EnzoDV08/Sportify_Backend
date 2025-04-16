public class ProfileUpdateDto
{
    public string? Name { get; set; }         // 👈 from Users
    public string? Email { get; set; }         // 👈 from Users
    public string? Password { get; set; }      // 👈 from Users
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
}
