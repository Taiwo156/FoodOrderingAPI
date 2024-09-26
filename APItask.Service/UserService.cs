using APItask.Data;
using ASPtask.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APItask.Service
{
    public class UserService : IUserService
    {
        

        EssentialProductsDbContext _DBContext;

        public UserService(EssentialProductsDbContext essentialProductsDbContext)
        {
            _DBContext = essentialProductsDbContext;
            //_users.Add(new User { Id = 1, Username = "testuser", Password = "password", Email = "testuser@example.com" });
        }

        public async Task<ASPtask.Core.User> AuthenticateAsync(string username, string password)
        {
          
            return await _DBContext.User.Where(u => u.Username == username && u.Password == password).FirstOrDefaultAsync();
        }

        public async Task<(bool Success, string ErrorMessage)> AddUserAsync(List<User> users)
        {


            string errorMessage = null;
            bool status = false;
            try
            {
                foreach (User user in users)
                {
                    user.Id = 0;

                    _DBContext.Add(user);
                }
                await _DBContext.SaveChangesAsync();

                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                errorMessage = ex.Message;
            }

            return (status, errorMessage);
        }



        //public async Task<User> GetUserByIdAsync(int id)
        //{
        //    return await Task.Run(() => _users.FirstOrDefault(u => u.Id == id));
        //}

        public async Task<(bool Success, string ErrorMessage)> DeleteUserAsync(int id)
        {
            var user = await _DBContext.User.Where(u => u.Id == id).FirstOrDefaultAsync();

            if (user == null)
                return (false, "User not found");

            _DBContext.Remove(user);
            _DBContext.SaveChanges();
            return (true, null);
        }

        // Implement the new method
        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                return _DBContext.User.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while retrieving users: {ex.Message}");
            }
            return null;
        }


        //public override bool Equals(object? obj)
        //{
        //    return obj is UserService service &&
        //           EqualityComparer<List<User>>.Default.Equals(_users, service._users) &&
        //           EqualityComparer<EssentialProductsDbContext>.Default.Equals(_DBContext, service._DBContext);
        //}

        //Task<User> IUserService.AuthenticateAsync(string username, string password)
        //{
        //    throw new NotImplementedException();
        //}




        public async Task<User?> GetUserByIdAsync(int id)
        {
            try
            {
                return await _DBContext.User.Where(u => u.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while retrieving user: {ex.Message}");
            }
            // Return null if an exception occurs or if no user is found

            return null;
        }
        public async Task<User> UpdateUserAsync(User user)
        {
            var existingUser = await _DBContext.User.FindAsync(user.Id);
            if (existingUser == null) return null;

            existingUser.Username = user.Username;
            existingUser.Password = user.Password;
            existingUser.Email = user.Email;

            await _DBContext.SaveChangesAsync();
            return existingUser;
        }



    }

}
