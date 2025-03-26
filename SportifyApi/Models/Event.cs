using System;
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

        [Required]
        public DateTime Date { get; set; }

        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        public string? Type { get; set; }

        public string? Visibility { get; set; }

        public string? Status { get; set; }
    }
}
