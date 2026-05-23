namespace AEngine.Physics;

public static class Math2D
{
	private const float VerySmallValue = 0.0005f;

	public static float Lenght(Vector2D vector) => MathF.Sqrt(vector.x * vector.x + vector.y * vector.y);
	public static float Clamp(float value, float min, float max)
	{
		if (min == max)
			return min;

		if (min > max)
			throw new ArgumentOutOfRangeException("Min is greater than the max.");

		if (value < min)
			return min;
		else if (value > max)
			return max;

		return value;
	}

	public static int Clamp(int value, int min, int max)
	{
		if (min == max)
			return min;

		if (min > max)
			throw new ArgumentOutOfRangeException("Min is greater than the max.");

		if (value < min)
			return min;
		else if (value > max)
			return max;

		return value;
	}

	public static float Distance(Vector2D a, Vector2D b)
	{
		float deltaX = a.x - b.x;
		float deltaY = a.y - b.y;
		return MathF.Sqrt(deltaX * deltaX + deltaY * deltaY);
	}

	public static Vector2D Normalize(Vector2D vector)
	{
		float lenght = Lenght(vector);

		float x = vector.x / lenght;
		float y = vector.y / lenght;

		return new Vector2D(x, y);
	}

	/// <summary> a * b = ax * bx + ay * by </summary>
	public static float Dot(Vector2D a, Vector2D b) => a.x * b.x + a.y * b.y;
	/// <summary> z = ax * by - ay * bx </summary>
	public static float Cross(Vector2D a, Vector2D b) => a.x * b.y - a.y * b.x;

	public static float LengthSquared(Vector2D v) => v.x * v.x + v.y * v.y;
	public static float DistanceSquared(Vector2D a, Vector2D b)
	{
		float dx = a.x - b.x;
		float dy = a.y - b.y;
		return dx * dx + dy * dy;
	}

	public static bool NearlyEqual(float a, float b) => MathF.Abs(a - b) < VerySmallValue;
	public static bool NearlyEqual(Vector2D a, Vector2D b) => DistanceSquared(a, b) < VerySmallValue * VerySmallValue;
}