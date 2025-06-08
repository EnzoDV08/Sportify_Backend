// DTOs/UpdateOrganizationDto.cs
namespace SportifyApi.DTOs
{
    public class UpdateOrganizationDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Website { get; set; }
        public string? ContactPerson { get; set; }
    }
}
