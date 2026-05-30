namespace AEngine;

public abstract class Component
{
    private Timer? _destroyTimer;
    private const ushort SECOND = 1000;

    internal bool IsUI = false;
    public bool IsPersistent { get; private set; }

    // Комната к которой принадлежит компонент
    internal Room? Owner { get; set; }

    public virtual void Start() { }
    public virtual void Update() => DestroyTimerUpdate();
    public virtual void FixedUpdate() { }
    public virtual void Draw(Vector2? offset = null, Vector2? origin = null) { }

    public void Destroy()
    {
        OnDestroy();
        Owner?.ScheduleDestroy(this);
    }

    public void Destroy(float lifeTime)
    {
        if (_destroyTimer != null) return;
        _destroyTimer = new((int)(lifeTime * SECOND));
    }

    public void DontDestroyOnLoad() => IsPersistent = true;

    protected virtual void OnDestroy() { }

    private void DestroyTimerUpdate()
    {
        if (_destroyTimer == null) return;
        _destroyTimer.Update();
        if (_destroyTimer.Test())
        {
            Owner?.ScheduleDestroy(this);
            _destroyTimer = null;
            OnDestroy();
        }
    }
}