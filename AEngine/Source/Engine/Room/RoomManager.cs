namespace AEngine;

public static class RoomManager
{
	private static Dictionary<string, IRoom> _allRooms = new();
	private static Stack<IRoom> _stack = new();

	// Регистрация — движок сам вызывает Load
	public static void Register(IRoom room)
	{
		if (_allRooms.ContainsKey(room.name))
			throw new InvalidOperationException($"Room '{room.name}' already registered.");

		room.Load();
		_allRooms[room.name] = room;
	}

	// Открыть комнату по имени
	public static void Push(string name)
	{
		if (!_allRooms.TryGetValue(name, out var room))
			throw new InvalidOperationException($"Room '{name}' not found.");

		_stack.Push(room);
	}

	// Открыть комнату напрямую
	public static void Push(IRoom room) => Push(room.name);

	// Закрыть текущую — вернуться к предыдущей
	public static void Pop()
	{
		if (_stack.Count > 1)
			_stack.Pop();
	}

	// Заменить текущую
	public static void Replace(string name)
	{
		if (_stack.Count > 0)
			_stack.Pop();

		Push(name);
	}

	public static IRoom Current => _stack.Count > 0
		? _stack.Peek()
		: throw new InvalidOperationException("No active room.");

	internal static void Update() => Current.Update();
	internal static void FixedUpdate() => Current.FixedUpdate();
	internal static void Draw() => Current.Draw();
	internal static void UnLoadContent() => Current.UnLoad();
}