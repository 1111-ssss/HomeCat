namespace Application.Handlers.Download.UploadFile;

public record FileResponse(
    string Url,
    string ContentType,
    string FileName
);