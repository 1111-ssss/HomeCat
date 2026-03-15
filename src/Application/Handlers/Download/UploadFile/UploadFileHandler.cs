using Application.Interfaces.Auth;
using Application.Interfaces.Common;
using Application.Interfaces.Download;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Model.Result;

namespace Application.Handlers.Download.UploadFile;

public class UploadFileHandler : IHandler<UploadFileRequest, FileResponse>
{
    private readonly IStorageService _storageService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IFileEntryRepository _fileEntryRepository;

    public UploadFileHandler(
        IStorageService storageService,
        ICurrentUserService currentUserService,
        IFileEntryRepository fileEntryRepository
    )
    {
        _storageService = storageService;
        _currentUserService = currentUserService;
        _fileEntryRepository = fileEntryRepository;
    }

    public async Task<Result<FileResponse>> Handle(UploadFileRequest request, CancellationToken ct)
    {
        var saveResult = await _storageService.SaveFile(SaveFileTypeEnum.MusicFile, request.File);
        if (!saveResult.IsSuccess)
            return (Result)saveResult;

        var userId = _currentUserService.GetUserId();
        if (userId == null)
            return Result.Failed(ErrorCode.Unauthorized, "Не авторизован");

        var fileEntry = new FileEntry
        {
            FileName = request.File.FileName,
            ContentType = request.File.ContentType,
            Path = saveResult.Value,
            FileUrl = saveResult.Value,
            FileType = "Music",
            Size = (int)(request.File.Length / 1024 / 1024),
            UploadedAt = DateTime.Now,
            UploadedById = userId.Value
        };

        try
        {
            await _fileEntryRepository.AddAsync(fileEntry);
        }
        catch (Exception e)
        {
            Console.WriteLine($"DB error: {e.Message}\n{e.StackTrace}");
            return Result.Failed(ErrorCode.DatabaseError, "Ошибка сохранения в базу данных");
        }

        return Result<FileResponse>.Success(new FileResponse(
            saveResult.Value,
            request.File.ContentType,
            request.File.FileName
        ));
    }
}