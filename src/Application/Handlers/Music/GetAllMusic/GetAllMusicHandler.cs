using Application.Handlers.Music.Common;
using Application.Interfaces.Common;
using Application.Interfaces.Music;
using Application.Interfaces.Repositories;
using Domain.Model.Result;

namespace Application.Handlers.Music.GetAllMusic;

public class GetAllMusicHandler : IHandler<GetAllMusicRequest, GetAllMusicResponse>
{
    private readonly IRadioService _radioService;
    private readonly IFileEntryRepository _fileEntryRepository;
    public GetAllMusicHandler(
        IRadioService radioService,
        IFileEntryRepository fileEntryRepository)
    {
        _radioService = radioService;
        _fileEntryRepository = fileEntryRepository;
    }
    public async Task<Result<GetAllMusicResponse>> Handle(GetAllMusicRequest request, CancellationToken ct)
    {
        var musics = await _fileEntryRepository.GetAllWithPagination(request.Search, request.Page, request.PageSize);
        if (musics == null)
            return Result.Failed(ErrorCode.NotFound, "Нет музыки");
        
        var musicFiles = musics.Select(x => new MusicFile(
            x.FileUrl.ToString(),
            x.FileName,
            x.ContentType,
            10
        ));

        return Result<GetAllMusicResponse>.Success(new GetAllMusicResponse(
            request.Page,
            request.PageSize,
            (int)Math.Ceiling(musics.Count() / (double)request.PageSize),
            musicFiles.ToList()
        ));
    }
}