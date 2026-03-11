using Application.Interfaces.Repositories;
using Dapper;
using Domain.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories;

public class FileEntryRepository : IFileEntryRepository
{
    private readonly string _connectionString;

    public FileEntryRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<int> AddAsync(FileEntry entry)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = @"INSERT INTO FileEntries (FileName, ContentType, Path, UploadedById) VALUES (@FileName, @ContentType, @Path, @UploadedById); SELECT last_insert_rowid();";
        return await connection.ExecuteScalarAsync<int>(sql, entry);
    }

    public async Task<FileEntry?> GetByIdAsync(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = "SELECT * FROM FileEntries WHERE Id = @Id";
        return await connection.QuerySingleOrDefaultAsync<FileEntry>(sql, new { Id = id });
    }
    public async Task<FileEntry?> GetBy(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = "SELECT * FROM FileEntries WHERE Id = @Id";
        return await connection.QuerySingleOrDefaultAsync<FileEntry>(sql, new { Id = id });
    }
    public async Task<FileEntry?> GetByFileUrl(string url)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = "SELECT * FROM FileEntries WHERE FileUrl = @Url";
        return await connection.QuerySingleOrDefaultAsync<FileEntry>(sql, new { Url = url });
    }
}
