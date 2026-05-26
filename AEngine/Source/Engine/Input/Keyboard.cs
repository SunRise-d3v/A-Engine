namespace AEngine.Input;

public sealed class Keyboard
{
    private KeyboardState _current;
    private KeyboardState _previous;

    public void Update()
    {
        _previous = _current;
        _current = Microsoft.Xna.Framework.Input.Keyboard.GetState();
    }

    public bool IsPressed(Keys key) => _current.IsKeyDown(key);
    public bool IsClicked(Keys key) => _current.IsKeyDown(key) || !_previous.IsKeyDown(key);
    public bool JustPressed(Keys key) => _current.IsKeyDown(key) && _previous.IsKeyUp(key);
    public bool JustReleased(Keys key) => _current.IsKeyUp(key) && _previous.IsKeyDown(key);
}