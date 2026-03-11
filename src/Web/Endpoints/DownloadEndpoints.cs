using Microsoft.AspNetCore.Mvc;
using Domain.Model.Result;
using Application.Handlers.Download.GetFile;
using Application.Handlers.Download.UploadFile;

namespace API.Endpoints;

public static class DownloadEndpoints
{
    public static IEndpointRouteBuilder MapDownloadEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/download")
            .RequireRateLimiting("StrictLimiter")
            .WithTags("Загрузка");

        group.MapGet("/get", GetFileAsync)
            .WithName("Get File")
            .WithSummary("Получение файла")
            .WithDescription("Позволяет пользователю загрузить файл по ссылке")
            .Accepts<GetFileRequest>("application/json")
            .Produces<FileStream>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        group.MapPost("/upload", UploadFileAsync)
            .WithName("Upload File")
            .WithSummary("Загрузка файла")
            .WithDescription("Позволяет пользователю загрузить на сервер")
            .Accepts<UploadFileRequest>("multipart/form-data")
            .Produces<FileResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

        return group;
    }
    private static async Task<IResult> GetFileAsync(
        [FromServices] GetFileHandler handler,
        [FromBody] GetFileRequest request,
        CancellationToken ct
    )
    {
        var result = await handler.Handle(request, ct);

        return result.ToApiResult();
    }

    private static async Task<IResult> UploadFileAsync(
        [FromServices] UploadFileHandler handler,
        [FromBody] UploadFileRequest request,
        CancellationToken ct
    )
    {
        var result = await handler.Handle(request, ct);

        return result.ToApiResult();
    }
}