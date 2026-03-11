using Application.Handlers.Music.Sync;
using Application.Interfaces.Music;

namespace Infrastructure.Services.Music;

public class RadioService : IRadioService
{
    private SyncMusicResponse? _currentlyPlaying;
    private DateTime _lastResponsed = DateTime.UtcNow;
    public RadioService()
    {
        
    }
    public void Play(SyncMusicResponse file)
    {
        _currentlyPlaying = file;
    }
    public SyncMusicResponse? GetCurrentlyPlaying()
    {
        if (_currentlyPlaying == null)
            return null;

        var currentPosition = (DateTime.UtcNow - _lastResponsed).TotalSeconds % _currentlyPlaying.Duration;

        var updatedMusic = new SyncMusicResponse(
            Title: _currentlyPlaying.Title,
            Duration: _currentlyPlaying.Duration,
            Position: currentPosition,
            ServerTimestamp: DateTime.UtcNow,
            File: _currentlyPlaying.File
        );
        _currentlyPlaying = updatedMusic;

        return updatedMusic;
    }
}