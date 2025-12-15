using UserService.Domain.Entities;

namespace UserService.Application.Auth.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);

    }
}
