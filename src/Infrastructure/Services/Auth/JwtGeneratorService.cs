using Application.DTOs.Auth;
using Application.Interfaces.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services.Auth;

public class JwtGeneratorService : IJwtGeneratorService
{
    private readonly ILogger<JwtGeneratorService> _logger;
    private readonly IConfigurationSection _jwtSection;
    public JwtGeneratorService(
        IConfiguration config,
        ILogger<JwtGeneratorService> logger
    )
    {
        _logger = logger;
        _jwtSection = config.GetSection("Jwt");

        if (_jwtSection["Key"] == null)
            throw new Exception("Не найден ключ для JWT");
    }
    public string? GenerateToken(GenerateTokenDTO dto)
    {
        try {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, dto.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, dto.Username),
                new Claim(ClaimTypes.Role, dto.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _jwtSection["Key"]!
            ));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(_jwtSection["Jwt:Expires"] ?? "30")
                ),
                audience: _jwtSection["Jwt:Audience"],
                issuer: _jwtSection["Jwt:Issuer"],
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Ошибка при генерации JWT токена");
            return null;
        }
    }
}