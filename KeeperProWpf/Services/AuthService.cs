using KeeperProWpf.Data;
using KeeperProWpf.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace KeeperProWpf.Services
{
    public class AuthService
    {
        public async Task<User?> LoginSqlAsync(string email, string password)
        {
            string hash = PasswordHelper.ToMd5(password);

            await using var con = new NpgsqlConnection(DbHelper.ConnectionString);
            await con.OpenAsync();

            string sql = @"
                SELECT user_id, email, password_hash, role_id, created_at, is_active
                FROM keeperpro.users
                WHERE email = @email
                  AND password_hash = @hash
                  AND is_active = true
                LIMIT 1;";

            await using var cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("email", email);
            cmd.Parameters.AddWithValue("hash", hash);

            await using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return new User
            {
                UserId = reader.GetInt32(0),
                Email = reader.GetString(1),
                PasswordHash = reader.GetString(2),
                RoleId = reader.GetInt32(3),
                CreatedAt = reader.GetDateTime(4),
                IsActive = reader.GetBoolean(5)
            };
        }

        public async Task<User?> LoginProcedureAsync(string email, string password)
        {
            string hash = PasswordHelper.ToMd5(password);

            await using var con = new NpgsqlConnection(DbHelper.ConnectionString);
            await con.OpenAsync();

            string sql = "SELECT * FROM keeperpro.login_user(@p_email, @p_password_hash);";

            await using var cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("p_email", email);
            cmd.Parameters.AddWithValue("p_password_hash", hash);

            await using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return new User
            {
                UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                Email = reader.GetString(reader.GetOrdinal("email")),
                PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")),
                RoleId = reader.GetInt32(reader.GetOrdinal("role_id")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("is_active"))
            };
        }

        public async Task<User?> LoginOrmAsync(string email, string password)
        {
            string hash = PasswordHelper.ToMd5(password);

            using var db = new AppDbContext();

            return await db.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x =>
                    x.Email == email &&
                    x.PasswordHash == hash &&
                    x.IsActive);
        }

        public async Task<(bool Success, string Message)> RegisterSqlAsync(string email, string password)
        {
            if (!PasswordHelper.IsPasswordValid(password))
                return (false, "Пароль не соответствует требованиям.");

            await using var con = new NpgsqlConnection(DbHelper.ConnectionString);
            await con.OpenAsync();

            string checkSql = "SELECT COUNT(*) FROM keeperpro.users WHERE email = @email;";
            await using (var checkCmd = new NpgsqlCommand(checkSql, con))
            {
                checkCmd.Parameters.AddWithValue("email", email);
                long count = (long)(await checkCmd.ExecuteScalarAsync() ?? 0);

                if (count > 0)
                    return (false, "Пользователь с таким email уже существует.");
            }

            int guestRoleId = await GetGuestRoleIdAsync(con);
            string hash = PasswordHelper.ToMd5(password);

            string insertSql = @"
                INSERT INTO keeperpro.users(email, password_hash, role_id, is_active)
                VALUES(@email, @password_hash, @role_id, true);";

            await using var cmd = new NpgsqlCommand(insertSql, con);
            cmd.Parameters.AddWithValue("email", email);
            cmd.Parameters.AddWithValue("password_hash", hash);
            cmd.Parameters.AddWithValue("role_id", guestRoleId);

            await cmd.ExecuteNonQueryAsync();

            return (true, "Регистрация через SQL выполнена.");
        }

        public async Task<(bool Success, string Message)> RegisterProcedureAsync(string email, string password)
        {
            if (!PasswordHelper.IsPasswordValid(password))
                return (false, "Пароль не соответствует требованиям.");

            string hash = PasswordHelper.ToMd5(password);

            await using var con = new NpgsqlConnection(DbHelper.ConnectionString);
            await con.OpenAsync();

            string sql = "CALL keeperpro.register_user(@p_email, @p_password_hash);";

            await using var cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("p_email", email);
            cmd.Parameters.AddWithValue("p_password_hash", hash);

            try
            {
                await cmd.ExecuteNonQueryAsync();
                return (true, "Регистрация через процедуру выполнена.");
            }
            catch (PostgresException ex)
            {
                return (false, ex.MessageText);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string Message)> RegisterOrmAsync(string email, string password)
        {
            if (!PasswordHelper.IsPasswordValid(password))
                return (false, "Пароль не соответствует требованиям.");

            using var db = new AppDbContext();

            bool exists = await db.Users.AnyAsync(x => x.Email == email);
            if (exists)
                return (false, "Пользователь с таким email уже существует.");

            int guestRoleId = await db.Roles
                .Where(x => x.RoleName == "guest")
                .Select(x => x.RoleId)
                .FirstOrDefaultAsync();

            if (guestRoleId == 0)
                return (false, "Роль guest не найдена в базе.");

            var user = new User
            {
                Email = email,
                PasswordHash = PasswordHelper.ToMd5(password),
                RoleId = guestRoleId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();

            return (true, "Регистрация через ORM выполнена.");
        }

        private static async Task<int> GetGuestRoleIdAsync(NpgsqlConnection con)
        {
            string sql = "SELECT role_id FROM keeperpro.roles WHERE role_name = 'guest' LIMIT 1;";

            await using var cmd = new NpgsqlCommand(sql, con);
            object? result = await cmd.ExecuteScalarAsync();

            if (result == null || result == DBNull.Value)
                throw new Exception("Роль guest не найдена в таблице roles.");

            return Convert.ToInt32(result);
        }
    }
}