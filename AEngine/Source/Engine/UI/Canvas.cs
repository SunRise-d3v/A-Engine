namespace AEngine.UI;

internal static class Canvas
{
    public static SpriteFont font =
        Main.Content.Load<SpriteFont>("Fonts/Arial");

    internal static SpriteFont fpsFont =
        Main.Content.Load<SpriteFont>("Fonts/FPSFont");

    public static Layer Background = new(0);
    public static Layer Default = new(1);
    public static Layer Entity = new(2);
    public static Layer Foreground = new(3);
    public static Layer UI = new(byte.MaxValue);
}