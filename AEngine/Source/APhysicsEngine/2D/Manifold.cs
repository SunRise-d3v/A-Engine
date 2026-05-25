namespace AEngine.Physics;

public readonly struct Manifold
{
	public readonly Body2D bodyA;
	public readonly Body2D bodyB;
	public readonly Vector2D normal;
	public readonly float depth;

	public readonly Vector2D contactA;
	public readonly Vector2D contactB;
	public readonly int contactCount;

	public Manifold(Body2D bodyA, Body2D bodyB, Vector2D normal, float depth,
		Vector2D contactA, Vector2D contactB, int count)
	{
		this.bodyA = bodyA;
		this.bodyB = bodyB;
		this.normal = normal;
		this.depth = depth;

		this.contactA = contactA;
		this.contactB = contactB;
		this.contactCount = count;
	}
}