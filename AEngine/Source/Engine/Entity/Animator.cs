using AEngine.Source.Engine.Entity.GameObject;

namespace AEngine;

public class Animator
{
    private readonly Dictionary<string, Animation2D> _animations = new();
    private string _currentName = "";
    private Animation2D _current;

    public string Current => _currentName;

    public void Add(string name, Texture2D texture, int frameCount, float frameTime, AnimationConfig config = null)
        => _animations[name] = new(texture, frameCount, frameTime, config);

    public void Add(string name, string path, int frameCount, float frameTime, AnimationConfig config = null)
    {
        Texture2D texture;
        try { texture = Main.Content.Load<Texture2D>(path ?? "Default/NullTexture"); }
        catch (ContentLoadException) { texture = Main.Content.Load<Texture2D>("Default/NullTexture"); }
        _animations[name] = new(texture, frameCount, frameTime, config);
    }

    public void Play(string name)
    {
        if (_currentName == name) return;
        if (!_animations.TryGetValue(name, out var anim)) return;

        _current?.Stop();
        _current = anim;
        _currentName = name;
        _current.Reset();
        _current.Play();
    }

    public void Update(GameTime gameTime) => _current?.Update(gameTime);

    public void Draw(Vector2 position, Color color, float rotation, Vector2 scale, SpriteEffects effect, float layerDepth)
        => _current?.Draw(position, color, rotation, scale, effect, layerDepth);
}