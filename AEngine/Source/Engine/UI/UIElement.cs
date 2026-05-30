namespace AEngine.UI;

public class UIElement : Component
{
    private readonly Texture2D _texture;
    private Color _color;

    private Transform _transform;
    private Rectangle _rect;

    public Texture2D texture { get => _texture; }
    public Color color
    {
        get => _color;
        set => _color = value;
    }

    public Transform transform { get => _transform; }
    public Rectangle rectangle
    {
        get => _rect;
        set => _rect = value;
    }

    public UIElement(string path, Vector2 position)
    {
        _transform = new();
        IsUI = true;

        try { _texture = Main.Content.Load<Texture2D>(path); }
        catch (ContentLoadException) { _texture = Main.Content.Load<Texture2D>("Default/NullTexture"); }
        _transform.position = position;
       
        _color = Color.White;
    }

    public override void Draw(Vector2? offset = null, Vector2? origin = null)
    {
        if (_texture == null)
            return;
        _rect = new(
          (int)_transform.position.X,
          (int)_transform.position.Y,
          (int)(_texture.Width * _transform.scale.x),
          (int)(_texture.Height * _transform.scale.y));
        Main.SpriteBatch.Draw(_texture, _rect, _color);
    }
}