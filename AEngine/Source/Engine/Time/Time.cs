namespace AEngine;

public static class Time
{
    private const byte FIXED_STEP = 40;

    public static float GetElapsedSecond() => (float)Main.GameTime.ElapsedGameTime.TotalSeconds;
    public static float GetElapsedMillisecond() => (float)Main.GameTime.ElapsedGameTime.TotalMilliseconds;
    public static float GetElapsedMicrosecond() => (float)Main.GameTime.ElapsedGameTime.TotalMicroseconds;
    public static float GetElapsedMinute() => (float)Main.GameTime.ElapsedGameTime.TotalMinutes;
    public static float GetElapsedNanosecond() => (float)Main.GameTime.ElapsedGameTime.TotalNanoseconds;
    public static float GetElapsedHour() => (float)Main.GameTime.ElapsedGameTime.TotalHours;
    public static float GetElapsedDay() => (float)Main.GameTime.ElapsedGameTime.TotalDays;

    /// <summary> Время в секундах с последнего кадра. Как Unity deltaTime </summary>
    public static float deltaTime => (float)Main.GameTime.ElapsedGameTime.TotalSeconds * FIXED_STEP;

    /// <summary> Время в секундах с запуска игры. Как Unity time </summary>
    public static float time => (float)Main.GameTime.TotalGameTime.TotalSeconds;

    /// <summary> Фиксированный шаг физики. Как Unity fixedDeltaTime </summary>
    public static float fixedDeltaTime => (float)Main.FixedGameTime.ElapsedGameTime.TotalSeconds;

    /// <summary> Масштаб времени. Как Unity timeScale </summary>
    public static float timeScale { get; set; } = 1f;

    /// <summary> deltaTime с учётом timeScale </summary>
    public static float scaledDeltaTime => deltaTime * timeScale;
}