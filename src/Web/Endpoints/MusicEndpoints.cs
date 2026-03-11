using Microsoft.AspNetCore.Mvc;
using Domain.Model.Result;
using Application.Handlers.Music.Sync;
using Application.Handlers.Music.GetAllMusic;

namespace API.Endpoints;

public static class MusicEndpoints
{
    public static IEndpointRouteBuilder MapMusicEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/music")
            .RequireRateLimiting("DefaultLimiter")
            .WithTags("Музыка");

        group.MapGet("/sync", SyncAsync)
            .WithName("Sync")
            .WithSummary("Синхронизация музыки")
            .WithDescription("Позволяет пользователю загрузить музыку и синхронизировать ее с другмими устройствами")
            .Produces<SyncMusicResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        group.MapGet("/search", SearchAsync)
            .WithName("Search")
            .WithSummary("Поиск музыки")
            .WithDescription("Позволяет получить список музыки по запросу")
            .Accepts<GetAllMusicRequest>("application/json")
            .Produces<GetAllMusicResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        return group;
    }
    private static async Task<IResult> SyncAsync(
        [FromServices] SyncMusicHandler handler,
        [FromBody] SyncMusicRequest request,
        CancellationToken ct
    )
    {
        var result = await handler.Handle(request, ct);

        return result.ToApiResult();
    }
    private static async Task<IResult> SearchAsync(
        [FromServices] GetAllMusicHandler handler,
        [FromBody] GetAllMusicRequest request,
        CancellationToken ct
    )
    {
        var result = await handler.Handle(request, ct);

        return result.ToApiResult();
    }
}