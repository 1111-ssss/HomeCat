using Application.Interfaces.Repositories;
using Dapper;
using Domain.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<int> AddAsync(User user)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = @"INSERT INTO Users (Username, PasswordHash, IsAdmin) VALUES (@Username, @PasswordHash, @IsAdmin); SELECT last_insert_rowid();";
        return await connection.ExecuteScalarAsync<int>(sql, user);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = "SELECT * FROM Users WHERE Id = @Id";
        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = "SELECT * FROM Users WHERE Username = @Username";
        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Username = username });
    }
}
