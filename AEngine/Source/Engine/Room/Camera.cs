using AEngine.Source.Engine.Entity.GameObject;

namespace AEngine;

public sealed class Camera
{
    private Matrix _transform;
    public Matrix transform { get => _transform; }

    public void Update(Basic2D target = null)
    {
        if (target == null)
            return;

        Matrix position = Matrix.CreateTranslation(
            -target.transform.position.X - (target.sprite.texture.Width / 2),
            -target.transform.position.Y - (target.sprite.texture.Height / 2),
            0f);

        Matrix offset = Matrix.CreateTranslation(
            Main.WindowWidth / 2,
            Main.WindowHeight / 2,
            0f);

        _transform = position * offset;
    }
}