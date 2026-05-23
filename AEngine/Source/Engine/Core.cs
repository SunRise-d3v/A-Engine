namespace AEngine;

public class Core : Game
{
	#region Xna
	internal static Core s_instance;
	public static Core Instance => s_instance;

	public static GraphicsDeviceManager Graphics { get; private set; }
	public static new GraphicsDevice GraphicsDevice { get; private set; }
	#endregion

	#region Engine
	private float _accumulator;
	private const float FIXED_STEP = 1f / 60f; // 60 updates/sec
	private const byte COLLISION_ITERATION = 16;
	private PhysicsWorld _world;

	private readonly FixedGameTime _fixedGameTime;
	private bool _vSync;

	private Basic2D _cursor;
	private bool _customMouseVisible;
	private string _cursorPath;
	protected string CursorPath { set => _cursorPath = string.IsNullOrEmpty(value) ? _cursorPath : value; }

	private Color[] _screenshotBuffer;
	#endregion

	public Core(string title, int width, int height,
		bool fullScreen, bool vSync = true, bool mouseVisible = true)
	{
		if (s_instance != null)
			throw new InvalidOperationException($"Only a single Core instance can be created");

		s_instance = this;

		IsFixedTimeStep = false;
		_vSync = vSync;

		Graphics = new GraphicsDeviceManager(this);
		_fixedGameTime = new(FIXED_STEP);
		Global.IsMouseVisible = mouseVisible;

		Global.WindowWidth = width;
		Global.WindowHeight = height;
		Global.FullScreen = fullScreen;

		Graphics.PreferredBackBufferWidth = Global.WindowWidth;
		Graphics.PreferredBackBufferHeight = Global.WindowHeight;
		Graphics.IsFullScreen = Global.FullScreen;
		VSync();

		Window.Title = title;

		Global.Content = base.Content;
		Global.Content.RootDirectory = "Content";

		_cursorPath = "Sprites/DefaultCursor";
		IsMouseVisible = Global.IsMouseVisible;
		_customMouseVisible = !IsMouseVisible;

		Components.Add(new FPS(this));
	}

	protected override void Initialize()
	{
		base.Initialize();
		Global.Init();

		_world = new();
		GraphicsDevice = base.GraphicsDevice;

		Global.SpriteBatch = new SpriteBatch(GraphicsDevice);
		Global.Keyboard = new();
		Global.Mouse = new();

		if (_customMouseVisible)
			_cursor = new(_cursorPath, Vector2.Zero, new Vector2(12, 19));

		//_allRoom.Add()
	}

	protected override void UnloadContent()
	{
		RoomManager.UnLoadContent();
		base.UnloadContent();
	}

	protected override void Update(GameTime time)
	{
		Global.GameTime = time;

		#region Call Fixed Update
		float deltaTime = Time.GetElapsedSecond();
		_accumulator += deltaTime;

		if (_accumulator > FIXED_STEP * 5)
			_accumulator = FIXED_STEP * 5;

		while (_accumulator >= FIXED_STEP)
		{
			_fixedGameTime.Update(time);
			FixedUpdate(_fixedGameTime);
			_accumulator -= FIXED_STEP;
		}
		#endregion

		Global.Keyboard.Update();
		Global.Mouse.Update();

		#region Default Keys
		if (Global.Keyboard.JustReleased(Keys.Escape) ||
			(Global.Keyboard.IsPressed(Keys.LeftAlt) && Global.Keyboard.JustReleased(Keys.F4)) ||
			(Global.Keyboard.IsPressed(Keys.RightAlt) && Global.Keyboard.JustReleased(Keys.F4)))
			Exit();

		if (Global.Keyboard.JustPressed(Keys.F1))
			VSync();

		if (Global.Keyboard.JustPressed(Keys.F3))
			FPS.Toggle();

		if (Global.Keyboard.JustPressed(Keys.F10))
			TakeScreenshot();

		if (Global.Keyboard.JustPressed(Keys.F11))
			ToggleFullscreen();
		#endregion

		if (_customMouseVisible)
			_cursor.position = Global.Mouse.Position;

		RoomManager.Update();
		base.Update(time);
	}

	protected virtual void FixedUpdate(FixedGameTime deltaTime)
	{
		RoomManager.FixedUpdate();
		_world?.Step(deltaTime, COLLISION_ITERATION);
	}

	/// <summary> Pixel rendering metod </summary>
	protected virtual void Draw(GameTime gameTime, bool pixelRender)
	{
		if (pixelRender)
			Global.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
		else
			Global.SpriteBatch.Begin();

		FPS.Draw();

		if (_customMouseVisible)
			_cursor.Draw();

		RoomManager.Draw();
		Global.SpriteBatch.End();

		base.Draw(gameTime);
	}

	#region Engine (private)Metods
	private void VSync()
	{
		_vSync = !_vSync;
		Graphics.SynchronizeWithVerticalRetrace = !_vSync;
		Graphics.ApplyChanges();
	}

	private void ToggleFullscreen()
	{
		Global.FullScreen = !Global.FullScreen;
		Graphics.IsFullScreen = Global.FullScreen;
		Graphics.PreferredBackBufferWidth = Global.WindowWidth;
		Graphics.PreferredBackBufferHeight = Global.WindowHeight;
		Graphics.ApplyChanges();
		GC.Collect();
	}

	private void TakeScreenshot()
	{
		const string folder = "Screenshots";
		if (!Directory.Exists(folder))
			Directory.CreateDirectory(folder);

		int size = GraphicsDevice.Viewport.Width * GraphicsDevice.Viewport.Height;
		if (_screenshotBuffer == null || _screenshotBuffer.Length != size)
			_screenshotBuffer = new Color[size];

		GraphicsDevice.GetBackBufferData(_screenshotBuffer);

		using var texture = new Texture2D(GraphicsDevice,
			GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
		texture.SetData(_screenshotBuffer);

		string path = Path.Combine(folder, $"screenshot_{DateTime.Now:yyyy.MM.dd_HH.mm.ss}.png");
		using var stream = File.OpenWrite(path);
		texture.SaveAsPng(stream, texture.Width, texture.Height);
	}
	#endregion
}