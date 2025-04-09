using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportifyApi.Models
{
    public class EventParticipant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("event_participant_id")]
        public int Id { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        // Relationships
       public Event? Event { get; set; }
       public User? User { get; set; }
    }
}
