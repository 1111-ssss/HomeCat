using Application.DTOs.Auth;

namespace Application.Interfaces.Auth;

public interface IJwtGeneratorService
{
    string? GenerateToken(GenerateTokenDTO username);
}