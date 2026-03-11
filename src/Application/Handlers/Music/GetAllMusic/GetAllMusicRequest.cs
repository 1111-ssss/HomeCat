using Application.Interfaces.Common;

namespace Application.Handlers.Music.GetAllMusic;

public record GetAllMusicRequest(
    int Page,
    int PageSize,
    string? Search
) : IRequest<GetAllMusicResponse>;