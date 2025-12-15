
namespace UserService.Application.Users.DTOs
{
    public class AuthResponse
    {
        public Guid UserId { get; set; }

        public string Name { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string Role { get; set; } = default!;

        // Luego aquí agregaremos el Token JWT.
        public string? Token { get; set; }
    }
}
