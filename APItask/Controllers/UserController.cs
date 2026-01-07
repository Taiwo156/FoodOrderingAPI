using APItask.Core.DTOs.Responses;
using APItask.Service;
using Microsoft.AspNetCore.Mvc;
using ForgotPasswordRequest = APItask.Core.DTOs.Requests.ForgotPasswordRequest;
using LoginRequest = APItask.Core.DTOs.Requests.LoginRequest;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        if (users == null || !users.Any())
            return NotFound(new { message = "No users found" });

        return Ok(users.Select (u => new UserResponseDto
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email

        }));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponseDto>> GetUser(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
            return NotFound(new { message = "User not found" });

        return Ok(new UserResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid login data");

        var user = await _userService.AuthenticateAsync(model.Username, model.Password);

        return user == null
            ? Unauthorized("Invalid username or password")
            : Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationDto registration)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { message = "Invalid registration data" });

        var user = await _userService.AddUserAsync(registration);

        return user == null
            ? BadRequest(new { message = "Username already exists" })
            : Ok(new { message = "User registered successfully" });
    }

    [HttpPost("request-reset")]
    public async Task<IActionResult> RequestPasswordReset([FromBody] ForgotPasswordRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { message = "Invalid email" });

        var result = await _userService.RequestPasswordResetAsync(request.Email);
        if (!result.Success)
            return BadRequest(new { message = result.ErrorMessage });

        return Ok(new { message = "If this email is registered, you'll receive a reset link" });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(new { message = "Validation failed", errors });
        }

        if (string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.NewPassword))
            return BadRequest(new { message = "Token and new password are required" });

        var result = await _userService.ResetPasswordAsync(request.Token, request.NewPassword);
        if (!result.Success)
            return BadRequest(new { message = "Password reset failed", error = result.ErrorMessage });

        return Ok(new { message = "Password reset successful" });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserResponseDto>> UpdateUser(int id, [FromBody] UserUpdateDto updateDto)
    {
        var user = await _userService.UpdateUserAsync(id, updateDto);
        if (user == null) return NotFound();

        return Ok(new UserResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _userService.DeleteUserAsync(id);
        if (!result.Success)
            return BadRequest(new { message = result.ErrorMessage });

        return Ok(new { message = "User deleted successfully" });
    }
}
