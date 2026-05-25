namespace AEngine;

public partial class Core
{
    #region Engine (private)Metods
    private void VSync()
    {
        _vSync = !_vSync;
        Graphics.SynchronizeWithVerticalRetrace = !_vSync;
        Graphics.ApplyChanges();
    }

    private void ToggleFullscreen()
    {
        Main.FullScreen = !Main.FullScreen;
        Graphics.IsFullScreen = Main.FullScreen;
        Graphics.ApplyChanges();
        GC.Collect();
    }

    private void TakeScreenshot()
    {
        const string folder = "Screenshots";
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        int size = GraphicsDevice.Viewport.Width * GraphicsDevice.Viewport.Height;
        if (_screenshotBuffer == null || _screenshotBuffer.Length != size)
            _screenshotBuffer = new Color[size];

        GraphicsDevice.GetBackBufferData(_screenshotBuffer);

        using var texture = new Texture2D(GraphicsDevice,
            GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        texture.SetData(_screenshotBuffer);

        string path = Path.Combine(folder, $"screenshot_{DateTime.Now:yyyy.MM.dd_HH.mm.ss}.png");
        using var stream = File.OpenWrite(path);
        texture.SaveAsPng(stream, texture.Width, texture.Height);
    }

    private void CalculateRenderDestination()
    {
        Point size = GraphicsDevice.Viewport.Bounds.Size;

        float scaleX = (float)size.X / _renderDestination.Width;
        float scaleY = (float)size.Y / _renderDestination.Height;
        float scale = MathF.Min(scaleX, scaleY);

        _renderDestination.Width = (int)(_renderTarget.Width * scale);
        _renderDestination.Height = (int)(_renderTarget.Height * scale);

        _renderDestination.X = (size.X - _renderDestination.Width) / 2;
        _renderDestination.Y = (size.Y - _renderDestination.Height) / 2;
    }

    private void OnClientSizeChanged(object sender, EventArgs eventArgs)
    {
        if (!_isResizing &&
            Window.ClientBounds.Width > 0 && Window.ClientBounds.Height > 0)
        {
            _isResizing = true;
            CalculateRenderDestination();
            _isResizing = false;
        }
    }
    #endregion
}