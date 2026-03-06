using PotapkinPrac1.Domain;
using PotapkinPrac1.Errors;

namespace PotapkinPrac1.Services;

public class TrackService
{
    private readonly List<Track> _tracks;
    private int _nextId = 1;
    private readonly object _lock = new();

    public TrackService()
    {
        _tracks = new List<Track>
        {
            new Track { Id = 1, Title = "Bohemian Rhapsody", Artist = "Queen", Album = "A Night at the Opera", Duration = 354 },
            new Track { Id = 2, Title = "Stairway to Heaven", Artist = "Led Zeppelin", Album = "Led Zeppelin IV", Duration = 482 },
            new Track { Id = 3, Title = "Hotel California", Artist = "Eagles", Album = "Hotel California", Duration = 391 }
        };
        _nextId = 4;
    }

    public List<Track> GetAll(string? artistFilter = null)
    {
        lock (_lock)
        {
            if (string.IsNullOrWhiteSpace(artistFilter))
            {
                return _tracks.ToList();
            }

            return _tracks
                .Where(t => t.Artist.Contains(artistFilter, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }

    public Track GetById(int id)
    {
        lock (_lock)
        {
            var track = _tracks.FirstOrDefault(t => t.Id == id);
            if (track == null)
            {
                throw new NotFoundException($"Трек с ID {id} не найден");
            }
            return track;
        }
    }

    public Track Create(Track track)
    {
        ValidateTrack(track);

        lock (_lock)
        {
            track.Id = _nextId++;
            _tracks.Add(track);
            return track;
        }
    }

    private void ValidateTrack(Track track)
    {
        if (string.IsNullOrWhiteSpace(track.Title))
        {
            throw new ValidationException("Название трека не может быть пустым");
        }

        if (track.Duration <= 0)
        {
            throw new ValidationException("Длительность трека должна быть больше 0");
        }

        if (string.IsNullOrWhiteSpace(track.Artist))
        {
            throw new ValidationException("Исполнитель не может быть пустым");
        }
    }
}
