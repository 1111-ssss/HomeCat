using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.DTOs.Auth;
using Application.Interfaces.Auth;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Auth;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor
    )
    {
        _httpContextAccessor = httpContextAccessor;
    }
    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public string? GetJwtToken() => 
        _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

    public string? GetUsername() =>
        User?.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value;

    public int? GetUserId()
    {
        var subClaim = User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (int.TryParse(subClaim, out var id))
            return id;

        var nameIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(nameIdClaim, out var id2))
            return id2;

        return null;
    }
    public string? GetUserIp() => 
        _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
    public bool IsAdmin() => 
        User?.FindFirst(ClaimTypes.Role)?.Value == "Admin";

    public DateTime? GetExpirationFromClaims()
    {
        if (User == null || !User.Identity?.IsAuthenticated == true)
            return null;

        var expClaim = User.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;
        if (expClaim != null && long.TryParse(expClaim, out var expSeconds))
        {
            return DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;
        }
        return null;
    }
    public GenerateTokenDTO? GetTokenDTO()
    {
        if (User == null || !User.Identity?.IsAuthenticated == true)
            return null;

        var idClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        var usernameClaim = User.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value;
        var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

        if (idClaim == null || usernameClaim == null || roleClaim == null)
            return null;

        if (!int.TryParse(idClaim, out var userId))
            return null;

        return new GenerateTokenDTO(userId, usernameClaim, roleClaim);
    }
}