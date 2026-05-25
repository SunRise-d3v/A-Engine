namespace AEngine;

public class Scale
{
    private Vector2 _size;

    public Scale()
    {
        _size.X = 1;
        _size.Y = 1;
    }
    public Scale(float x, float y)
    {
        _size.X = x;
        _size.Y = y;
    }
   
    public static Scale One => new(1, 1);

    public float x
    {
        get => _size.X;
        set => _size.X = value;
    }
    public float y
    {
        get => _size.Y;
        set => _size.Y = value;
    }
}