namespace AEngine;

public interface IHittable
{
	public Vector2 position { get; }
	public float hitDistance { get; set; }
	public void Hit();
}