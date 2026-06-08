namespace AEngine;

public enum Window_Resolution { Small, Medium, High };
public partial class Core : Game
{
    #region Xna
    internal static Core s_instance;
    public static Core Instance => s_instance;

    public static GraphicsDeviceManager Graphics { get; private set; }
    public static new GraphicsDevice GraphicsDevice { get; private set; }

    private RenderTarget2D _renderTarget;
    #endregion

    #region Engine
    private float _accumulator;
    internal const float FIXED_STEP = 1f / 60f; // 60 updates/sec
    private const byte COLLISION_ITERATION = 16;
    private PhysicsWorld _world;

    private readonly FixedGameTime _fixedGameTime;
    private bool _vSync;

    private Basic2D _cursor;
    private bool _customMouseVisible;
    private string _cursorPath;
    protected string CursorPath { set => _cursorPath = string.IsNullOrEmpty(value) ? _cursorPath : value; }

    private Color[] _screenshotBuffer;

    private Rectangle _renderDestination;
    private bool _isResizing;
    private bool _resolution;
    #endregion

    public Camera MainCamera;

    public Core(string title, int width, int height,
        bool fullScreen = false, bool resolution = false, bool vSync = true, bool mouseVisible = true)
    {
        if (s_instance != null)
            throw new InvalidOperationException($"Only a single Core instance can be created");

        s_instance = this;

        IsFixedTimeStep = false;
        _vSync = vSync;
        _resolution = resolution;

        Graphics = new GraphicsDeviceManager(this);
        _fixedGameTime = new(FIXED_STEP);
        Main.IsMouseVisible = mouseVisible;

        Main.WindowWidth = width;
        Main.WindowHeight = height;
        Main.FullScreen = fullScreen;

        Graphics.PreferredBackBufferWidth = Main.WindowWidth;
        Graphics.PreferredBackBufferHeight = Main.WindowHeight;
        Graphics.IsFullScreen = Main.FullScreen;

        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += OnClientSizeChanged;

        VSync();

        Window.Title = title;

        Main.Content = base.Content;
        Main.Content.RootDirectory = "Content";

        _cursorPath = "Sprites/DefaultCursor";
        IsMouseVisible = Main.IsMouseVisible;
        _customMouseVisible = !IsMouseVisible;

        Components.Add(new FPS(this));
    }

    protected override void Initialize()
    {
        base.Initialize();

        _world = new();
        GraphicsDevice = base.GraphicsDevice;
        Main.Init();

        MainCamera = new();

        if (_customMouseVisible)
            _cursor = new(_cursorPath, Vector2.Zero, new Vector2(12, 19));

        _renderTarget = new(GraphicsDevice, Main.WindowWidth, Main.WindowHeight);
        CalculateRenderDestination();
    }

    protected override void UnloadContent()
    {
        RoomManager.UnLoadContent();
        base.UnloadContent();
    }

    protected override void Update(GameTime time)
    {
        Main.GameTime = time;

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

        Main.Keyboard.Update();
        Main.Mouse.Update();

        #region Default Keys
        if (Main.Keyboard.JustReleased(Keys.Escape) ||
            (Main.Keyboard.IsPressed(Keys.LeftAlt) && Main.Keyboard.JustReleased(Keys.F4)) ||
            (Main.Keyboard.IsPressed(Keys.RightAlt) && Main.Keyboard.JustReleased(Keys.F4)))
            Exit();

        if (Main.Keyboard.JustPressed(Keys.F1))
            VSync();

        if (Main.Keyboard.JustPressed(Keys.F3))
            FPS.Toggle();

        if (Main.Keyboard.JustPressed(Keys.F10))
            TakeScreenshot();

        if (Main.Keyboard.JustPressed(Keys.F11))
            ToggleFullscreen();
        #endregion

        if (_customMouseVisible)
            _cursor.transform.position = Main.Mouse.Position;

        RoomManager.Update();
        Component.UpdateComponent();

        base.Update(time);
    }

    protected virtual void FixedUpdate(FixedGameTime deltaTime)
    {
        Main.FixedGameTime = deltaTime;
        _world?.Step(deltaTime, COLLISION_ITERATION);

        RoomManager.FixedUpdate();
        Component.FixedUpdateComponent();
    }
}