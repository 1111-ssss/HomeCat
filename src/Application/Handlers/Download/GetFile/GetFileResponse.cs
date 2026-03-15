namespace Application.Handlers.Download.GetFile;

public record GetFileResponse(
    FileStream FileStream,
    string ContentType,
    string FileName
);