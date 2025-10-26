using System.ComponentModel.DataAnnotations;

namespace APItask.Core.DTOs.Requests
{
    public class LoginRequest
    {
        [Required]
        [MinLength(5), MaxLength(500)]
        public string? Username { get; set; }

        [Required]
        [MinLength(5), MaxLength(500)]
        public string? Password { get; set; }
    }
}
