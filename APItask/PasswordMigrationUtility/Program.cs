using APItask.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BCrypt.Net; // Add this using directive

namespace PasswordMigrationUtility
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<EssentialProductsDbContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DbContext"));

            using var db = new EssentialProductsDbContext(optionsBuilder.Options, config);

            Console.WriteLine("Starting password migration...");

            try
            {
                var users = await db.Users
                    .Where(u => string.IsNullOrEmpty(u.PasswordHash))
                    .ToListAsync();

                Console.WriteLine($"Found {users.Count} users to migrate");

                foreach (var user in users)
                {
                    Console.WriteLine($"Migrating user {user.Id} ({user.Username})...");
                    string plainPassword = await GetLegacyPasswordAsync(user.Id);

                    // Correct BCrypt hashing:
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(plainPassword);
                }

                await db.SaveChangesAsync();
                Console.WriteLine("Migration completed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Migration failed: {ex.Message}");
            }
        }

        static async Task<string> GetLegacyPasswordAsync(int userId)
        {
            return await Task.FromResult($"TempPwdFor{userId}");
        }
    }
}