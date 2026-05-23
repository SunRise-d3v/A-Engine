namespace AEngine;

public static class Convert
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 ToVector2(Vector2D vector) => new Vector2(vector.x, vector.y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2D ToVector2D(Vector2 vector) => new Vector2D(vector.X, vector.Y);

	public static void ToVector2Array(Vector2D[] src, ref Vector2[] dst)
	{
		if (src is null || src.Length == 0)
			return;

		if (dst is null || src.Length != dst.Length)
			dst = new Vector2[src.Length];

		for (int i = 0; i < src.Length; i++)
			dst[i] = new(src[i].x, src[i].y);
	}
	public static void ToVector2DArray(Vector2[] src, ref Vector2D[] dst)
	{
		if (dst is null || src.Length != dst.Length)
			dst = new Vector2D[src.Length];

		for (int i = 0; i < src.Length; i++)
		{
			Vector2 vector = src[i];
			dst[i] = new(vector.X, vector.Y);
		}
	}
}