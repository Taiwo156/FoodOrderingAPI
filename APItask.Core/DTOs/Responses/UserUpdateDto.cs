using System.ComponentModel.DataAnnotations;
using APItask.Core.Models;

namespace APItask.Core.DTOs.Responses
{
    public class UserUpdateDto
    {
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? NewPassword { get; set; } // Optional for password updates

        // Conversion method to User entity
        public Users ToUser()
        {
            return new Users
            {
                Id = Id,
                Username = Username,
                Email = Email
            };
        }
    }
}