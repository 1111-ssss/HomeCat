using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions.Bootstrapper;

public static class AuthConfigurationExtensions
{
    public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Authentication + Authorization
        var jwtSettings = configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new ArgumentNullException("Jwt:Key пустой в конфигурации"));

        services.AddAuthentication(options =>
            {
                options.DefaultScheme = "HomeCatAuth";
                options.DefaultAuthenticateScheme = "HomeCatAuth";
                options.DefaultChallengeScheme = "HomeCatAuth";
            })
            .AddPolicyScheme("HomeCatAuthOrJwt", "Cookie", options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    var hasBearer = context.Request.Headers["Authorization"].ToString().StartsWith("Bearer ");
                    return hasBearer ? JwtBearerDefaults.AuthenticationScheme : "HomeCatAuth";
                };
            })
            .AddCookie("HomeCatAuth", options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Error/403";
                options.Cookie.Name = "HomeCatAuth";
            })

        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };
        });
        services.AddAuthorization();

        return services;
    }
}