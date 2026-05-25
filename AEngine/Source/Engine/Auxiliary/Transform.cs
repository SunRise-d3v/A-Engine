namespace AEngine;

public class Transform
{
    private Vector2 _position;
    private float _rotation;
    private Scale _scale;

    public Transform()
    {
        _scale = Scale.One;
        _rotation = 0f;
        _position = Vector2.Zero;
    }

    #region Properties
    public float x
    {
        get => _position.X;
        set => _position.X = value;
    }
    public float y
    {
        get => _position.Y;
        set => _position.Y = value;
    }
    public Vector2 position
    {
        get => _position;
        set => _position = value;
    }

    public Scale scale
    {
        get => _scale;
        set => _scale = value;
    }

    public float rotation
    {
        get => _rotation;
        set => _rotation = value % (MathF.PI * 2);
    }
    #endregion
}