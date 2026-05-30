namespace AEngine.UI;

public class Button : UIElement
{
    public event Action OnClick;
    public event Action OnHover;
    public event Action OnHoverExit;

    private Color _shade = Color.DarkGray;
    public Color shade { get => _shade; set => _shade = value; }

    private bool _wasHovered;
    private bool _pressed;
    public bool pressed { get => _pressed; }

    public Button(string path, Vector2 position)
        : base(path, position)
    {
        OnClick += Click;
        OnHover += OnHover;
        OnHoverExit += OnHoverExit;
        //EventSystem.Buttons.Add(this);
    }

    public override void Update()
    {
        Rectangle cursor = new((int)Main.Mouse.Position.X, (int)Main.Mouse.Position.Y, 1, 1);
        bool hovered = cursor.Intersects(rectangle);

        if (hovered)
        {
            color = _shade;

            if (!_wasHovered)
                OnHover?.Invoke();

            if (Main.Mouse.LeftClick() || Main.Mouse.RightClick())
            {
                OnClick?.Invoke();
            }
        }
        else
        {
            color = Color.White;

            if (_wasHovered)
                OnHoverExit?.Invoke();
        }

        _wasHovered = hovered;
        base.Update();
    }

    public override void Draw(Vector2? offset = null, Vector2? origin = null) => base.Draw(offset, origin);

    public virtual void Click() { }
    public virtual void Hover() { }
    public virtual void HoverExit() { }
}