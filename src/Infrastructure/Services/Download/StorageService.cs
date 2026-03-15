using Application.Interfaces.Download;
using Domain.Model.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.Download;

public class StorageService : IStorageService
{
    private readonly IConfigurationSection _fileStorageSection;
    private readonly ILogger<StorageService> _logger;
    public StorageService(
        IConfiguration configuration,
        ILogger<StorageService> logger
    )
    {
        _fileStorageSection = configuration.GetSection("FileStorage");
        if (_fileStorageSection == null)
            throw new ArgumentNullException("Не найден раздел FileStorage в файле конфигурации");

        _logger = logger;
    }
    public Result<FileStream> GetFile(string path)
    {
        if (File.Exists(path))
            return Result<FileStream>.Success(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read));

        return Result.Failed(ErrorCode.NotFoundOnServer, "Файл не найден по серверной ошибке");
    }

    public async Task<Result<string>> SaveFile(SaveFileTypeEnum type, IFormFile file)
    {
        string subPath = type switch
        {
            SaveFileTypeEnum.MusicFile => "music",
            SaveFileTypeEnum.File => "files",
            _ => "unknown"
        };
        string path = Path.Combine(_fileStorageSection.Path, subPath);
        Directory.CreateDirectory(path);

        string fileName = $"{DateTime.Now.ToShortDateString()}_{Guid.NewGuid()}";
        string fullPath = Path.Combine(path, fileName);
        
        try
        {
            await using var destination = new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
            await file.CopyToAsync(destination);

            return Result<string>.Success(fullPath);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Не удалось загрузить файл на сервер");
            return Result.Failed(ErrorCode.UploadingFileError, "Не получилось загрузить файл на сервер");
        }
    }
}