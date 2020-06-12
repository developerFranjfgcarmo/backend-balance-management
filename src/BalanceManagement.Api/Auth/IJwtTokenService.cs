using BalanceManagement.Contracts.Dtos.Users;

namespace BalanceManagement.Api.Auth
{
    public interface IJwtTokenService
    {
        UserAuthenticatedDto GenerateToken(UserDto user);
    }
}
