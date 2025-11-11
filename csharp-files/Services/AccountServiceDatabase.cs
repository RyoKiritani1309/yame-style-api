using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace YameApi.Services
{
    public interface IAccountService
    {
        Task<UserAccount?> RegisterAsync(string email, string password, string? fullName, string? phone);
        Task<UserAccount?> LoginAsync(string email, string password);
        Task<UserAccount?> GetUserByIdAsync(int userId);
        Task<UserAccount?> GetUserByEmailAsync(string email);
    }

    public class AccountServiceDatabase : IAccountService
    {
        private readonly string _connectionString;

        public AccountServiceDatabase(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("YameDB") 
                ?? throw new InvalidOperationException("Database connection string 'YameDB' not found");
        }

        public async Task<UserAccount?> RegisterAsync(string email, string password, string? fullName, string? phone)
        {
            // Check if email already exists
            var existingUser = await GetUserByEmailAsync(email);
            if (existingUser != null)
                return null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
                    INSERT INTO Users (Email, PasswordHash, CreatedAt)
                    OUTPUT INSERTED.UserId, INSERTED.Email, INSERTED.PasswordHash, INSERTED.CreatedAt
                    VALUES (@Email, @PasswordHash, GETDATE())";

                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@Email", email));
                    cmd.Parameters.Add(new SqlParameter("@PasswordHash", HashPassword(password)));

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var userId = reader.GetInt32(0);
                            reader.Close();

                            // Create customer profile
                            var customerQuery = @"
                                INSERT INTO Customers (UserId, FullName, Phone)
                                VALUES (@UserId, @FullName, @Phone)";

                            using (var customerCmd = new SqlCommand(customerQuery, connection))
                            {
                                customerCmd.Parameters.Add(new SqlParameter("@UserId", userId));
                                customerCmd.Parameters.Add(new SqlParameter("@FullName", (object?)fullName ?? DBNull.Value));
                                customerCmd.Parameters.Add(new SqlParameter("@Phone", (object?)phone ?? DBNull.Value));
                                await customerCmd.ExecuteNonQueryAsync();
                            }

                            return new UserAccount
                            {
                                UserId = userId,
                                Email = email,
                                FullName = fullName,
                                Phone = phone,
                                CreatedAt = DateTime.Now
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<UserAccount?> LoginAsync(string email, string password)
        {
            var user = await GetUserByEmailAsync(email);
            
            if (user == null || !VerifyPassword(password, user.PasswordHash))
                return null;

            return user;
        }

        public async Task<UserAccount?> GetUserByIdAsync(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
                    SELECT u.UserId, u.Email, u.PasswordHash, u.CreatedAt,
                           c.FullName, c.Phone
                    FROM Users u
                    LEFT JOIN Customers c ON u.UserId = c.UserId
                    WHERE u.UserId = @UserId";

                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@UserId", userId));

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new UserAccount
                            {
                                UserId = reader.GetInt32(0),
                                Email = reader.GetString(1),
                                PasswordHash = reader.GetString(2),
                                CreatedAt = reader.GetDateTime(3),
                                FullName = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Phone = reader.IsDBNull(5) ? null : reader.GetString(5)
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<UserAccount?> GetUserByEmailAsync(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
                    SELECT u.UserId, u.Email, u.PasswordHash, u.CreatedAt,
                           c.FullName, c.Phone
                    FROM Users u
                    LEFT JOIN Customers c ON u.UserId = c.UserId
                    WHERE u.Email = @Email";

                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@Email", email));

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new UserAccount
                            {
                                UserId = reader.GetInt32(0),
                                Email = reader.GetString(1),
                                PasswordHash = reader.GetString(2),
                                CreatedAt = reader.GetDateTime(3),
                                FullName = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Phone = reader.IsDBNull(5) ? null : reader.GetString(5)
                            };
                        }
                    }
                }
            }

            return null;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "YAME_SALT"));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            var newHash = HashPassword(password);
            return newHash == hash;
        }
    }

    // User model
    public class UserAccount
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
