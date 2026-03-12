using Application.Handlers.Music.Common;
using Application.Handlers.Music.Sync;

namespace Application.Handlers.Music.PlayRadio;

public record PlayRadioResponse(
    SyncMusicResponse Value
);