using APItask.Core.Models;

public interface IUserRepository
{
    Task<Users?> GetByIdAsync(int id);
    Task<Users?> GetByUsernameAsync(string username);
    Task<Users?> GetByEmailAsync(string email);
    Task<List<Users>> GetAllAsync();
    Task<Users> AddAsync(Users user);
    Task<Users> UpdateAsync(Users user);
    Task<bool> DeleteAsync(int id);
    Task<bool> UsernameExistsAsync(string username);
    Task<bool> EmailExistsAsync(string email);
    Task<Users?> GetByResetTokenAsync(string token);
    Task UpdateResetTokenAsync(int userId, string token, DateTime expiry);
}