using Application.Interfaces.Auth;
using Infrastructure.Services.Auth;

namespace API.Extensions.Bootstrapper;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // HttpContextAccessor
        services.AddHttpContextAccessor();

        // OpenApi
        services.AddOpenApi();

        // Auth
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IJwtGeneratorService, JwtGeneratorService>();
        services.AddScoped<IPasswordHashingService, PasswordHashingService>();

        return services;
    }
}