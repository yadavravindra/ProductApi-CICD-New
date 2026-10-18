
using System.ComponentModel.DataAnnotations;

namespace ProductApi.DTOs
{
    public class RegisterUserDto
    {
        [Required]
        public string EmpCode { get; set; }

        [Required]
        [MinLength(3)]
        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }

        public DateTime? DOB { get; set; }

        public int? Gender { get; set; }

        [Required]
        public int RoleId { get; set; }
    }

    public class LoginDto
    {
        [Required]
        public string EmpCode { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
