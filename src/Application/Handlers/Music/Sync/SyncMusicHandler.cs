using Application.Interfaces.Common;
using Domain.Model.Result;

namespace Application.Handlers.Music.Sync;

public class SyncMusicHandler : IHandler<SyncMusicRequest, SyncMusicResponse>
{
    public SyncMusicHandler(
        
    )
    {
        
    }
    public Task<Result<SyncMusicResponse>> Handle(SyncMusicRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}