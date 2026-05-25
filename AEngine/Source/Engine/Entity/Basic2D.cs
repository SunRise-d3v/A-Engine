namespace AEngine;

public class Basic2D : Component
{
    //
    private readonly Texture2D _texture;
    private readonly Layer _layer;
    //
    private Transform _transform;
    private Vector2 _dimension;
    //
    private SpriteEffects _effect;
    private Color _color;

    #region Properties
    public Texture2D texture { get => _texture; }
    public Transform transform
    {
        get => _transform;
        set => _transform = value;
    }

    public SpriteEffects effect
    {
        get => _effect;
        set => _effect = value;
    }
    public Color color
    {
        get => _color;
        set => _color = value;
    }
    #endregion

    public Basic2D(string pathTexture, Vector2 position, Vector2? dimension = null, byte depth = 0)
    {
        _transform = new();
        try
        {
            _texture = Main.Content.Load<Texture2D>(pathTexture ?? "Default/NullTexture");
        }
        catch (ContentLoadException)
        {
            _texture = Main.Content.Load<Texture2D>("Default/NullTexture");
            //Debug.Warning($"Texture not found: '{pathTexture}', using default.");
        }

        _transform.position = position;
        _dimension = dimension ?? new(_texture.Width, _texture.Height);

        _effect = SpriteEffects.None;
        _color = Color.White;

        _layer = new(depth);
    }

    public override void Update() { }

    public override void Draw(Vector2? offset = null, Vector2? origin = null)
    {
        if (_texture == null)
            return;

        Vector2 off = offset ?? Vector2.Zero;
        Vector2 org = origin ?? new Vector2(_texture.Bounds.Width / 2f, _texture.Bounds.Height / 2f);

        Main.SpriteBatch.Draw(_texture,
            new Rectangle((int)(_transform.position.X + off.X), (int)(_transform.position.Y + off.Y), (int)(_dimension.X * _transform.scale.x), (int)(_dimension.Y * _transform.scale.y)),
            null, _color, _transform.rotation, org, _effect, _layer.depth);
    }

    public virtual float GetDistance(Vector2 a, Vector2 b) => Vector2.Distance(a, b);

    public virtual float RotateTowards(Vector2 position, Vector2 focus)
    {
        Vector2 direction = focus - position;
        return (float)Math.Atan2(direction.Y, direction.X);
    }
}