using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportifyApi.Models
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("event_id")]
        public int EventId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        public string? Type { get; set; }
        public string? Visibility { get; set; }
        public string? Status { get; set; }

        public int? CreatorUserId { get; set; }
        public int? AdminId { get; set; }

        [ForeignKey("CreatorUserId")]
        public User? Creator { get; set; }

        [ForeignKey("AdminId")]
        public Admin? Admin { get; set; }

        public bool IsPrivate { get; set; } = false;
        public List<int>? InvitedUserIds { get; set; } 
        
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }


    }
}
