namespace AEngine.Physics;

public static class Polygon2D
{
	public static float Area(Vector2D[] vertices)
	{
		float area = 0f;

		for (int i = 0; i < vertices.Length; i++)
		{
			Vector2D a = vertices[i];
			Vector2D b = vertices[(i + 1) % vertices.Length];

			float width = b.x - a.x;
			float height = (b.y + b.x) * 0.5f;

			area += width * height;
		}

		return area;
	}

	public static bool PointInTriangle(Vector2D p, Vector2D a, Vector2D b, Vector2D c)
	{
		Vector2D ab = b - a;
		Vector2D bc = c - b;
		Vector2D ca = a - c;

		Vector2D ap = p - a;
		Vector2D bp = p - b;
		Vector2D cp = p - c;

		float c1 = Math2D.Cross(ap, ab);
		float c2 = Math2D.Cross(bp, bc);
		float c3 = Math2D.Cross(cp, ca);

		if (c1 <= 0f || c2 <= 0f || c3 <= 0f)
			return false;

		return true;
	}

	private static bool AnyVerticesInTriangle(Vector2D[] vertices, Vector2D a, Vector2D b, Vector2D c)
	{
		for (int j = 0; j < vertices.Length; j++)
		{
			Vector2D p = vertices[j];

			if (PointInTriangle(p, a, b, c))
				return true;
		}

		return false;
	}

	private static T GetItem<T>(List<T> list, int index)
	{
		int count = list.Count;

		if (index >= count)
			return list[index % count];
		else if (index < 0)
			return list[index % count + count];

		return list[index];
	}

	public static bool Triangulate(Vector2D[] vertices, [NotNullWhen(true)] out int[] triangleIndices)
	{
		triangleIndices = null;

		if (vertices is null)
			return false;

		if (vertices.Length < 3)
			return false;

		int triangleCount = vertices.Length - 2;
		int triangleIndicesCount = triangleCount * 3;

		triangleIndices = new int[triangleIndicesCount];
		int indexCount = 0;

		List<int> indices = new List<int>(vertices.Length);
		for (int i = 0; i < vertices.Length; i++)
			indices.Add(i);

		while (indices.Count > 3)
		{
			for (int i = 0; i < indices.Count; i++)
			{
				int a = Polygon2D.GetItem(indices, i - 1);
				int b = Polygon2D.GetItem(indices, i);
				int c = Polygon2D.GetItem(indices, i + 1);

				Vector2D va = vertices[a];
				Vector2D vb = vertices[b];
				Vector2D vc = vertices[c];

				// Test for convexity. If not convex move to next angle.
				if (Math2D.Cross(va - vb, vc - vb) <= 0f)
					continue;

				// Test for any points "inside" this triangle.
				if (AnyVerticesInTriangle(vertices, va, vb, vc))
					continue;

				triangleIndices[indexCount++] = a;
				triangleIndices[indexCount++] = b;
				triangleIndices[indexCount++] = c;

				indices.RemoveAt(i);

				break;
			}
		}

		triangleIndices[indexCount++] = indices[0];
		triangleIndices[indexCount++] = indices[1];
		triangleIndices[indexCount++] = indices[2];

		return true;
	}
}