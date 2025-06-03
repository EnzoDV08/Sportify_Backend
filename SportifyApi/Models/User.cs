using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportifyApi.Models
{
    // This class represents the user in the database.
    public class User
    {
        // Primary key for user, auto-incremented by the database
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required] // Makes the field required
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
        public string UserType { get; set; } = "user"; // Makes it so that the default userType is "user"
    }
}
