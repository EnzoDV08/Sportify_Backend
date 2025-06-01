// DTO class that used to update both the user and profile tables in the database
public class ProfileUpdateDto
{
    public string? Name { get; set; }   //Name, Email, and Password are in the User table           
    public string? Email { get; set; }         
    public string? Password { get; set; }      
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
