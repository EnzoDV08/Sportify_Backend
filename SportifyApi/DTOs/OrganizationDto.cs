// DTOs/OrganizationDto.cs
namespace SportifyApi.DTOs
{
    public class OrganizationDto
    {
        public int OrganizationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Password { get; set; }
        public string? Website { get; set; }
        public string? ContactPerson { get; set; }
    }
}
