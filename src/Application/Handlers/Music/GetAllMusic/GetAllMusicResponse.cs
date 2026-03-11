using Application.Handlers.Music.Common;

namespace Application.Handlers.Music.GetAllMusic;

public record GetAllMusicResponse(
    int Page,
    int PageSize,
    int TotalPages,
    List<MusicFile> MusicFiles
);