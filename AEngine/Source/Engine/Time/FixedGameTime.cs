namespace AEngine;

public sealed class FixedGameTime
{
	private readonly float _fixedStep;

	public TimeSpan ElapsedGameTime { get; }
	public TimeSpan TotalGameTime { get; private set; }
	public bool IsRunningSlowly { get; private set; }

	public float DeltaTime => _fixedStep;

	public FixedGameTime(float fixedStep)
	{
		_fixedStep = fixedStep;
		ElapsedGameTime = TimeSpan.FromSeconds(fixedStep);
	}

	internal void Update(GameTime gameTime)
	{
		TotalGameTime = gameTime.TotalGameTime;
		IsRunningSlowly = gameTime.IsRunningSlowly;
	}

	internal float GetElapsedSecond() => (float)Global.GameTime.ElapsedGameTime.TotalSeconds;
}