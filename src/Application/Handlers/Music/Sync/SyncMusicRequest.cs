using Application.Interfaces.Common;

namespace Application.Handlers.Music.Sync;

public record SyncMusicRequest() : IRequest<SyncMusicResponse>;