namespace AEngine;

public class Projectile2D : Basic2D
{
	private readonly Timer _timer;
	protected float Tick { get; private set; }
	protected Vector2 velocity;

	public Basic2D owner { get; }
	public Vector2 direction { get; private set; }
	public float speed { get; set; }
	public bool IsFriendly { get; }
	public bool dead { get; private set; }

	public Projectile2D(string pathTexture, Vector2 position, Vector2 dims,
		Basic2D owner, bool friendly, Vector2 target, float speed, int lifetime = 1000)
		: base(pathTexture, position, dims)
	{
		this.owner = owner;
		IsFriendly = friendly;
		this.speed = speed;
		dead = false;

		direction = Vector2.Normalize(target - position);
		transform.rotation = MathF.Atan2(direction.Y, direction.X);

		_timer = new(lifetime);
	}

	public virtual void Update(List<IHittable> targets)
	{
		Tick = (float)Main.GameTime.ElapsedGameTime.TotalSeconds;
		velocity = direction * Tick;
		transform.position += velocity * speed;

		_timer.Update();
		if (_timer.Test())
			dead = true;
	}

	public virtual bool Hit(List<IHittable> targets)
	{
		foreach (var target in targets)
		{
			if (Vector2.Distance(transform.position, target.position) < target.hitDistance)
			{
				target.Hit();
				dead = true;
				return true;
			}
		}

		return false;
	}
}