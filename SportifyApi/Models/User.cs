using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportifyApi.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("password")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Column("user_type")]
        public string UserType { get; set; } = "user"; // user, admin, creator

        public ICollection<Event> CreatedEvents { get; set; } = new List<Event>();

        public ICollection<EventParticipant> EventParticipants { get; set; } = new List<EventParticipant>();
    }
}
