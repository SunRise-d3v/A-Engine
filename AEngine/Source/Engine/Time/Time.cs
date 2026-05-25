namespace AEngine;

public static class Time
{
    private const byte FIXED_TIME = 40;

    public static float GetElapsedSecond() => (float)Main.GameTime.ElapsedGameTime.TotalSeconds;
    public static float GetElapsedMillisecond() => (float)Main.GameTime.ElapsedGameTime.TotalMilliseconds;
    public static float GetElapsedMicrosecond() => (float)Main.GameTime.ElapsedGameTime.TotalMicroseconds;
    public static float GetElapsedMinute() => (float)Main.GameTime.ElapsedGameTime.TotalMinutes;
    public static float GetElapsedNanosecond() => (float)Main.GameTime.ElapsedGameTime.TotalNanoseconds;
    public static float GetElapsedHour() => (float)Main.GameTime.ElapsedGameTime.TotalHours;
    public static float GetElapsedDay() => (float)Main.GameTime.ElapsedGameTime.TotalDays;

    public static float deltaTime { get => (float)Main.GameTime.ElapsedGameTime.TotalMilliseconds / FIXED_TIME; }
}