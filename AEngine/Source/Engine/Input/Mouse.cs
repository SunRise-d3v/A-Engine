namespace AEngine.Input;

public class Mouse
{
    private MouseState _current, _previous, _first;
    private Vector2 _currentPos, _previousPos, _firstPos;

    public bool Dragging { get; private set; }
    public bool RightDragging { get; private set; }

    public MouseState Current => _current;
    public MouseState Previous => _previous;
    public MouseState First => _first;

    public Vector2 Position => _currentPos;
    public Vector2 PreviousPos => _previousPos;
    public Vector2 FirstPosition => _firstPos;

    public Mouse()
    {
        _current = Microsoft.Xna.Framework.Input.Mouse.GetState();
        _previous = _current;
        _first = _current;

        _currentPos = _previousPos = _firstPos = _current.Position.ToVector2();
    }

    public void Update()
    {
        _previous = _current;
        _previousPos = _currentPos;

        _current = Microsoft.Xna.Framework.Input.Mouse.GetState();
        _currentPos = _current.Position.ToVector2();

        if (IsJustPressed(_current.LeftButton, _previous.LeftButton))
        {
            _first = _current;
            _firstPos = _currentPos;
        }
    }

    #region Left
    public virtual bool LeftClick() => IsJustPressed(_current.LeftButton, _previous.LeftButton) && IsInWindow();
    public virtual bool LeftClickReleased() => IsJustReleased(_current.LeftButton, _previous.LeftButton);

    public virtual bool LeftClickHold()
    {
        if (_current.LeftButton != ButtonState.Pressed ||
            _previous.LeftButton != ButtonState.Pressed ||
            !IsInWindow())
            return false;

        if (Vector2.Distance(_currentPos, _firstPos) > 8f)
            Dragging = true;

        return true;
    }
    #endregion

    #region Right
    public virtual bool RightClick() => IsJustPressed(_current.RightButton, _previous.RightButton) && IsInWindow();
    public virtual bool RightClickReleased() => IsJustReleased(_current.RightButton, _previous.RightButton);

    public virtual bool RightClickHold()
    {
        if (_current.RightButton != ButtonState.Pressed ||
            _previous.RightButton != ButtonState.Pressed ||
            !IsInWindow())
            return false;

        if (Vector2.Distance(_currentPos, _firstPos) > 8f)
            RightDragging = true;

        return true;
    }
    #endregion

    public int ScrollDelta() => _current.ScrollWheelValue - _previous.ScrollWheelValue;
    public float DistanceFromClick() => Vector2.Distance(_currentPos, _firstPos);
    public float RotateTowards(Vector2 position, Vector2 focus) => (float)Math.Atan2(focus.Y - position.Y, focus.X - position.X);

    #region Private
    private bool IsJustPressed(ButtonState current, ButtonState previous)
        => current == ButtonState.Pressed && previous == ButtonState.Released;

    private bool IsJustReleased(ButtonState current, ButtonState previous)
        => current == ButtonState.Released && previous == ButtonState.Pressed;

    private bool IsInWindow()
        => _current.Position.X >= 0 && _current.Position.X <= Main.WindowWidth
        && _current.Position.Y >= 0 && _current.Position.Y <= Main.WindowHeight;
    #endregion
}