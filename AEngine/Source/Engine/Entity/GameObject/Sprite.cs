namespace AEngine;

public sealed class Sprite //: IDisposable
{
    //private bool IsDisposed;

    private readonly Texture2D _texture;
    private readonly Layer _layer;

    private SpriteEffects _flip;
    private BasicEffect _effect;
    private Color _color;

    #region Properties
    public Texture2D texture { get => _texture; }
    public Layer layer { get => _layer; }
    public SpriteEffects flip
    {
        get => _flip;
        set => _flip = value;
    }
    public BasicEffect effect
    {
        get => _effect;
        set => _effect = value;
    }
    public Color color
    {
        get => _color;
        set => _color = value;
    }
    #endregion

    public Sprite(string pathTexture, byte depth = 1)
    {
        try
        {
            _texture = Main.Content.Load<Texture2D>(pathTexture ?? "Default/NullTexture");
        }
        catch (ContentLoadException)
        {
            _texture = Main.Content.Load<Texture2D>("Default/NullTexture");
        }

        //IsDisposed = false;

        _flip = SpriteEffects.None;

        /*effect = new(Core.GraphicsDevice);
        _effect.FogEnabled = false;
        _effect.TextureEnabled = true;
        _effect.LightingEnabled = false;
        _effect.VertexColorEnabled = true;

        _effect.World = Matrix.Identity;
        _effect.Projection = Matrix.Identity;
        _effect.View = Matrix.Identity;*/

        _color = Color.White;

        _layer = new(depth);
    }

    /*public void Dispose()
    {
        if (IsDisposed)
            return;

        Main.SpriteBatch?.Dispose();
        IsDisposed = true;
    }*/
}