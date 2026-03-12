using Application.Interfaces.Music;
using Domain.Model.Result;
using TagLib;

namespace Infrastructure.Services.Music;

public class AudioUtilsService : IAudioUtilsService
{
    public AudioUtilsService()
    {
        
    }
    public async Task<Result<TimeSpan>> GetAudioDurationAsync(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return Result.Failed(ErrorCode.WrongFileType, "Путь к файлу не указан");
        }

        if (!Path.Exists(path))
        {
            return Result.Failed(ErrorCode.NotFoundOnServer, "Файл не найден");
        }

        try
        {
            var audioFile = TagLib.File.Create(path);
            return Result<TimeSpan>.Success(audioFile.Properties.Duration);
        }
        catch (UnsupportedFormatException)
        {
            return Result.Failed(ErrorCode.WrongFileType, "Неподдерживаемый формат файла");
        }
        catch (CorruptFileException)
        {
            return Result.Failed(ErrorCode.CorruptedFile, "Файл повреждён");
        }
        catch (Exception)
        {
            return Result.Failed(ErrorCode.InternalServerError, "Не получилось получить длительность аудиофайла");
        }
    }
}