namespace AEngine.Physics;

public enum ShapeType { Circle = 0, Box };
public sealed partial class Body2D
{
	#region Private
	private Vector2D _position;
	private Vector2D _linearVelocity;
	private float _angle;
	private float _angularVelocity;
	private Vector2D _force;

	private readonly Vector2D[] _vertices;
	private Vector2D[] _transformedVertices;
	private AABB _aabb;

	private bool _transformUpdateRequired;
	private bool _aabbUpdateRequired;
	#endregion

	#region Public
	public readonly ShapeType shape;
	public readonly float density;
	public readonly float mass;
	public readonly float invMass;
	public readonly float restitution;
	public readonly float area;
	public readonly float inertia;
	public readonly float invInertia;
	public readonly bool IsStatic;
	public readonly float radius;
	public readonly float width;
	public readonly float height;
	public readonly float staticFriction;
	public readonly float dynamicFriction;
	#endregion

	#region Properties
	public Vector2D position { get => _position; }
	public Vector2D linearVelocity
	{
		get => _linearVelocity;
		internal set => _linearVelocity = value;
	}
	public float Angle { get => _angle; }
	public float angularVelocity
	{
		get => _angularVelocity;
		internal set => _angularVelocity = value;
	}
	#endregion

	private Body2D(float density, float mass, float inertia, float restitution, float area,
		bool isStatic, float radius, float width, float height, Vector2D[] vertices, ShapeType shapeType)
	{
		this._position = Vector2D.Zero;
		this._linearVelocity = Vector2D.Zero;
		this._angle = 0f;
		this._angularVelocity = 0f;
		this._force = Vector2D.Zero;

		this.shape = shapeType;
		this.density = density;
		this.mass = mass;
		this.invMass = mass > 0f ? 1f / mass : 0f;
		this.inertia = inertia;
		this.invInertia = inertia > 0f ? 1f / inertia : 0f;
		this.restitution = restitution;
		this.area = area;
		IsStatic = isStatic;
		this.radius = radius;
		this.width = width;
		this.height = height;
		this.staticFriction = 0.6f;
		this.dynamicFriction = 0.4f;

		if (this.shape is ShapeType.Box)
		{
			_vertices = vertices;
			_transformedVertices = new Vector2D[this._vertices.Length];
		}
		else
		{
			_vertices = null;
			_transformedVertices = null;
		}

		_transformUpdateRequired = true;
		_aabbUpdateRequired = true;
	}

	internal void Step(float time, Vector2D gravity, int iterations)
	{
		if (this.IsStatic)
			return;

		time /= (float)iterations;

		// force = mass * acc
		// acc = force / mass;

		//Vector2D acceleration = this.force / this.Mass;
		//this.linearVelocity += acceleration * time;


		_linearVelocity += gravity * time;
		_position += _linearVelocity * time;

		_angle += _angularVelocity * time;

		_force = Vector2D.Zero;
		_transformUpdateRequired = true;
		_aabbUpdateRequired = true;
	}

	public void Move(Vector2D amount)
	{
		this._position += amount;
		_transformUpdateRequired = true;
		_aabbUpdateRequired = true;
	}

	public void MoveTo(Vector2D _position)
	{
		this._position = _position;
		_transformUpdateRequired = true;
		_aabbUpdateRequired = true;
	}

	public void Rotate(float amount)
	{
		this._angle += amount;
		_transformUpdateRequired = true;
		_aabbUpdateRequired = true;
	}

	public void RotateTo(float angle)
	{
		this._angle = angle;
		_transformUpdateRequired = true;
		_aabbUpdateRequired = true;
	}

	public void AddForce(Vector2D amount) => _force = amount;
}