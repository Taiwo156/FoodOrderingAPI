using ASPtask.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APItask.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly EssentialProductsDbContext _context;

        public UserRepository(EssentialProductsDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Id == id)!;
        }


        public async Task<User> AuthenticateAsync(string username, string password)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Username == username && u.Password == password); // Changed Users to User
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.User.ToListAsync(); 
        }

        //public async Task AddUserAsync(User user)
        //{
        //    _context.User.Add(user); 
        //    await _context.SaveChangesAsync();
        //}
        public async Task<(bool Success, string ErrorMessage)> AddUserAsync(List<User> users)
        {
            try
            {
                foreach (var user in users)
                {
                    var exists = await _context.User.AnyAsync(u => u.Username == user.Username || u.Email == user.Email);
                    if (exists)
                    {
                        return (false, $"User with username {user.Username} or email {user.Email} already exists.");
                    }

                    user.Id = 0;  // Assuming ID is auto-generated
                    _context.User.Add(user);  // Add each user
                }

                await _context.SaveChangesAsync();  // Save all changes
                return (true, null);  // Return success
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return (false, $"An error occurred: {ex.Message}");
            }
        }



        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == id); 
            if (user != null)
            {
                _context.User.Remove(user); 
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> UserExistsAsync(string username, string email)
        {
            return await _context.User.AnyAsync(u => u.Username == username || u.Email == email); 
        }
    }
}
