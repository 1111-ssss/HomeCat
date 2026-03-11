using Application.Interfaces.Common;
using Domain.Model.Result;
using Microsoft.AspNetCore.Http;

namespace Application.Handlers.Download.UploadFile;

public record UploadFileRequest(
    IFormFile File
) : IRequest<FileResponse>;