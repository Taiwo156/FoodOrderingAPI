using ASPtask.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APItask.Service
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(string username, string password);
        Task<(bool Success, string ErrorMessage)> AddUserAsync(List<User> users);
        Task<ASPtask.Core.User> GetUserByIdAsync(int id);
        Task<(bool Success, string ErrorMessage)> DeleteUserAsync(int id);

        // Add this method
        Task<List<User>> GetAllUsersAsync();
        Task<User> UpdateUserAsync(User user);
    }
}
