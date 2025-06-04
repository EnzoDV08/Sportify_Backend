using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

    // ✅ Allow this to be included in JSON
    public Event Event { get; set; } = null!;

    public User User { get; set; } = null!;
}
}
