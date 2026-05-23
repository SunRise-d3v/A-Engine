namespace AEngine;

public delegate void PassObject(object obj);
public delegate object PassObjectAndReturn(object obj);

public static class Global
{
    public static int WindowWidth, WindowHeight;
    public static bool FullScreen, IsMouseVisible;
    public static Random Rnd;

	public static SpriteBatch SpriteBatch;
    public static ContentManager Content;
    public static GameTime GameTime;

    public static Input.Keyboard Keyboard;
    public static Input.Mouse Mouse;

    public static void Init()
    {
        Rnd = new();
    }
}