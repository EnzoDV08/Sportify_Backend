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
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        public string? Type { get; set; }
        public string? Visibility { get; set; }
        public string? Status { get; set; }
        public string? RequiredItems { get; set; }
        public string? ImageUrl { get; set; }

        public int? CreatorUserId { get; set; }

        [ForeignKey("CreatorUserId")]
        public User? Creator { get; set; }

        public ICollection<EventParticipant> Participants { get; set; } = new List<EventParticipant>();

    }
}
