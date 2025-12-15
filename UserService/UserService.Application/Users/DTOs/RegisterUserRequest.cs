using System.ComponentModel.DataAnnotations;

namespace UserService.Application.Users.DTOs
{
    public class RegisterUserRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = default!;

        [Required]
        [EmailAddress] // Email con formato válido
        [MaxLength(256)]
        public string Email { get; set; } = default!;

        [Required]
        [MinLength(6)] // Contraseña mínimo 6 caracteres
        public string Password { get; set; } = default!;

        [Required]
        [RegularExpression("Admin|User", ErrorMessage = "Role must be Admin or User")]
        public string Role { get; set; } = default!;
    }
}
