using APItask.Service;
using ASPtask.Core;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace APItask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;



        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// // GET api/users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            if (users == null || users.Count == 0)
                return NotFound(new { message = "No users found" });

            return Ok(users);
        }

        /// <summary>
        /// // POST api/users/login
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult> Login(string username, string password)
        {
            var user = await _userService.AuthenticateAsync(username, password);
            if (user == null)
                return Unauthorized(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        /// <summary>
        /// // POST api/users/register
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] List<User> users)
        {
            var result = await _userService.AddUserAsync(users);
            if (!result.Success)
                return BadRequest(new { message = result.ErrorMessage });

            return Ok(new { message = "User(s) registered successfully" });
        }


        /// <summary>
        /// // GET api/users/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUser(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound();

                return Ok(user);
            }

            catch (Exception ex)
            {
                // Log the exception and return a 500 status code
                Console.WriteLine($"Error retrieving user: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

            //return _userService.GetUserByIdAsync(id);
        }

        /// <summary>
        /// // PUT: api/user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.Id)
            {
                return BadRequest("User ID mismatch.");
            }

            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound("User not found.");
            }

            var updatedUser = await _userService.UpdateUserAsync(user);
            if (updatedUser == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating user.");
            }

            return Ok("User updated successfully.");
        }

        /// <summary>
        /// // DELETE api/users/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result.Success)
                return BadRequest(new { message = result.ErrorMessage });

            return Ok(new { message = "User deleted successfully" });
        }
    }
}