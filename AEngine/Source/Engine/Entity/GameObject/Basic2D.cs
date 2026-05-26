namespace AEngine;

public class Basic2D : Component
{
    private Transform _transform;
    private Velocity _velocity;
    private Vector2 _dimension;

    private Sprite _sprite;

    #region Properties
    public Transform transform { get => _transform; }
    public Sprite sprite
    {
        get => _sprite;
        set => _sprite = value;
    }
    public Velocity velocity
    {
        get => _velocity;
        set => _velocity = value;
    }
    #endregion

    public Basic2D(string pathTexture, Vector2 position, Vector2? dimension = null, byte depth = 0)
    {
        _transform = new();
        _velocity = new();
        _sprite = new(pathTexture, depth);

        _transform.position = position;
        _dimension = dimension ?? new(_sprite.texture.Width, _sprite.texture.Height);
    }

    public override void Update()
    {
        _transform.position += _velocity.force * Time.deltaTime;
        base.Update();
    }

    public override void Draw(Vector2? offset = null, Vector2? origin = null)
    {
        if (_sprite.texture == null)
            return;

        Vector2 off = offset ?? Vector2.Zero;
        Vector2 org = origin ?? new Vector2(_sprite.texture.Bounds.Width / 2f, _sprite.texture.Bounds.Height / 2f);

        Main.SpriteBatch.Draw(_sprite.texture,
            new Rectangle((int)(_transform.position.X + off.X), (int)(_transform.position.Y + off.Y), (int)(_dimension.X * _transform.scale.x), (int)(_dimension.Y * _transform.scale.y)),
            null, _sprite.color, _transform.rotation, org, _sprite.flip, _sprite.layer.depth);
    }

    public virtual float GetDistance(Vector2 a, Vector2 b) => Vector2.Distance(a, b);

    public virtual float RotateTowards(Vector2 position, Vector2 focus)
    {
        Vector2 direction = focus - position;
        return (float)Math.Atan2(direction.Y, direction.X);
    }
}