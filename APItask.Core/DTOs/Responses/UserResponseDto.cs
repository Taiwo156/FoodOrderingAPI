public class UserResponseDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    // Exclude ALL sensitive fields!
}