using Application.Interfaces.Auth;
using Application.Interfaces.Download;
using Application.Interfaces.Music;
using Application.Interfaces.Repositories;
using Infrastructure.Repositories;
using Infrastructure.Services.Auth;
using Infrastructure.Services.Download;
using Infrastructure.Services.Init;
using Infrastructure.Services.Music;

namespace API.Extensions.Bootstrapper;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // HttpContextAccessor
        services.AddHttpContextAccessor();

        // OpenApi
        services.AddOpenApi();

        // Init
        services.AddScoped<DataBaseInitializer>();

        // Auth
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IJwtGeneratorService, JwtGeneratorService>();
        services.AddScoped<IPasswordHashingService, PasswordHashingService>();
        services.AddScoped<IRadioService, RadioService>();
        services.AddScoped<IStorageService, StorageService>();
        services.AddScoped<IAudioUtilsService, AudioUtilsService>();

        //Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFileEntryRepository, FileEntryRepository>();

        return services;
    }
}