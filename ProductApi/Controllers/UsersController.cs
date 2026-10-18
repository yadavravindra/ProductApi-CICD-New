using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.DTOs;
using ProductApi.Services;

namespace ProductApi.Controllers
{
    

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            try
            {
                await _service.RegisterUserAsync(dto);

                return Ok(new
                {
                    message = "User registered successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var token = await _service.LoginAsync(dto);

                return Ok(new
                {
                    message = "Login successful",
                    token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
    }
}
