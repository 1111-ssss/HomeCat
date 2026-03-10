using Application.DTOs.Auth;

namespace Application.Interfaces.Auth;

public interface ICurrentUserService
{
    string? GetUsername();
    int? GetUserId();
    bool IsAdmin();
    string? GetUserIp();
    string? GetJwtToken();

    GenerateTokenDTO? GetTokenDTO();
    DateTime? GetExpirationFromClaims();
}