namespace AEngine;

public class Basic2D
{
	private readonly Texture2D _texture;
	private Vector2 _position, _dimension;
	private float _rotation;

	public Texture2D texture { get => _texture; }
	public Vector2 position
	{
		get => _position;
		set => _position = value;
	}
	public float rotation
	{
		get => _rotation;
		set => _rotation = value % (MathF.PI * 2);
	}

	public Basic2D(string pathTexture, Vector2 position, Vector2 dimension)
	{
		this._position = position;
		this._dimension = dimension;

		_texture = Global.Content.Load<Texture2D>(pathTexture);
	}

	public virtual void Update(Vector2? offset = null)
	{
	}

	public virtual void Draw(Vector2? offset = null, Vector2? origin = null)
	{
		if (_texture == null)
			return;

		Vector2 off = offset ?? Vector2.Zero;
		Vector2 org = origin ?? new Vector2(_texture.Bounds.Width / 2f, _texture.Bounds.Height / 2f);
		Global.SpriteBatch.Draw(_texture,
			new Rectangle((int)(_position.X + off.X), (int)(_position.Y + off.Y), (int)_dimension.X, (int)_dimension.Y),
			null, Color.White, _rotation, org, SpriteEffects.None, 0f);
	}

	public virtual float GetDistance(Vector2 a, Vector2 b) => Vector2.Distance(a, b);

	public virtual float RotateTowards(Vector2 position, Vector2 focus)
	{
		Vector2 direction = focus - position;
		return (float)Math.Atan2(direction.Y, direction.X);
	}
}