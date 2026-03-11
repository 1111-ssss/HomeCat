using Application.Interfaces.Common;
using Application.Interfaces.Music;
using Domain.Model.Result;

namespace Application.Handlers.Music.Sync;

public class SyncMusicHandler : IHandler<SyncMusicRequest, SyncMusicResponse>
{
    private readonly IRadioService _radioService;
    public SyncMusicHandler(
        IRadioService radioService
    )
    {
        _radioService = radioService;
    }
    public async Task<Result<SyncMusicResponse>> Handle(SyncMusicRequest request, CancellationToken ct)
    {
        var music = _radioService.GetCurrentlyPlaying();
        if (music == null)
            return Result.Failed(ErrorCode.NotFound, "Нет проигрываемой музыки");
        
        return Result<SyncMusicResponse>.Success(music);
    }
}