using Application.Handlers.Music.Common;
using Application.Handlers.Music.Sync;
using Application.Interfaces.Common;
using Application.Interfaces.Music;
using Application.Interfaces.Repositories;
using Domain.Model.Result;

namespace Application.Handlers.Music.PlayRadio;

public class PlayRadioHandler : IHandler<PlayRadioRequest, PlayRadioResponse>
{
    private readonly IRadioService _radioService;
    private readonly IFileEntryRepository _fileEntryRepository;
    private readonly IAudioUtilsService _audioUtilsService;
    public PlayRadioHandler(
        IRadioService radioService,
        IFileEntryRepository fileEntryRepository,
        IAudioUtilsService audioUtilsService
    )
    {
        _radioService = radioService;
        _fileEntryRepository = fileEntryRepository;
        _audioUtilsService = audioUtilsService;
    }
    public async Task<Result<PlayRadioResponse>> Handle(PlayRadioRequest request, CancellationToken ct)
    {
        var musicResult = await _fileEntryRepository.GetByFileUrl(request.Url);
        if (musicResult == null)
            return Result.Failed(ErrorCode.NotFound, "Музыка не найдена");

        if (musicResult.FileType != "Music")
            return Result.Failed(ErrorCode.WrongFileType, "Музыка не найдена");

        var durationResult = await _audioUtilsService.GetAudioDurationAsync(musicResult.Path);
        if (!durationResult.IsSuccess)
            return (Result)durationResult;

        var musicValue = new SyncMusicResponse(
            Title: musicResult.FileName,
            Duration: durationResult.Value.Milliseconds,
            Position: 0,
            File: new MusicFile(
                DownloadUrl: musicResult.FileUrl.ToString(),
                FileName: musicResult.FileName,
                ContentType: musicResult.ContentType,
                Size: musicResult.Size
            ),
            ServerTimestamp: DateTime.UtcNow
        );
        _radioService.Play(musicValue);
        
        return Result<PlayRadioResponse>.Success(new PlayRadioResponse(
            Value: musicValue
        ));
    }
}