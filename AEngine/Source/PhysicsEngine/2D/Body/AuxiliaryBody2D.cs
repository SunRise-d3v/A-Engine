namespace AEngine.Physics;

public sealed partial class Body2D
{
#if false
	private static int[] CreateBoxTriangles()
	{
		int[] triangles = new int[6];

		triangles[0] = 0;
		triangles[1] = 1;
		triangles[2] = 2;
		triangles[3] = 0;
		triangles[4] = 2;
		triangles[5] = 3;

		return triangles;
	}
#endif
	public Vector2D[] GetTransformedVertices()
	{
		if (_transformUpdateRequired)
		{
			Transform transform = new(_position, _angle);

			for (int i = 0; i < _vertices.Length; i++)
			{
				Vector2D v = _vertices[i];
				_transformedVertices[i] = Vector2D.Transform(v, transform);
			}

			PhysicsWorld.TransformCount++;
		}
		else
			PhysicsWorld.NoTransformCount++;

		_transformUpdateRequired = false;
		return _transformedVertices;
	}

	public AABB GetAABB()
	{
		if (_aabbUpdateRequired)
		{
			float minX = float.MaxValue;
			float minY = float.MaxValue;
			float maxX = float.MinValue;
			float maxY = float.MinValue;

			if (shape is ShapeType.Box)
			{
				Vector2D[] vertices = GetTransformedVertices();

				for (int i = 0; i < vertices.Length; i++)
				{
					Vector2D v = vertices[i];

					if (v.x < minX)
					{ minX = v.x; }
					if (v.x > maxX)
					{ maxX = v.x; }
					if (v.y < minY)
					{ minY = v.y; }
					if (v.y > maxY)
					{ maxY = v.y; }
				}
			}
			else if (this.shape is ShapeType.Circle)
			{
				minX = this._position.x - this.radius;
				minY = this._position.y - this.radius;
				maxX = this._position.x + this.radius;
				maxY = this._position.y + this.radius;
			}

			_aabb = new(minX, minY, maxX, maxY);
		}

		_aabbUpdateRequired = false;
		return _aabb;
	}

	private static Vector2D[] CreateBoxVertices(float width, float height)
	{
		float left = -width / 2f;
		float right = left + width;
		float bottom = -height / 2f;
		float top = bottom + height;

		Vector2D[] vertices = new Vector2D[4];

		vertices[0] = new Vector2D(left, top);
		vertices[1] = new Vector2D(right, top);
		vertices[2] = new Vector2D(right, bottom);
		vertices[3] = new Vector2D(left, bottom);

		return vertices;
	}
}