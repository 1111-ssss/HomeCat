using Application.Interfaces.Common;
using Application.Interfaces.Download;
using Application.Interfaces.Repositories;
using Domain.Model.Result;

namespace Application.Handlers.Download.GetFile;

public class GetFileHandler : IHandler<GetFileRequest, GetFileResponse>
{
    private readonly IStorageService _storageService;
    private readonly IFileEntryRepository _fileEntryRepository;

    public GetFileHandler(
        IStorageService storageService,
        IFileEntryRepository fileEntryRepository
    )
    {
        _storageService = storageService;
        _fileEntryRepository = fileEntryRepository;
    }

    public async Task<Result<GetFileResponse>> Handle(GetFileRequest request, CancellationToken ct)
    {
        var fileResult = await _fileEntryRepository.GetByFileUrl(request.Url);
        if (fileResult == null)
            return Result.Failed(ErrorCode.NotFound, "Файл не найден");

        var result = _storageService.GetFile(fileResult.Path);
        if (!result.IsSuccess)
            return (Result)result;

        return Result<GetFileResponse>.Success(new GetFileResponse(
            result.Value,
            fileResult.ContentType,
            fileResult.FileName
        ));
    }
}