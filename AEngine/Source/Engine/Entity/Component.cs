namespace AEngine;
public abstract class Component
{
    private static readonly List<Component> _components = new();
    private static readonly List<Component> _pendingDestroy = new();
    internal static IReadOnlyList<Component> All => _components;
    internal bool IsUI = false;
    private Timer _destroyTimer;
    private const ushort SECOND = 1000;
    public bool IsPersistent { get; private set; }
    protected Component() => _components.Add(this);
    public virtual void Start() { }
    public virtual void Update() => DestroyTimerUpdate();
    public virtual void FixedUpdate() { }
    public virtual void Draw(Vector2? offset = null, Vector2? origin = null) { }
    public void Destroy()
    {
        OnDestroy();
        _pendingDestroy.Add(this);
    }
    public void Destroy(Component component)
    {
        OnDestroy();
        _pendingDestroy.Add(component);
    }
    public void Destroy(float lifeTime)
    {
        if (_destroyTimer != null)
            return;
        _destroyTimer = new((int)(lifeTime * SECOND));
    }
    public void DontDestroyOnLoad() => IsPersistent = true;
    protected virtual void OnDestroy() { }

    #region (Core)Public
    public static void StartComponent()
    {
        foreach (var component in _components)
            component.Start();
    }
    public static void UpdateComponent()
    {
        foreach (var component in Component.All.ToList())
            component.Update();
    }
    public static void FixedUpdateComponent()
    {
        foreach (var component in Component.All)
            component.FixedUpdate();
    }
    public static void DrawComponent()
    {
        foreach (var component in Component.All)
            if (!component.IsUI)
                component.Draw();
    }
    public static void DrawUIComponent()
    {
        foreach (var component in Component.All)
            if (component.IsUI)
                component.Draw();
    }
    public static void ClearDestroyList()
    {
        foreach (var component in _pendingDestroy)
            _components.Remove(component);
        _pendingDestroy.Clear();
    }
    #endregion

    #region Private
    private void DestroyTimerUpdate()
    {
        if (_destroyTimer != null)
        {
            _destroyTimer.Update();
            if (_destroyTimer.Test())
            {
                _pendingDestroy.Add(this);
                _destroyTimer = null;
                OnDestroy();
            }
        }
    }
    #endregion
}