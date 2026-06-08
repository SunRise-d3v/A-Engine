namespace AEngine;

public partial class Core
{
    /// <summary> Pixel game render metod </summary>
    protected override void Draw(GameTime gameTime)
    {
        CameraRender();
        FinalRender();
        UIRender();

        base.Draw(gameTime);
    }

    #region Draws Metods
    private void CameraRender()
    {
        GraphicsDevice.SetRenderTarget(_renderTarget);

        Main.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend,
            SamplerState.PointClamp, rasterizerState: RasterizerState.CullNone, transformMatrix: MainCamera.transform);

        RoomManager.Draw();
        Component.DrawComponent();

        Main.SpriteBatch.End();
    }

    private void FinalRender()
    {
        GraphicsDevice.SetRenderTarget(null);

        Main.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
            SamplerState.PointClamp);

        Main.SpriteBatch.Draw(_renderTarget, Vector2.Zero, Color.White);

        Main.SpriteBatch.End();
    }

    private void UIRender()
    {
        Main.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
            SamplerState.PointClamp);

        FPS.Draw();
        if (_customMouseVisible)
            _cursor.Draw();

        Component.DrawUIComponent();

        Main.SpriteBatch.End();
    }
    #endregion
}