namespace AEngine;

public class Animation2D
{
    private readonly Texture2D _texture;
    private readonly Rectangle[] _frames;
    private readonly Layer _layer;
    private readonly float _animationSpeed;
    private float _frameTimeLeft;
    private int _frame;
    private bool _active;

    public bool Loop { get; set; }
    public event Action OnComplete;

    public Animation2D(Texture2D texture, int frameCount, float frameTime, AnimationConfig config = null)
    {
        config ??= new();
        _texture = texture;
        _layer = new(config.Depth);
        Loop = config.Loop;
        _active = true;
        _animationSpeed = frameTime;
        _frameTimeLeft = frameTime;

        int frameWidth = texture.Width / frameCount;
        _frames = new Rectangle[frameCount];
        for (int i = 0; i < frameCount; i++)
            _frames[i] = new(i * frameWidth, 0, frameWidth, texture.Height);
    }

    public void Play() => _active = true;
    public void Stop() => _active = false;
    public void Reset() { _frame = 0; _frameTimeLeft = _animationSpeed; }

    public void Update(GameTime gameTime)
    {
        if (!_active) return;

        _frameTimeLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_frameTimeLeft <= 0)
        {
            _frameTimeLeft += _animationSpeed;
            if (!Loop && _frame == _frames.Length - 1)
            {
                Stop();
                OnComplete?.Invoke();
                return;
            }
            _frame = (_frame + 1) % _frames.Length;
        }
    }

    public void Draw(Vector2 position, Color color, float rotation, Vector2 scale, SpriteEffects effect, float layerDepth)
        => Main.SpriteBatch.Draw(_texture, position, _frames[_frame],
            color, rotation, Vector2.Zero, scale, effect, layerDepth);
}

public record AnimationConfig
{
    public bool Loop { get; init; } = true;
    public byte Depth { get; init; } = 0;
}