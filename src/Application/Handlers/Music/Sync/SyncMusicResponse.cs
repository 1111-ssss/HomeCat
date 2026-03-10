using Application.Handlers.Music.Common;

namespace Application.Handlers.Music.Sync;

public record SyncMusicResponse(
    string TrackId,
    string Artist,
    string Album,
    string Title,
    int Duration,
    int Position,
    MusicFile File
);