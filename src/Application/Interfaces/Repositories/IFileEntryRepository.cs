using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IFileEntryRepository
{
    Task<int> AddAsync(FileEntry entry);
    Task<FileEntry?> GetByIdAsync(int id);
    Task<FileEntry?> GetByFileUrl(string url);
    Task<IEnumerable<FileEntry>> GetAllWithPagination(int page, int pageSize);
    Task<IEnumerable<FileEntry>> GetAllWithPagination(string? search, int page, int pageSize);
}