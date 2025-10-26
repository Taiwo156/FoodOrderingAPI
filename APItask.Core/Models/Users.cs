using System.ComponentModel.DataAnnotations;

namespace APItask.Core.Models
{
    public class Users
    {
        public int Id { get; set; }

        [Required]
        [MinLength(5), MaxLength(500)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MinLength(5), MaxLength(500)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MinLength(5), MaxLength(500)]
        public string Email { get; set; } = string.Empty;

        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }
        //public byte[]? PasswordSalt { get; set; }
    }
}
