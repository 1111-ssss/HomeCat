using System.Diagnostics;
using Application.Interfaces.Auth;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.Init;

public class DataBaseInitializer
{
    private readonly IConfigurationSection _database;
    private readonly IConfigurationSection _defaultAdmins;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHashingService _passwordHashingService;
    private readonly ILogger<DataBaseInitializer> _logger;

    public DataBaseInitializer(
        IConfiguration configuration,
        IUserRepository userRepository, 
        IPasswordHashingService passwordHashingService,
        ILogger<DataBaseInitializer> logger
    )
    {
        _defaultAdmins = configuration.GetSection("DefaultAdmins")
            ?? throw new ArgumentNullException(nameof(configuration));

        _database = configuration.GetSection("Database")
            ?? throw new ArgumentNullException(nameof(configuration));

        _userRepository = userRepository;
        _passwordHashingService = passwordHashingService;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        var dbPath = _database["Path"];
        if (!File.Exists(dbPath))
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var sql = await File.ReadAllTextAsync("../init-db.sql");
            using var command = connection.CreateCommand();
            command.CommandText = sql;
            await command.ExecuteNonQueryAsync();
        }

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
