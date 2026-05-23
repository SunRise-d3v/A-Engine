namespace AEngine;

public static class RandomHelper
{
	private static Random _rand = new();

	#region Seed
	private static long _seed;
	public static long Seed
	{
		get => _seed;
		private set => _seed = value;
	}
	#endregion

	#region Seed
	public static void GenerateSeed()
	{
		long guid = BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0);
		_seed = Math.Abs(guid) % 90_000_000_000L + 10_000_000_000L;
		_rand = new Random((int)_seed);
	}
	public static void SetSeed(long seed)
	{
		_seed = seed;
		_rand = new Random((int)seed);
	}
	#endregion

	#region Bool
	public static bool RandomBool() => _rand.Next(0, 2) == 1;
	public static bool FiftyFifty() => RandomBool();
	public static bool Chance(int percent) => _rand.Next(0, 100) < Math.Clamp(percent, 0, 100);
	public static bool Chance(float percent) => (float)_rand.NextDouble() * 100f < Math.Clamp(percent, 0f, 100f);
	#endregion

	#region Byte
	public static byte RandomByte() => (byte)_rand.Next(byte.MinValue, byte.MaxValue + 1);
	public static byte RandomByte(byte min, byte max)
	{
		if (min == max)
			return min;
		if (min > max)
			Util.Swap(ref min, ref max);
		return (byte)_rand.Next(min, max + 1);
	}
	public static byte RandomMinByte() => (byte)_rand.Next(byte.MinValue, byte.MaxValue / 2);
	public static byte RandomMaxByte() => (byte)_rand.Next(byte.MaxValue / 2, byte.MaxValue + 1);
	public static byte RandomByteFromZero() => (byte)_rand.Next(0, byte.MaxValue + 1);
	#endregion

	#region Int
	public static int RandomInt() => _rand.Next();
	public static int RandomInt(int min, int max)
	{
		if (min == max)
			return min;
		if (min > max)
			Util.Swap(ref min, ref max);
		return _rand.Next(min, max);
	}
	public static int RandomMinInt() => _rand.Next(int.MinValue, 0);
	public static int RandomMaxInt() => _rand.Next(0, int.MaxValue);
	public static int RandomAbsInt() => _rand.Next(int.MinValue, int.MaxValue);
	public static int RandomIntFromZero() => _rand.Next(0, int.MaxValue);
	public static int RandomNegativeInt() => _rand.Next(int.MinValue, 0);
	#endregion

	#region Float
	public static float RandomFloat() => (float)_rand.NextDouble() * (float.MaxValue / 1e30f);
	public static float RandomFloat(float min, float max)
	{
		if (min == max)
			return min;
		if (min > max)
			Util.Swap(ref min, ref max);
		return min + (float)_rand.NextDouble() * (max - min);
	}
	public static float RandomFloat01() => (float)_rand.NextDouble();
	public static float RandomMinFloat() => RandomFloat(float.MinValue / 1e30f, 0f);
	public static float RandomMaxFloat() => RandomFloat(0f, float.MaxValue / 1e30f);
	public static float RandomNegativeFloat() => RandomFloat(-1_000_000f, 0f);
	public static float RandomPositiveFloat() => RandomFloat(0f, 1_000_000f);
	public static float RandomFloatFromZero() => RandomFloat(0f, 1_000_000f);
	#endregion

	#region Special
	/// <summary> Случайный элемент из массива </summary>
	public static T RandomFrom<T>(params T[] items)
	{
		if (items == null || items.Length == 0)
			throw new ArgumentException("Items cannot be empty.");
		return items[_rand.Next(0, items.Length)];
	}

	/// <summary> Случайный элемент из списка </summary>
	public static T RandomFrom<T>(List<T> items)
	{
		if (items == null || items.Count == 0)
			throw new ArgumentException("List cannot be empty.");
		return items[_rand.Next(0, items.Count)];
	}

	/// <summary> Перемешать список </summary>
	public static void Shuffle<T>(List<T> list)
	{
		for (int i = list.Count - 1; i > 0; i--)
		{
			int j = _rand.Next(0, i + 1);
			(list[i], list[j]) = (list[j], list[i]);
		}
	}

	/// <summary> Случайный знак: +1 или -1 </summary>
	public static int RandomSign() => RandomBool() ? 1 : -1;

	/// <summary> Случайный угол в радианах [0, 2π] </summary>
	public static float RandomAngle() => RandomFloat(0f, MathF.PI * 2f);

	/// <summary> Случайная точка в круге радиуса r </summary>
	public static Vector2 RandomPointInCircle(float radius)
	{
		float angle = RandomAngle();
		float r = radius * MathF.Sqrt(RandomFloat01());
		return new Vector2(r * MathF.Cos(angle), r * MathF.Sin(angle));
	}

	/// <summary> Случайная точка в прямоугольнике </summary>
	public static Vector2 RandomPointInRect(float x, float y, float width, float height)
		=> new Vector2(RandomFloat(x, x + width), RandomFloat(y, y + height));

	/// <summary> Случайный цвет </summary>
	public static Color RandomColor(Random rand)
	{
		Color result = new Color((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble());
		return result;
	}

	/// <summary> Случайный цвет с заданной яркостью [0-1] </summary>
	public static Color RandomColor(float brightness)
	{
		// https://www.nbdtech.com/Blog/archive/2008/04/27/Calculating-the-Perceived-Brightness-of-a-Color.aspx
		// brightness  =  sqrt( .241 R^2 + .691 G^2 + .068 B^2 )

		brightness = Math2D.Clamp(brightness, 0f, 1f);

		float r = RandomFloat(0f, 1f);
		float g = RandomFloat(0f, 1f);
		float b = RandomFloat(0f, 1f);

		float dec = 0.98f;
		float inc = 1f / dec;

		for (int i = 0; i < 64; i++)
		{
			float perceivedBrightness = Util.PercievedBrightness(r, g, b);

			if (perceivedBrightness < brightness)
			{
				r *= inc;
				g *= inc;
				b *= inc;
			}
			else if (perceivedBrightness > brightness)
			{
				r *= dec;
				g *= dec;
				b *= dec;
			}

			dec += 0.0001f;
			inc -= 0.0001f;

			if (dec > 1f)
			{ dec = 1f; }
			if (inc < 1f)
			{ inc = 1f; }
		}

		return new Color(r, g, b);
	}
	/// <summary> Взвешенный случайный выбор </summary>
	public static T RandomWeighted<T>(T[] items, float[] weights)
	{
		if (items.Length != weights.Length)
			throw new ArgumentException("Items and weights must have the same length.");

		float total = 0f;
		foreach (var w in weights)
			total += w;

		float roll = RandomFloat(0f, total);
		float cumulative = 0f;

		for (int i = 0; i < items.Length; i++)
		{
			cumulative += weights[i];
			if (roll <= cumulative)
				return items[i];
		}

		return items[^1];
	}
	#endregion
}