using Application.Handlers.Music.Common;

namespace Application.Handlers.Music.Sync;

public record SyncMusicResponse(
    string Title,
    int Duration,
    int Position,
    MusicFile File
);