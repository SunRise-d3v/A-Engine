namespace AEngine;

public static class Time
{
	public static float GetElapsedSecond() => (float)Global.GameTime.ElapsedGameTime.TotalSeconds;
}