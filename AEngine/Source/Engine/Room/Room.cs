namespace AEngine;

public interface IRoom
{
	public string name { get; }

	public void Load();
	public virtual void UnLoad(){}

	public void Update();
	public virtual void FixedUpdate(){}

	public void Draw();
}