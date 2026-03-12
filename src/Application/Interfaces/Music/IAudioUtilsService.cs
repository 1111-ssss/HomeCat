using Domain.Model.Result;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Music;

public interface IAudioUtilsService
{
    Task<Result<TimeSpan>> GetAudioDurationAsync(string path);
}