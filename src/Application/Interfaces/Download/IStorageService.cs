using Domain.Model.Result;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Download;

public interface IStorageService
{
    Result<FileStream> GetFile(string path);
    Task<Result<string>> SaveFile(SaveFileTypeEnum type, IFormFile file);
}