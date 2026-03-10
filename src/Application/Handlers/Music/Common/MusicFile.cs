namespace Application.Handlers.Music.Common;

public record MusicFile(
    string DownloadUrl,
    string FileName,
    string ContentType,
    int Size
);