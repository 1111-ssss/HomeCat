using Application.Handlers.Music.Common;

namespace Application.Handlers.Music.Sync;

public record SyncMusicResponse(
    string Title,
    double Duration,
    double Position,
    MusicFile File,
    DateTime ServerTimestamp
);