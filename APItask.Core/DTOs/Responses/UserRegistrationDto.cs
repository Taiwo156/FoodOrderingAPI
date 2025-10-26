using System.ComponentModel.DataAnnotations;

public class UserRegistrationDto
{
    [Required]
    [MinLength(5), MaxLength(500)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]  // Enforce stronger passwords
    public string Password { get; set; } = string.Empty;
}