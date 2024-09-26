using ASPtask.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APItask.Data
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> AuthenticateAsync(string username, string password);
        Task<List<User>> GetAllUsersAsync();
        Task<(bool Success, string ErrorMessage)> AddUserAsync(List<User> users);
        Task DeleteUserAsync(int id);
        Task<bool> UserExistsAsync(string username, string email);
    }
}
