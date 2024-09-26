using System.ComponentModel.DataAnnotations;

namespace ASPtask.Core
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MinLength(5), MaxLength(500)]
        public string? Username { get; set; }

        [Required]
        [MinLength(5), MaxLength(500)]
        public string? Password { get; set; }

        [Required]
        [MinLength(5), MaxLength(500)]
        public string? Email { get; set; }  
    }
}
