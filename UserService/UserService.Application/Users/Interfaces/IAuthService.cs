
using UserService.Application.Users.DTOs;

namespace UserService.Application.Users.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterUserRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}
