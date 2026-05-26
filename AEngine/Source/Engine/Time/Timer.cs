namespace AEngine;

public class Timer
{
    private TimeSpan _elapsed;
    private int _duration;

    public bool GoodToGo { get; set; }

    public int Duration
    {
        get => _duration;
        set => _duration = value;
    }

    public int Elapsed => (int)_elapsed.TotalMilliseconds;

    public Timer() => GoodToGo = true;

    public Timer(int duration, bool startLoaded = false)
    {
        _duration = duration;
        GoodToGo = startLoaded;
    }

    public void Update() => _elapsed += Main.GameTime.ElapsedGameTime;

    public void Update(float multiplier) => _elapsed += TimeSpan.FromTicks((long)(Main.GameTime.ElapsedGameTime.Ticks * multiplier));

    public void Add(int milliseconds) => _elapsed += TimeSpan.FromMilliseconds(milliseconds);

    public bool Test() => _elapsed.TotalMilliseconds >= _duration || GoodToGo;

    public void Reset()
    {
        _elapsed -= TimeSpan.FromMilliseconds(_duration);
        if (_elapsed < TimeSpan.Zero)
            _elapsed = TimeSpan.Zero;
        GoodToGo = false;
    }

    public void Reset(int newDuration)
    {
        _elapsed = TimeSpan.Zero;
        _duration = newDuration;
        GoodToGo = false;
    }

    public void ResetToZero()
    {
        _elapsed = TimeSpan.Zero;
        GoodToGo = false;
    }

    public void SetElapsed(TimeSpan elapsed) => _elapsed = elapsed;
    public void SetElapsed(int milliseconds) => _elapsed = TimeSpan.FromMilliseconds(milliseconds);

    public virtual XElement ToXml() =>
        new XElement("Timer",
        new XElement("Duration", _duration),
        new XElement("Elapsed", Elapsed));
}