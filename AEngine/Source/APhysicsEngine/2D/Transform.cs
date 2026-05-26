namespace AEngine.Physics;

public readonly struct Transform
{
	public readonly float x;
	public readonly float y;

	public readonly float sin;
	public readonly float cos;

	public readonly static Transform Zero = new Transform(0, 0, 0);

	public Transform(Vector2D position, float angle)
	{
		this.x = position.x;
		this.y = position.y;

		this.sin = MathF.Sin(angle);
		this.cos = MathF.Cos(angle);
	}

	public Transform(float x, float y, float angle)
	{
		this.x = x;
		this.y = y;

		this.sin = MathF.Sin(angle);
		this.cos = MathF.Cos(angle);
	}
}