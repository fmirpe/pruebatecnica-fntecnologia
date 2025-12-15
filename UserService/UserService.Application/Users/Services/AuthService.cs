
using System.Security.Cryptography;
using System.Text;
using UserService.Application.Auth.Interfaces;
using UserService.Application.Users.DTOs;
using UserService.Application.Users.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Enums;
using UserService.Domain.Repositories;

namespace UserService.Application.Users.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }
        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email.Trim().ToLowerInvariant());
            if (user is null)
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var hash = HashPassword(request.Password);
            if (!string.Equals(user.PasswordHash, hash, StringComparison.Ordinal))
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var token = _tokenService.GenerateToken(user);

            return new AuthResponse
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
                Token = token
            };
        }

        public async Task<AuthResponse> RegisterAsync(RegisterUserRequest request)
        {
            // Validaciones de negocio adicionales a los DataAnnotations

            // Email único en base de datos
            var exists = await _userRepository.EmailExistsAsync(request.Email);
            if (exists)
            {
                throw new InvalidOperationException("Email is already registered.");
            }

            // Rol válido
            if (!Enum.TryParse<UserRole>(request.Role, ignoreCase: true, out var role))
            {
                throw new InvalidOperationException("Invalid role. Must be Admin or User.");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Email = request.Email.Trim().ToLowerInvariant(),
                PasswordHash = HashPassword(request.Password),
                Role = role,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            var token = _tokenService.GenerateToken(user);

            return new AuthResponse
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
                Token = token
            };
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
