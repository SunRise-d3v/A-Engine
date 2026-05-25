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
            -target.transform.position.X - (target.texture.Width / 2),
            -target.transform.position.Y - (target.texture.Height / 2),
            0f);

        Matrix offset = Matrix.CreateTranslation(
            Main.WindowWidth / 2,
            Main.WindowHeight / 2,
            0f);

        _transform = position * offset;
    }
}