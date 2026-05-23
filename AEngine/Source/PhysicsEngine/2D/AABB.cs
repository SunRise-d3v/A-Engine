namespace AEngine.Physics;

public readonly struct AABB
{
	public readonly Vector2D min, max;

	public AABB(Vector2D min, Vector2D max)
	{
		this.min = min;
		this.max = max;
	}

	public AABB(float minX, float maxX,
		float minY, float maxY)
	{
		this.min = new(minX, minY);
		this.max = new(maxX, maxY);
	}
}