namespace AEngine;

/*public abstract class Room : IRoom
{
    private readonly List<Component> _components = new();
    private readonly List<Component> _pendingDestroy = new();

    public abstract string name { get; }

    // Добавить компонент в комнату
    protected T Add<T>(T component) where T : Component
    {
        component.Owner = this;
        _components.Add(component);
        component.Start();
        return component;
    }

    internal void ScheduleDestroy(Component component)
        => _pendingDestroy.Add(component);

    // Уничтожить не персистентные компоненты при выходе
    public virtual void UnLoad()
    {
        foreach (var component in _components.ToList())
            if (!component.IsPersistent)
            {
                component.Destroy();
                _components.Remove(component);
            }
        _pendingDestroy.Clear();
    }

    public abstract void Load();

    public virtual void Update()
    {
        foreach (var component in _components.ToList())
            component.Update();

        foreach (var component in _pendingDestroy)
            _components.Remove(component);
        _pendingDestroy.Clear();
    }

    public virtual void FixedUpdate()
    {
        foreach (var component in _components)
            component.FixedUpdate();
    }

    public virtual void Draw()
    {
        foreach (var component in _components)
            if (!component.IsUI)
                component.Draw();
    }

    public virtual void DrawUI()
    {
        foreach (var component in _components)
            if (component.IsUI)
                component.Draw();
    }
}*/