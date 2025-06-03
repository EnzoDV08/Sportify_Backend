// Models/OrganizationProfile.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportifyApi.Models
{
    public class OrganizationProfile
    {
        [Key]
        [ForeignKey("Organization")]
        [Column("organization_id")]
        public int OrganizationId { get; set; }

        [Column("logo_url")]
        public string? LogoUrl { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("contact_number")]
        public string? ContactNumber { get; set; }

        [Column("location")]
        public string? Location { get; set; }

        public Organization? Organization { get; set; }
    }
}
