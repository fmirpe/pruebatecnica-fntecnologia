using System.ComponentModel.DataAnnotations;

namespace UserService.Application.Users.DTOs
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = default!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = default!;
    }
}
