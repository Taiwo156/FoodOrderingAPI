using APItask.Core.DTOs.Responses;
using APItask.Core.Models;
using APItask.Data;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace APItask.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public UserService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<Users?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<Users?> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            return user;
        }

        public async Task<List<Users>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<Users?> AddUserAsync(UserRegistrationDto registration)
        {
            if (await _userRepository.UsernameExistsAsync(registration.Username))
                return null;

            var user = new Users
            {
                Username = registration.Username,
                Email = registration.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registration.Password)
            };

            return await _userRepository.AddAsync(user);
        }

        public async Task<Users?> UpdateUserAsync(int id, UserUpdateDto updateDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            user.Username = updateDto.Username;
            user.Email = updateDto.Email;

            if (!string.IsNullOrEmpty(updateDto.NewPassword))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateDto.NewPassword);
            }

            return await _userRepository.UpdateAsync(user);
        }

        public async Task<(bool Success, string? ErrorMessage)> DeleteUserAsync(int id)
        {
            var result = await _userRepository.DeleteAsync(id);
            return result ? (true, null) : (false, "User not found");
        }

        public async Task<(bool Success, string? ErrorMessage)> RequestPasswordResetAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return (false, "Email not found");

            var token = Guid.NewGuid().ToString();
            var expiry = DateTime.UtcNow.AddHours(1);

            await _userRepository.UpdateResetTokenAsync(user.Id, token, expiry);
            return (true, token);
        }

        public async Task<(bool Success, string? ErrorMessage)> ResetPasswordAsync(string token, string newPassword)
        {
            var user = await _userRepository.GetByResetTokenAsync(token);
            if (user == null) return (false, "Invalid token");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.ResetToken = null;
            user.ResetTokenExpiry = null;

            await _userRepository.UpdateAsync(user);
            return (true, null);
        }
    }
}
