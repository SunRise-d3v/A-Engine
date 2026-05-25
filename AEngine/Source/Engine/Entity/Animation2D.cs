namespace AEngine;

public class Animation2D
{
    #region Fields
    private readonly Texture2D texture;
    private readonly Rectangle[] sourceRectangle;
    private readonly Layer layer;
    private readonly int frameWidth;
    private readonly int frameHeight;
    private readonly float animationSpeed;
    private float frameTimeLeft;
    private readonly int frames;
    private int frame;
    private bool active;
    #endregion

    public bool Loop { get; set; }
    public float Rotation { get; set; }
    public Vector2 Scale { get; set; }

    public Animation2D(Texture2D texture, byte frameX, float frameTime, AnimationConfig config)
    {
        sourceRectangle = new Rectangle[frameX];

        layer = new(config.Depth);
        Rotation = config.Rotation;
        Scale = config.Scale;
        active = true;

        this.Loop = config.Loop;
        this.texture = texture;
        this.animationSpeed = frameTime;

        frameTimeLeft = animationSpeed;
        frames = frameX;

        frameWidth = (texture.Width / frameX);
        frameHeight = texture.Height;

        for (byte i = 0; i < frames; i++)
            sourceRectangle[i] = new(i * frameWidth, 0, frameWidth, frameHeight);
    }
    public void Start() => active = true;
    public void Stop() => active = false;
    public void Reset()
    {
        frame = 0;
        frameTimeLeft = animationSpeed;
    }
    public void Update(FixedGameTime deltaTime)
    {
        if (!active)
            return;

        frameTimeLeft -= (float)deltaTime.ElapsedGameTime.TotalSeconds;
        if (frameTimeLeft <= 0)
        {
            frameTimeLeft += animationSpeed;

            if (!Loop && frame == frames - 1)
            {
                Stop();
                return;
            }

            frame = (frame + 1) % frames;
        }
    }
    public void Draw(Vector2? position = null)
    {
        if (position == null)
            return;

        Main.SpriteBatch.Draw(texture, (Vector2)position, sourceRectangle[frame], Color.White, Rotation, Vector2.Zero, Scale, SpriteEffects.None, layer.depth);
    }
    public void Draw(Vector2 position, Color color, float rotation, SpriteEffects effect, float layerDepth)
    {
        Main.SpriteBatch.Draw(texture, position, sourceRectangle[frame],
            color, rotation, Vector2.Zero, Scale, effect, layerDepth);
    }
}

public record AnimationConfig
{
    public byte Depth { get; init; } = 0;
    public float Rotation { get; init; } = 0f;
    public Vector2 Scale { get; init; } = Vector2.One;
    public bool Loop { get; init; } = true;
}