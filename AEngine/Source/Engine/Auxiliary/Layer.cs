namespace AEngine;

public sealed class Layer
{
    private readonly float _depth;
    public float depth { get => _depth; }

    internal Layer() => _depth = 0f;
    internal Layer(byte layer) => _depth = layer / 255f;
}