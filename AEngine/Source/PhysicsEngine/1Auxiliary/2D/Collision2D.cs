namespace AEngine.Physics;

public static class Collision2D
{
	public static void PointSegmentDistance(Vector2D p, Vector2D a, Vector2D b,
		out float distanceSquared, out Vector2D cp)
	{
		Vector2D ab = b - a;
		Vector2D ap = p - a;

		float proj = Math2D.Dot(ap, ab);
		float abLenSq = Math2D.LengthSquared(ab);
		float d = proj / abLenSq;

		if (d <= 0f)
			cp = a;
		else if (d >= 1f)
			cp = b;
		else
			cp = a + ab * d;

		distanceSquared = Math2D.DistanceSquared(p, cp);
	}

	public static bool IntersectAABBs(AABB a, AABB b)
	{
		if (a.max.x <= b.min.x || b.max.x <= a.min.x ||
			a.max.y <= b.min.y || b.max.y <= a.min.y)
			return false;

		return true;
	}

	public static void FindContactPoints(Body2D bodyA, Body2D bodyB,
		out Vector2D contact1, out Vector2D contact2, out int contactCount)
	{
		contact1 = Vector2D.Zero;
		contact2 = Vector2D.Zero;
		contactCount = 0;

		ShapeType shapeTypeA = bodyA.shape;
		ShapeType shapeTypeB = bodyB.shape;

		if (shapeTypeA is ShapeType.Box)
		{
			if (shapeTypeB is ShapeType.Box)
				FindPolygonsContactPoints(bodyA.GetTransformedVertices(), bodyB.GetTransformedVertices(),
					out contact1, out contact2, out contactCount);
			else if (shapeTypeB is ShapeType.Circle)
				FindCirclePolygonContactPoint(bodyB.position, bodyB.radius, bodyA.position, bodyA.GetTransformedVertices(), out contact1);
			contactCount = 1;
		}
		else if (shapeTypeA is ShapeType.Circle)
		{
			if (shapeTypeB is ShapeType.Box)
			{
				FindCirclePolygonContactPoint(bodyA.position, bodyA.radius, bodyB.position, bodyB.GetTransformedVertices(), out contact1);
				contactCount = 1;
			}
			else if (shapeTypeB is ShapeType.Circle)
			{
				FindCirclesContactPoint(bodyA.position, bodyA.radius, bodyB.position, out contact1);
				contactCount = 1;
			}
		}
	}

	private static void FindPolygonsContactPoints(Vector2D[] verticesA, Vector2D[] verticesB,
		out Vector2D contact1, out Vector2D contact2, out int contactCount)
	{
		contact1 = Vector2D.Zero;
		contact2 = Vector2D.Zero;
		contactCount = 0;

		float minDistSq = float.MaxValue;

		for (int i = 0; i < verticesA.Length; i++)
		{
			Vector2D p = verticesA[i];

			for (int j = 0; j < verticesB.Length; j++)
			{
				Vector2D va = verticesB[j];
				Vector2D vb = verticesB[(j + 1) % verticesB.Length];

				PointSegmentDistance(p, va, vb, out float distSq, out Vector2D cp);

				if (Math2D.NearlyEqual(distSq, minDistSq))
				{
					if (!Math2D.NearlyEqual(cp, contact1))
					{
						contact2 = cp;
						contactCount = 2;
					}
				}
				else if (distSq < minDistSq)
				{
					minDistSq = distSq;
					contactCount = 1;
					contact1 = cp;
				}
			}
		}

		for (int i = 0; i < verticesB.Length; i++)
		{
			Vector2D p = verticesB[i];

			for (int j = 0; j < verticesA.Length; j++)
			{
				Vector2D va = verticesA[j];
				Vector2D vb = verticesA[(j + 1) % verticesA.Length];

				PointSegmentDistance(p, va, vb, out float distSq, out Vector2D cp);

				if (Math2D.NearlyEqual(distSq, minDistSq))
				{
					if (!Math2D.NearlyEqual(cp, contact1))
					{
						contact2 = cp;
						contactCount = 2;
					}
				}
				else if (distSq < minDistSq)
				{
					minDistSq = distSq;
					contactCount = 1;
					contact1 = cp;
				}
			}
		}
	}

	private static void FindCirclePolygonContactPoint(Vector2D circleCenter, float circleRadius,
		Vector2D polygonCenter, Vector2D[] polygonVertices, out Vector2D cp)
	{
		cp = Vector2D.Zero;

		float minDistSq = float.MaxValue;

		for (int i = 0; i < polygonVertices.Length; i++)
		{
			Vector2D va = polygonVertices[i];
			Vector2D vb = polygonVertices[(i + 1) % polygonVertices.Length];

			PointSegmentDistance(circleCenter, va, vb, out float distSq, out Vector2D contact);

			if (distSq < minDistSq)
			{
				minDistSq = distSq;
				cp = contact;
			}
		}
	}

	private static void FindCirclesContactPoint(Vector2D centerA, float radiusA,
		Vector2D centerB, out Vector2D cp)
	{
		Vector2D ab = centerB - centerA;
		Vector2D dir = Math2D.Normalize(ab);
		cp = centerA + dir * radiusA;
	}

	public static bool Collide(Body2D bodyA, Body2D bodyB,
		out Vector2D normal, out float depth)
	{
		normal = Vector2D.Zero;
		depth = 0f;

		ShapeType shapeTypeA = bodyA.shape;
		ShapeType shapeTypeB = bodyB.shape;

		if (shapeTypeA is ShapeType.Box)
		{
			if (shapeTypeB is ShapeType.Box)
				return IntersectPolygons(
					bodyA.position, bodyA.GetTransformedVertices(),
					bodyB.position, bodyB.GetTransformedVertices(),
					out normal, out depth);
			else if (shapeTypeB is ShapeType.Circle)
			{
				bool result = IntersectCirclePolygon(
					bodyB.position, bodyB.radius,
					bodyA.position, bodyA.GetTransformedVertices(),
					out normal, out depth);

				normal = -normal;
				return result;
			}
		}
		else if (shapeTypeA is ShapeType.Circle)
		{
			if (shapeTypeB is ShapeType.Box)
				return IntersectCirclePolygon(
					bodyA.position, bodyA.radius,
					bodyB.position, bodyB.GetTransformedVertices(),
					out normal, out depth);
			else if (shapeTypeB is ShapeType.Circle)
				return IntersectCircles(
					bodyA.position, bodyA.radius,
					bodyB.position, bodyB.radius,
					out normal, out depth);
		}

		return false;
	}

	public static bool IntersectCirclePolygon(Vector2D circleCenter, float circleRadius,
		Vector2D polygonCenter, Vector2D[] vertices, out Vector2D normal, out float depth)
	{
		normal = Vector2D.Zero;
		depth = float.MaxValue;

		Vector2D axis = Vector2D.Zero;
		float axisDepth = 0f;
		float minA, maxA, minB, maxB;

		for (int i = 0; i < vertices.Length; i++)
		{
			Vector2D va = vertices[i];
			Vector2D vb = vertices[(i + 1) % vertices.Length];

			Vector2D edge = vb - va;
			axis = new Vector2D(-edge.y, edge.x);
			axis = Math2D.Normalize(axis);

			ProjectVertices(vertices, axis, out minA, out maxA);
			ProjectCircle(circleCenter, circleRadius, axis, out minB, out maxB);

			if (minA >= maxB || minB >= maxA)
				return false;

			axisDepth = MathF.Min(maxB - minA, maxA - minB);

			if (axisDepth < depth)
			{
				depth = axisDepth;
				normal = axis;
			}
		}

		int cpIndex = FindClosestPointOnPolygon(circleCenter, vertices);
		Vector2D cp = vertices[cpIndex];

		axis = cp - circleCenter;
		axis = Math2D.Normalize(axis);

		ProjectVertices(vertices, axis, out minA, out maxA);
		ProjectCircle(circleCenter, circleRadius, axis, out minB, out maxB);

		if (minA >= maxB || minB >= maxA)
			return false;

		axisDepth = MathF.Min(maxB - minA, maxA - minB);

		if (axisDepth < depth)
		{
			depth = axisDepth;
			normal = axis;
		}

		Vector2D direction = polygonCenter - circleCenter;

		if (Math2D.Dot(direction, normal) < 0f)
			normal = -normal;

		return true;
	}

	private static int FindClosestPointOnPolygon(Vector2D circleCenter, Vector2D[] vertices)
	{
		int result = -1;
		float minDistance = float.MaxValue;

		for (int i = 0; i < vertices.Length; i++)
		{
			Vector2D v = vertices[i];
			float distance = Math2D.Distance(v, circleCenter);

			if (distance < minDistance)
			{
				minDistance = distance;
				result = i;
			}
		}

		return result;
	}

	private static void ProjectCircle(Vector2D center, float radius, Vector2D axis,
		out float min, out float max)
	{
		Vector2D direction = Math2D.Normalize(axis);
		Vector2D directionAndRadius = direction * radius;

		Vector2D p1 = center + directionAndRadius;
		Vector2D p2 = center - directionAndRadius;

		min = Math2D.Dot(p1, axis);
		max = Math2D.Dot(p2, axis);

		if (min > max)
		{
			float t = min;
			min = max;
			max = t;
		}
	}

	public static bool IntersectPolygons(Vector2D centerA, Vector2D[] verticesA,
		Vector2D centerB, Vector2D[] verticesB, out Vector2D normal, out float depth)
	{
		normal = Vector2D.Zero;
		depth = float.MaxValue;

		for (int i = 0; i < verticesA.Length; i++)
		{
			Vector2D va = verticesA[i];
			Vector2D vb = verticesA[(i + 1) % verticesA.Length];

			Vector2D edge = vb - va;
			Vector2D axis = new Vector2D(-edge.y, edge.x);
			axis = Math2D.Normalize(axis);

			ProjectVertices(verticesA, axis, out float minA, out float maxA);
			ProjectVertices(verticesB, axis, out float minB, out float maxB);

			if (minA >= maxB || minB >= maxA)
				return false;

			float axisDepth = MathF.Min(maxB - minA, maxA - minB);

			if (axisDepth < depth)
			{
				depth = axisDepth;
				normal = axis;
			}
		}

		for (int i = 0; i < verticesB.Length; i++)
		{
			Vector2D va = verticesB[i];
			Vector2D vb = verticesB[(i + 1) % verticesB.Length];

			Vector2D edge = vb - va;
			Vector2D axis = new Vector2D(-edge.y, edge.x);
			axis = Math2D.Normalize(axis);

			ProjectVertices(verticesA, axis, out float minA, out float maxA);
			ProjectVertices(verticesB, axis, out float minB, out float maxB);

			if (minA >= maxB || minB >= maxA)
				return false;

			float axisDepth = MathF.Min(maxB - minA, maxA - minB);

			if (axisDepth < depth)
			{
				depth = axisDepth;
				normal = axis;
			}
		}

		Vector2D direction = centerB - centerA;

		if (Math2D.Dot(direction, normal) < 0f)
			normal = -normal;

		return true;
	}

	private static void ProjectVertices(Vector2D[] vertices, Vector2D axis,
		out float min, out float max)
	{
		min = float.MaxValue;
		max = float.MinValue;

		for (int i = 0; i < vertices.Length; i++)
		{
			Vector2D v = vertices[i];
			float proj = Math2D.Dot(v, axis);

			if (proj < min)
min = proj; 			if (proj > max)
max = proj; 		}
	}

	public static bool IntersectCircles(
		Vector2D centerA, float radiusA,
		Vector2D centerB, float radiusB,
		out Vector2D normal, out float depth)
	{
		normal = Vector2D.Zero;
		depth = 0f;

		float distance = Math2D.Distance(centerA, centerB);
		float radii = radiusA + radiusB;

		if (distance >= radii)
			return false;

		normal = Math2D.Normalize(centerB - centerA);
		depth = radii - distance;

		return true;
	}
}