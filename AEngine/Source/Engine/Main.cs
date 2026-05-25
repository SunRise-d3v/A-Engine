namespace AEngine;

public delegate void PassObject(object obj);
public delegate object PassObjectAndReturn(object obj);

public static class Main
{
    public static int WindowWidth, WindowHeight;
    public static bool FullScreen, IsMouseVisible;
    public static System.Random Rnd;

	public static SpriteBatch SpriteBatch;
    public static ContentManager Content;

    public static FixedGameTime FixedGameTime;
    public static GameTime GameTime;

    public static Input.Keyboard Keyboard;
    public static Input.Mouse Mouse;

    public static void Init(GraphicsDevice graphicsDevice, float fixedStep)
    {
        SpriteBatch = new(graphicsDevice);

        Keyboard = new();
        Mouse = new();

        FixedGameTime = new(fixedStep);

        Rnd = new();
    }
}