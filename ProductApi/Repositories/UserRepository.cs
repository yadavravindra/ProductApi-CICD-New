using Microsoft.Data.SqlClient;
using ProductApi.DTOs;
using ProductApi.Helper;
using ProductApi.Models;
using System.Data;

namespace ProductApi.Repositories
{
    public class UserRepository
    {
        private readonly IConfiguration _config;
        public UserRepository(IConfiguration config)
        {
            _config = config;
        }
        private SqlConnection GetConnection()
        {
            return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        }
        public async Task InsertUserAsync(RegisterUserDto dto)
        {
            using var con = GetConnection();
            using var cmd = new SqlCommand("sp_InsertUser", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@EmpCode", dto.EmpCode);
            cmd.Parameters.AddWithValue("@FirstName", dto.FirstName);
            cmd.Parameters.AddWithValue("@MiddleName", (object?)dto.MiddleName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@LastName", dto.LastName);
            cmd.Parameters.AddWithValue("@PasswordHash", PasswordHelper.HashPassword(dto.Password));
            cmd.Parameters.AddWithValue("@DOB", (object?)dto.DOB ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Gender", (object?)dto.Gender ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@RoleId", dto.RoleId);

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<User?> GetUserByEmpCodeAsync(string empCode)
        {
            using var con = GetConnection();
            using var cmd = new SqlCommand("sp_GetUserByEmpCode", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@EmpCode", empCode);

            await con.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = (int)reader["Id"],
                    EmpCode = reader["EmpCode"].ToString(),
                    FirstName = reader["FirstName"].ToString(),
                    PasswordHash = reader["PasswordHash"].ToString(),
                    RoleId = (int)reader["RoleId"]
                };
            }

            return null;
        }
    }
}
