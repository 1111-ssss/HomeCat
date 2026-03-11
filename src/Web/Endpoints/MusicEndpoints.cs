using Microsoft.AspNetCore.Mvc;
using Domain.Model.Result;
using Application.Handlers.Music.Sync;

namespace API.Endpoints;

public static class MusicEndpoints
{
    public static IEndpointRouteBuilder MapMusicEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/music")
            .RequireRateLimiting("DefaultLimiter")
            .WithTags("Музыка");

        group.MapGet("/sync", LoginAsync)
            .WithName("Sync")
            .WithSummary("Синхронизация музыки")
            .WithDescription("Позволяет пользователю загрузить музыку и синхронизировать ее с другмими устройствами")
            .Produces<SyncMusicResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        return group;
    }
    private static async Task<IResult> LoginAsync(
        [FromServices] SyncMusicHandler handler,
        [FromBody] SyncMusicRequest request,
        CancellationToken ct
    )
    {
        var result = await handler.Handle(request, ct);

        return result.ToApiResult();
    }
}