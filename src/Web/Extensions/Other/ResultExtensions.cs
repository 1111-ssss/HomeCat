using Application.Handlers.Download.GetFile;
using Application.Handlers.Download.UploadFile;
using Domain.Model.Result;

public static class ResultExtensions
{
    public static IResult ToApiResult(this IResultBase result)
    {
        if (result.IsSuccess)
            return Results.Ok();

        if (!result.Error.HasValue)
            return Results.BadRequest(new { message = result.Message ?? "Unknown error" });

        int statusCode = HttpStatusCodeAttribute.GetHttpStatusCode(result.Error.Value);
        var body = new
        {
            error = Enum.GetName(typeof(ErrorCode), result.Error.Value) ?? "Unknown",
            message = result.Message,
            details = result.Details
        };

        return statusCode switch
        {
            400 => Results.BadRequest(body),
            401 => Results.Json(body, statusCode: statusCode),
            403 => Results.Json(body, statusCode: statusCode),
            404 => Results.NotFound(body),
            409 => Results.Conflict(body),
            _ => Results.Json(body, statusCode: statusCode),
        };
    }

    public static IResult ToApiFileResult(this Result<GetFileResponse> result) {
        if (result.IsSuccess) {
            var value = result.Value;
            return Results.File(value.FileStream, value.ContentType, value.FileName);
        }
        
        return ((IResultBase)result).ToApiResult();
    }
    public static IResult ToApiResult<T>(this Result<T> result)
    {
        if (result.IsSuccess && result.Value is FileStream fileStream)
        {
            var contentType = "application/octet-stream";
            var fileName = "downloaded.file";
            return Results.File(fileStream, contentType, fileName);
        }

        if (result.IsSuccess)
            return Results.Ok(result.Value);

        return ((IResultBase)result).ToApiResult();
    }
}