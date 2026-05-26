namespace AEngine.Physics;

public sealed partial class Body2D
{
	public static bool CreateCircleBody(float radius, float density, bool isStatic, float restitution, out Body2D body)
	{
		body = null;

		float area = radius * radius * MathF.PI;

		if (area < PhysicsWorld.MIN_BODY_SIZE)
			return false;

		if (area > PhysicsWorld.MAX_BODY_ZISE)
			return false;

		if (density < PhysicsWorld.MIN_DENSITY)
			return false;

		if (density > PhysicsWorld.MAX_DENSITY)
			return false;

		restitution = Math2D.Clamp(restitution, 0f, 1f);

		float mass = 0f;
		float inertia = 0f;

		if (!isStatic)
		{
			mass = area * density;
			inertia = (1f / 2f) * mass * radius * radius;
		}

		body = new Body2D(density, mass, inertia, restitution, area, isStatic, radius, 0f, 0f, null, ShapeType.Circle);
		return true;
	}

	public static bool CreateBoxBody(float width, float height, float density, bool isStatic, float restitution, out Body2D body)
	{
		body = null;

		float area = width * height;

		if (area < PhysicsWorld.MIN_BODY_SIZE)
			return false;

		if (area > PhysicsWorld.MAX_BODY_ZISE)
			return false;

		if (density < PhysicsWorld.MIN_DENSITY)
			return false;

		if (density > PhysicsWorld.MAX_DENSITY)
			return false;

		restitution = Math2D.Clamp(restitution, 0f, 1f);

		float mass = 0f;
		float inertia = 0f;

		if (!isStatic)
		{
			mass = area * density;
			inertia = (1f / 12) * mass * (width * width + height * height);
		}

		Vector2D[] vertices = Body2D.CreateBoxVertices(width, height);

		body = new Body2D(density, mass, inertia, restitution, area, isStatic, 0f, width, height, vertices, ShapeType.Box);
		return true;
	}
}