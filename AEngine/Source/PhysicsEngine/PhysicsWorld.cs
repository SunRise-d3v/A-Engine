namespace AEngine.Physics;

public sealed class PhysicsWorld
{
	public static int TransformCount = 0;
	public static int NoTransformCount = 0;

	public const float MIN_BODY_SIZE = 0.01f * 0.01f;
	public const float MAX_BODY_ZISE = 64f * 64f;

	public const float MIN_DENSITY = 0.5f;     // g/cm^3
	public const float MAX_DENSITY = 21.4f;

	public const int MIN_ITERATIONS = 1;
	public const int MAX_ITERATIONS = 64;

	#region Private
	private Vector2D _gravity;
	private List<Body2D> _bodyList;
	private List<(int, int)> _contactPairs;

	private Vector2D[] _contactList;
	private Vector2D[] _impulseList;
	private Vector2D[] _raList;
	private Vector2D[] _rbList;
	private Vector2D[] _frictionImpulseList;
	private float[] _jList;
	#endregion

	public int bodyCount { get => _bodyList.Count; }

	public PhysicsWorld()
	{
		this._gravity = new Vector2D(0f, -9.81f);
		this._bodyList = new List<Body2D>();
		this._contactPairs = new List<(int, int)>();

		this._contactList = new Vector2D[2];
		this._impulseList = new Vector2D[2];
		this._raList = new Vector2D[2];
		this._rbList = new Vector2D[2];
		this._frictionImpulseList = new Vector2D[2];
		this._jList = new float[2];
	}

	public void AddBody(Body2D body) => _bodyList.Add(body);
	public bool RemoveBody(Body2D body) => _bodyList.Remove(body);
	public bool GetBody(int index, out Body2D body)
	{
		body = null;

		if (index < 0 || index >= this._bodyList.Count)
			return false;

		body = this._bodyList[index];
		return true;
	}

	public void Step(FixedGameTime time, int totalIterations)
	{
		totalIterations = Math2D.Clamp(totalIterations, MIN_ITERATIONS, MAX_ITERATIONS);

		for (int currentIteration = 0; currentIteration < totalIterations; currentIteration++)
		{
			this._contactPairs.Clear();
			StepBodies(time.GetElapsedSecond(), totalIterations);
			BroadPhase();
			NarrowPhase();
		}
	}

	private void BroadPhase()
	{
		for (int i = 0; i < _bodyList.Count - 1; i++)
		{
			Body2D bodyA = _bodyList[i];
			AABB bodyA_aabb = bodyA.GetAABB();

			for (int j = i + 1; j < _bodyList.Count; j++)
			{
				Body2D bodyB = _bodyList[j];
				AABB bodyB_aabb = bodyB.GetAABB();

				if (bodyA.IsStatic && bodyB.IsStatic)
					continue;

				if (!Collision2D.IntersectAABBs(bodyA_aabb, bodyB_aabb))
					continue;

				_contactPairs.Add((i, j));
			}
		}
	}

	private void NarrowPhase()
	{
		for (int i = 0; i < _contactPairs.Count; i++)
		{
			(int, int) pair = _contactPairs[i];
			Body2D bodyA = _bodyList[pair.Item1];
			Body2D bodyB = _bodyList[pair.Item2];

			if (Collision2D.Collide(bodyA, bodyB, out Vector2D normal, out float depth))
			{
				SeparateBodies(bodyA, bodyB, normal * depth);
				Collision2D.FindContactPoints(bodyA, bodyB, out Vector2D contact1, out Vector2D contact2, out int contactCount);
				Manifold contact = new(bodyA, bodyB, normal, depth, contact1, contact2, contactCount);
				ResolveCollisionWithRotationAndFriction(in contact);
			}

		}
	}

	public void StepBodies(float time, int totalIterations)
	{
		for (int i = 0; i < this._bodyList.Count; i++)
			this._bodyList[i].Step(time, this._gravity, totalIterations);
	}

	private void SeparateBodies(Body2D bodyA, Body2D bodyB, Vector2D mtv)
	{
		if (bodyA.IsStatic)
			bodyB.Move(mtv);
		else if (bodyB.IsStatic)
			bodyA.Move(-mtv);
		else
		{
			bodyA.Move(-mtv / 2f);
			bodyB.Move(mtv / 2f);
		}
	}

	public void ResolveCollisionBasic(in Manifold contact)
	{
		Body2D bodyA = contact.bodyA;
		Body2D bodyB = contact.bodyB;
		Vector2D normal = contact.normal;
		float depth = contact.depth;

		Vector2D relativeVelocity = bodyB.linearVelocity - bodyA.linearVelocity;

		if (Math2D.Dot(relativeVelocity, normal) > 0f)
			return;

		float e = MathF.Min(bodyA.restitution, bodyB.restitution);

		float j = -(1f + e) * Math2D.Dot(relativeVelocity, normal);
		j /= bodyA.invMass + bodyB.invMass;

		Vector2D impulse = j * normal;

		bodyA.linearVelocity -= impulse * bodyA.invMass;
		bodyB.linearVelocity += impulse * bodyB.invMass;
	}

	public void ResolveCollisionWithRotation(in Manifold contact)
	{
		Body2D bodyA = contact.bodyA;
		Body2D bodyB = contact.bodyB;
		Vector2D normal = contact.normal;
		Vector2D contact1 = contact.contactA;
		Vector2D contact2 = contact.contactB;
		int contactCount = contact.contactCount;

		float e = MathF.Min(bodyA.restitution, bodyB.restitution);

		_contactList[0] = contact1;
		_contactList[1] = contact2;

		for (int i = 0; i < contactCount; i++)
		{
			_impulseList[i] = Vector2D.Zero;
			_raList[i] = Vector2D.Zero;
			_rbList[i] = Vector2D.Zero;
		}

		for (int i = 0; i < contactCount; i++)
		{
			Vector2D ra = _contactList[i] - bodyA.position;
			Vector2D rb = _contactList[i] - bodyB.position;

			_raList[i] = ra;
			_rbList[i] = rb;

			Vector2D raPerp = new(-ra.y, ra.x);
			Vector2D rbPerp = new(-rb.y, rb.x);

			Vector2D angularLinearVelocityA = raPerp * bodyA.angularVelocity;
			Vector2D angularLinearVelocityB = rbPerp * bodyB.angularVelocity;

			Vector2D relativeVelocity =
				bodyB.linearVelocity + angularLinearVelocityB -
				(bodyA.linearVelocity + angularLinearVelocityA);

			float contactVelocityMag = Math2D.Dot(relativeVelocity, normal);

			if (contactVelocityMag > 0f)
				continue;

			float raPerpDotN = Math2D.Dot(raPerp, normal);
			float rbPerpDotN = Math2D.Dot(rbPerp, normal);

			float denom = bodyA.invMass + bodyB.invMass +
				raPerpDotN * raPerpDotN * bodyA.invInertia +
				rbPerpDotN * rbPerpDotN * bodyB.invInertia;

			float j = -(1f + e) * contactVelocityMag;
			j /= denom;
			j /= contactCount;

			Vector2D impulse = j * normal;
			_impulseList[i] = impulse;
		}

		for (int i = 0; i < contactCount; i++)
		{
			Vector2D impulse = _impulseList[i];
			Vector2D ra = _raList[i];
			Vector2D rb = _rbList[i];

			bodyA.linearVelocity += -impulse * bodyA.invMass;
			bodyA.angularVelocity += -Math2D.Cross(ra, impulse) * bodyA.invInertia;
			bodyB.linearVelocity += impulse * bodyB.invMass;
			bodyB.angularVelocity += Math2D.Cross(rb, impulse) * bodyB.invInertia;
		}
	}

	public void ResolveCollisionWithRotationAndFriction(in Manifold contact)
	{
		Body2D bodyA = contact.bodyA;
		Body2D bodyB = contact.bodyB;
		Vector2D normal = contact.normal;
		Vector2D contact1 = contact.contactA;
		Vector2D contact2 = contact.contactB;
		int contactCount = contact.contactCount;

		float e = MathF.Min(bodyA.restitution, bodyB.restitution);

		float sf = (bodyA.staticFriction + bodyB.staticFriction) * 0.5f;
		float df = (bodyA.dynamicFriction + bodyB.dynamicFriction) * 0.5f;

		_contactList[0] = contact1;
		_contactList[1] = contact2;

		for (int i = 0; i < contactCount; i++)
		{
			_impulseList[i] = Vector2D.Zero;
			_raList[i] = Vector2D.Zero;
			_rbList[i] = Vector2D.Zero;
			_frictionImpulseList[i] = Vector2D.Zero;
			_jList[i] = 0f;
		}

		for (int i = 0; i < contactCount; i++)
		{
			Vector2D ra = _contactList[i] - bodyA.position;
			Vector2D rb = _contactList[i] - bodyB.position;

			_raList[i] = ra;
			_rbList[i] = rb;

			Vector2D raPerp = new(-ra.y, ra.x);
			Vector2D rbPerp = new(-rb.y, rb.x);

			Vector2D angularLinearVelocityA = raPerp * bodyA.angularVelocity;
			Vector2D angularLinearVelocityB = rbPerp * bodyB.angularVelocity;

			Vector2D relativeVelocity =
				bodyB.linearVelocity + angularLinearVelocityB -
				(bodyA.linearVelocity + angularLinearVelocityA);

			float contactVelocityMag = Math2D.Dot(relativeVelocity, normal);

			if (contactVelocityMag > 0f)
				continue;

			float raPerpDotN = Math2D.Dot(raPerp, normal);
			float rbPerpDotN = Math2D.Dot(rbPerp, normal);

			float denom = bodyA.invMass + bodyB.invMass +
				raPerpDotN * raPerpDotN * bodyA.invInertia +
				rbPerpDotN * rbPerpDotN * bodyB.invInertia;

			float j = -(1f + e) * contactVelocityMag;
			j /= denom;
			j /= contactCount;

			_jList[i] = j;

			Vector2D impulse = j * normal;
			_impulseList[i] = impulse;
		}

		for (int i = 0; i < contactCount; i++)
		{
			Vector2D impulse = _impulseList[i];
			Vector2D ra = _raList[i];
			Vector2D rb = _rbList[i];

			bodyA.linearVelocity += -impulse * bodyA.invMass;
			bodyA.angularVelocity += -Math2D.Cross(ra, impulse) * bodyA.invInertia;
			bodyB.linearVelocity += impulse * bodyB.invMass;
			bodyB.angularVelocity += Math2D.Cross(rb, impulse) * bodyB.invInertia;
		}

		for (int i = 0; i < contactCount; i++)
		{
			Vector2D ra = _contactList[i] - bodyA.position;
			Vector2D rb = _contactList[i] - bodyB.position;

			_raList[i] = ra;
			_rbList[i] = rb;

			Vector2D raPerp = new(-ra.y, ra.x);
			Vector2D rbPerp = new(-rb.y, rb.x);

			Vector2D angularLinearVelocityA = raPerp * bodyA.angularVelocity;
			Vector2D angularLinearVelocityB = rbPerp * bodyB.angularVelocity;

			Vector2D relativeVelocity =
				bodyB.linearVelocity + angularLinearVelocityB -
				(bodyA.linearVelocity + angularLinearVelocityA);

			Vector2D tangent = relativeVelocity - Math2D.Dot(relativeVelocity, normal) * normal;

			if (Math2D.NearlyEqual(tangent, Vector2D.Zero))
				continue;
			else
				tangent = Math2D.Normalize(tangent);

			float raPerpDotT = Math2D.Dot(raPerp, tangent);
			float rbPerpDotT = Math2D.Dot(rbPerp, tangent);

			float denom = bodyA.invMass + bodyB.invMass +
				raPerpDotT * raPerpDotT * bodyA.invInertia +
				rbPerpDotT * rbPerpDotT * bodyB.invInertia;

			float jt = -Math2D.Dot(relativeVelocity, tangent);
			jt /= denom;
			jt /= contactCount;

			Vector2D frictionImpulse;
			float j = _jList[i];

			if (MathF.Abs(jt) <= j * sf)
				frictionImpulse = jt * tangent;
			else
				frictionImpulse = -j * tangent * df;

			this._frictionImpulseList[i] = frictionImpulse;
		}

		for (int i = 0; i < contactCount; i++)
		{
			Vector2D frictionImpulse = this._frictionImpulseList[i];
			Vector2D ra = _raList[i];
			Vector2D rb = _rbList[i];

			bodyA.linearVelocity += -frictionImpulse * bodyA.invMass;
			bodyA.angularVelocity += -Math2D.Cross(ra, frictionImpulse) * bodyA.invInertia;
			bodyB.linearVelocity += frictionImpulse * bodyB.invMass;
			bodyB.angularVelocity += Math2D.Cross(rb, frictionImpulse) * bodyB.invInertia;
		}
	}
}