using ProductApi.DTOs;

namespace ProductApi.Services
{
    public interface IUserService
    {
        Task RegisterUserAsync(RegisterUserDto dto);
        Task<string> LoginAsync(LoginDto dto);
    }
}
