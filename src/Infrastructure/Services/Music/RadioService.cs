using Application.Handlers.Music.Sync;
using Application.Interfaces.Music;

namespace Infrastructure.Services.Music;

public class RadioService : IRadioService
{
    private SyncMusicResponse? _currentlyPlaying;
    private DateTime _startedAt = DateTime.UtcNow;
    
    public RadioService()
    {

    }
    
    public void Play(SyncMusicResponse file)
    {
        _currentlyPlaying = file;
        _startedAt = DateTime.UtcNow;
    }
    
    public SyncMusicResponse? GetCurrentlyPlaying()
    {
        if (_currentlyPlaying == null)
            return null;

        var currentPosition = (DateTime.UtcNow - _startedAt).TotalMilliseconds % _currentlyPlaying.Duration;

        return new SyncMusicResponse(
            Title: _currentlyPlaying.Title,
            Duration: _currentlyPlaying.Duration,
            Position: currentPosition,
            ServerTimestamp: DateTime.UtcNow,
            File: _currentlyPlaying.File
        );
    }
}