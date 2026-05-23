namespace AEngine.Physics;

public readonly struct Vector2D
{
	public readonly float x;
	public readonly float y;

	public Vector2D()
	{
		x = 0f;
		y = 0f;
	}

	public Vector2D(float x, float y)
	{
		this.x = x;
		this.y = y;
	}

	public static Vector2D Zero => new Vector2D(0f, 0f);
	//public static Vector2D One => new Vector2D(1, 1);

	public static Vector2D operator +(Vector2D a, Vector2D b) => new Vector2D(a.x + b.x, a.y + b.y);
	public static Vector2D operator -(Vector2D a, Vector2D b) => new Vector2D(a.x - b.x, a.y - b.y);
	/// <summary> Inversion vectors </summary>
	public static Vector2D operator -(Vector2D v) => new Vector2D(-v.x, -v.y);

	public static Vector2D operator /(Vector2D a, float scalar) => new Vector2D(a.x / scalar, a.y / scalar);
	public static Vector2D operator *(Vector2D a, float scalar) => new Vector2D(a.x * scalar, a.y * scalar);
	public static Vector2D operator *(float scalar, Vector2D a) => new Vector2D(a.x * scalar, a.y * scalar);

	private readonly bool Equal(Vector2D other) => this.x == other.x && this.y == other.y;
	public override bool Equals([NotNullWhen(true)] object obj)
	{
		if (obj is Vector2D other)
			return Equal(other);

		return false;
	}

	public override string ToString() => $"X: {this.x}, Y: {this.y}";
	public override int GetHashCode() => new { this.x, this.y }.GetHashCode();

	internal static Vector2D Transform(Vector2D vector, Transform transform)
	{
		float rotateX = transform.cos * vector.x - transform.sin * vector.y;
		float rotateY = transform.sin * vector.x + transform.cos * vector.y;

		float transformX = rotateX + transform.x;
		float transformY = rotateY + transform.y;

		return new Vector2D(transformX, transformY);
	}
}