namespace AEngine;

public class Velocity
{
    private Vector2 _force;
    public Vector2 force { get => _force; set => _force = value; }

    public Velocity() => _force = Vector2.Zero;

    public float x
    {
        get => _force.X;
        set => _force.X = value;
    }
    public float y
    {
        get => _force.Y;
        set => _force.Y = value;
    }
}