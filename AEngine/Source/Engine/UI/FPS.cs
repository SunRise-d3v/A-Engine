namespace AEngine.UI;

internal sealed class FPS : DrawableGameComponent
{
    private static FPS _instance;

    private ushort _fps;
    private ushort _frames;

    private double _seconds;

    private bool _visible;

    private const byte LOWER_THRESHOLD = 30;

    private const byte STABILITY_FPS = 60;
    private const byte WARNING_FPS = 55;
    private const byte ERROR_FPS = 40;

    public FPS(Game game) : base(game) { _instance = this; }

    public override void Update(GameTime gameTime)
    {
        _seconds += gameTime.ElapsedGameTime.TotalSeconds;

        if (_seconds >= 1)
        {
            _fps = _frames;
            _seconds = 0;
            _frames = 0;
#if false
			if (_fps <= LOWER_THRESHOLD)
				throw new InvalidOperationException($"Low FPS: {_fps}." +
					$"Optimization needed!");
#endif
        }

        _frames++;

        base.Update(gameTime);
    }

    private void DrawFPS(Color color) => Main.SpriteBatch.DrawString(Canvas.fpsFont, $"FPS: {_fps}", Vector2.Zero, color);
    public static void Toggle() => _instance._visible = !_instance._visible;
    public static void Draw()
    {
        if (_instance._visible)
        {
            if (_instance._fps <= ERROR_FPS)
                _instance.DrawFPS(Color.Red);
            else if (_instance._fps <= WARNING_FPS)
                _instance.DrawFPS(Color.Yellow);
            else
                _instance.DrawFPS(Color.Green);
        }
    }
}