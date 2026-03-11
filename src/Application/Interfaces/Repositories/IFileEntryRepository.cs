using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IFileEntryRepository
{
    Task<int> AddAsync(FileEntry entry);
    Task<FileEntry?> GetByIdAsync(int id);
    Task<FileEntry?> GetBy(int id);
    Task<FileEntry?> GetByFileUrl(string url);
}