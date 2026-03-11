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
    public async Task<IEnumerable<FileEntry>> GetAllWithPagination(int page, int pageSize)
    {
        using var connection = new SqliteConnection(_connectionString);
        var offset = (page - 1) * pageSize;
        var sql = @"SELECT * FROM FileEntries LIMIT @PageSize OFFSET @Offset";
        return await connection.QueryAsync<FileEntry>(sql, new { PageSize = pageSize, Offset = offset });
    }
    public async Task<IEnumerable<FileEntry>> GetAllWithPagination(string? search, int page, int pageSize)
    {
        if (string.IsNullOrEmpty(search))
            return await GetAllWithPagination(page, pageSize);
            
        using var connection = new SqliteConnection(_connectionString);
        var offset = (page - 1) * pageSize;
        
        var searchPattern = string.IsNullOrEmpty(search) ? "" : search;
        var sql = @"SELECT * FROM FileEntries 
                    WHERE FileName LIKE '%' || @Search || '%' 
                    LIMIT @PageSize OFFSET @Offset";

        return await connection.QueryAsync<FileEntry>(sql, new 
            { 
                Search = searchPattern, 
                PageSize = pageSize, 
                Offset = offset 
            }
        );
    }
    public async Task<FileEntry?> GetByFileUrl(string url)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = "SELECT * FROM FileEntries WHERE FileUrl = @Url";
        return await connection.QuerySingleOrDefaultAsync<FileEntry>(sql, new { Url = url });
    }
}
