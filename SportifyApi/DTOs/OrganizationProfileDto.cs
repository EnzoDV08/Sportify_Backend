// DTOs/OrganizationProfileDto.cs
namespace SportifyApi.DTOs
{
    public class OrganizationProfileDto
    {
        public int OrganizationId { get; set; }
        public string? LogoUrl { get; set; }
        public string? Description { get; set; }
        public string? ContactNumber { get; set; }
        public string? Location { get; set; }
    }
}
