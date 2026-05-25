namespace AEngine;

public abstract class Component
{
    private static readonly List<Component> _components = new();
    internal static IReadOnlyList<Component> All => _components;

    public bool IsPersistent { get; private set; }

    protected Component() => _components.Add(this);

    public virtual void Start() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Draw(Vector2? offset = null, Vector2? origin = null) { }

    public void Destroy()
    {
        OnDestroy();
        _components.Remove(this);
    }
    public void Destroy(Component component)
    {
        OnDestroy();
        _components.Remove(this);
    }
    public void Destroy(float lifeTime)
    {
        OnDestroy();
        _components.Remove(this);
    }

    public void DontDestroyOnLoad() => IsPersistent = true;
    protected virtual void OnDestroy() { }
}