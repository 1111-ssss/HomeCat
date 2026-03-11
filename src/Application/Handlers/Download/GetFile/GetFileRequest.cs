using Application.Interfaces.Common;

namespace Application.Handlers.Download.GetFile;

public record GetFileRequest(
    string Url,
    string ContentType,
    string FileName
) : IRequest<FileStream>;