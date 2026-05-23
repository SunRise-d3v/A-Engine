namespace AEngine;

internal static class Util
{
	public static void Swap<T>(ref T a, ref T b) where T : struct
	{
		T t = a;
		a = b;
		b = t;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float PercievedBrightness(float r, float g, float b) => MathF.Sqrt((r * r * 0.241f) + (g * g * 0.691f) + (b * b * 0.068f));

	public static float PercievedBrightness(Color color)
	{
		int r = color.R;
		int g = color.G;
		int b = color.B;

		float value = MathF.Sqrt((r * r * 0.241f) + (g * g * 0.691f) + (b * b * 0.068f));
		return value / 255f;
	}
}