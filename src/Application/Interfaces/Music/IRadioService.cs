using Application.Handlers.Music.Sync;

namespace Application.Interfaces.Music;

public interface IRadioService
{
    void Play(SyncMusicResponse file);
    SyncMusicResponse? GetCurrentlyPlaying();
}