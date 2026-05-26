using AEngine.Source.Engine.Entity.GameObject;

namespace AEngine;

public class Character2D : Basic2D
{
	public float speed { get; set; }

	public Character2D(string pathTexture, Vector2 position, Vector2 dims)
		: base(pathTexture, position, dims)
	{
		
	}

	public virtual void AI() { }
	public virtual void AI(Character2D target) { }

	public virtual Vector2 RadialMovement(Vector2 focus)
	{
		float distance = Vector2.Distance(transform.position, focus);
		if (distance <= speed)
			return focus - transform.position;

		return (focus - transform.position) * speed / distance;
	}
}