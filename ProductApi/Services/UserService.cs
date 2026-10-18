using Microsoft.IdentityModel.Tokens;
using ProductApi.DTOs;
using ProductApi.Helper;
using ProductApi.Models;
using ProductApi.Repositories;
using ProductApi.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class UserService : IUserService
{
    private readonly UserRepository _repo;
    private readonly IConfiguration _config;

    public UserService(UserRepository repo, IConfiguration config)
    {
        _repo = repo;
        _config = config;
    }

    public async Task RegisterUserAsync(RegisterUserDto dto)
    {
        await _repo.InsertUserAsync(dto);
    }

    public async Task<string> LoginAsync(LoginDto dto)
       {
        var user = await _repo.GetUserByEmpCodeAsync(dto.EmpCode);

        if (user == null)
            throw new Exception("Invalid EmpCode");

        bool isValid = PasswordHelper.VerifyPassword(dto.Password, user.PasswordHash);

        if (!isValid)
            throw new Exception("Invalid Password");

        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.EmpCode),
            new Claim(ClaimTypes.Role, user.RoleId.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"])
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}