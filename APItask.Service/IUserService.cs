using APItask.Core.DTOs.Responses;
using APItask.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APItask.Service
{
    public interface IUserService
    {
       
        Task<Users?> GetUserByIdAsync(int id);
        Task<Users?> AuthenticateAsync(string username, string password);
        Task<List<Users>> GetAllUsersAsync();
        Task<Users?> AddUserAsync(UserRegistrationDto registration);
        Task<Users?> UpdateUserAsync(int id, UserUpdateDto updateDto);
        Task<(bool Success, string? ErrorMessage)> DeleteUserAsync(int id);
        Task<(bool Success, string? ErrorMessage)> RequestPasswordResetAsync(string email);
        Task<(bool Success, string? ErrorMessage)> ResetPasswordAsync(string token, string newPassword);
    }
}