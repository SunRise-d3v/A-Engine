namespace AEngine;

public interface IHealth
{
	public float health { get; }
	public float damage { get; }

	public bool IsDead { get; }

	public void TakeDamage(float damage);
	public void Regeneration(float heal);
	public bool Die();
}