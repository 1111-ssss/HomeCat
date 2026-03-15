using Application.Interfaces.Common;

namespace Application.Handlers.Download.GetFile;

public record GetFileRequest(
    string Url
) : IRequest<GetFileResponse>;