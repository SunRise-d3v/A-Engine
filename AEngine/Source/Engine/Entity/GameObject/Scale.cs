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

    #region Properties
    public static Scale One => new(1, 1);
    public static Scale Two => new(2, 2);
    public static Scale Three => new(3, 3);
    public static Scale Four => new(4, 4);
    public static Scale Five => new(5, 5);
    public static Scale Six => new(6, 6);
    public static Scale Seven => new(7, 7);
    public static Scale Eight => new(8, 8);
    public static Scale Nine => new(9, 9);
    public static Scale Ten => new(10, 10);
    #endregion

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