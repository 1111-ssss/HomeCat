using Application.Interfaces.Auth;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services.Auth;

public class AdminInitializer
{
    private readonly IConfigurationSection _defaultAdmins;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHashingService _passwordHashingService;

    public AdminInitializer(IConfiguration configuration, IUserRepository userRepository, IPasswordHashingService passwordHashingService)
    {
        _defaultAdmins = configuration.GetSection("DefaultAdmins")
            ?? throw new ArgumentNullException(nameof(configuration));
        _userRepository = userRepository;
        _passwordHashingService = passwordHashingService;
    }

    public async Task InitializeAsync()
    {
        foreach (var user in _defaultAdmins.GetChildren()) {
            var login = user["Login"];
            var password = user["Password"];
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                continue;

            var existing = await _userRepository.GetByUsernameAsync(login);
            if (existing != null && existing.IsAdmin)
                continue;

            var hashResult = _passwordHashingService.HashPassword(password);
            if (!hashResult.IsSuccess)
                continue;

            var adminUser = new User
            {
                Username = login,
                PasswordHash = hashResult.Value,
                IsAdmin = true
            };
            await _userRepository.AddAsync(adminUser);
        }
    }
}
