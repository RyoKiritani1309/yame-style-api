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
        Task<bool> UpdateProfileAsync(int userId, string? fullName, string? phone, string? address);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<string?> GeneratePasswordResetTokenAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
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

        public async Task<bool> UpdateProfileAsync(int userId, string? fullName, string? phone, string? address)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
                    UPDATE Customers 
                    SET FullName = @FullName, 
                        Phone = @Phone, 
                        Address = @Address
                    WHERE UserId = @UserId";

                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                    cmd.Parameters.Add(new SqlParameter("@FullName", (object?)fullName ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Phone", (object?)phone ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Address", (object?)address ?? DBNull.Value));

                    var rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null || !VerifyPassword(currentPassword, user.PasswordHash))
                return false;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "UPDATE Users SET PasswordHash = @PasswordHash WHERE UserId = @UserId";

                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                    cmd.Parameters.Add(new SqlParameter("@PasswordHash", HashPassword(newPassword)));

                    var rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<string?> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await GetUserByEmailAsync(email);
            if (user == null)
                return null;

            // Generate a simple token (in production, use a more secure approach)
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 32);
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Store token with expiration (1 hour)
                var query = @"
                    IF EXISTS (SELECT 1 FROM PasswordResetTokens WHERE Email = @Email)
                        UPDATE PasswordResetTokens SET Token = @Token, ExpiresAt = DATEADD(HOUR, 1, GETDATE())
                        WHERE Email = @Email
                    ELSE
                        INSERT INTO PasswordResetTokens (Email, Token, ExpiresAt)
                        VALUES (@Email, @Token, DATEADD(HOUR, 1, GETDATE()))";

                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@Email", email));
                    cmd.Parameters.Add(new SqlParameter("@Token", token));
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return token;
        }

        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Verify token
                var verifyQuery = @"
                    SELECT Email FROM PasswordResetTokens 
                    WHERE Email = @Email AND Token = @Token AND ExpiresAt > GETDATE()";

                using (var cmd = new SqlCommand(verifyQuery, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@Email", email));
                    cmd.Parameters.Add(new SqlParameter("@Token", token));

                    var result = await cmd.ExecuteScalarAsync();
                    if (result == null)
                        return false;
                }

                // Update password
                var updateQuery = @"
                    UPDATE Users SET PasswordHash = @PasswordHash WHERE Email = @Email";

                using (var cmd = new SqlCommand(updateQuery, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@Email", email));
                    cmd.Parameters.Add(new SqlParameter("@PasswordHash", HashPassword(newPassword)));

                    var rowsAffected = await cmd.ExecuteNonQueryAsync();
                    
                    if (rowsAffected > 0)
                    {
                        // Delete used token
                        var deleteQuery = "DELETE FROM PasswordResetTokens WHERE Email = @Email";
                        using (var deleteCmd = new SqlCommand(deleteQuery, connection))
                        {
                            deleteCmd.Parameters.Add(new SqlParameter("@Email", email));
                            await deleteCmd.ExecuteNonQueryAsync();
                        }
                        return true;
                    }
                }
            }

            return false;
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
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
