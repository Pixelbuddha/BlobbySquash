using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PhysicsCollision : MonoBehaviour {

	public static void Collide(PhysicsObject A, PhysicsObject B) {
		bool aMoving = A.isMoving;
		bool bMoving = B.isMoving;
		if (!aMoving && !bMoving) { return; }

		var colliderA = A.collider;
		var colliderB = B.collider;

		foreach (PhysicsCollider colA in colliderA) {
			foreach (PhysicsCollider colB in colliderB) {
				Collide(colA, colB);
			}
		}
	}

	private static void Collide(PhysicsCollider A, PhysicsCollider B) {
		//Optimze mit Enum!
		var AType = A.GetType();
		var BType = B.GetType();

		if (AType == typeof(PhysicsSphere)) {
			if (BType == typeof(PhysicsSphere)) {
				ApplyCollision(A as PhysicsSphere, B as PhysicsSphere);
			}

			else if (BType == typeof(PhysicsPlane)) {
				ApplyCollision(B as PhysicsPlane, A as PhysicsSphere);
			}
		}
		else if (AType == typeof(PhysicsPlane)) {
			if (BType == typeof(PhysicsSphere)) {
				ApplyCollision(A as PhysicsPlane, B as PhysicsSphere);
			}
			else if (BType == typeof(PhysicsPlane)) {
				//ApplyCollision(A as PhysicsPlane, B as PhysicsPlane);
			}
		}
	}

	private static void ApplyCollision(PhysicsPlane plane, PhysicsSphere sphere) {
		float distanceCur = plane.GetDistanceToPoint(sphere.physicsObject.state.position);
		float distanceLast = plane.GetDistanceToPoint(sphere.physicsObject.state.lastPosition);

		//Check if there could be a collision - fast check
		if (distanceCur >= sphere.Radius && distanceLast >= sphere.Radius) {
			float dotCur = Vector3.Dot(plane.Normal, (plane.physicsObject.state.position - sphere.physicsObject.state.position).normalized);
			float dotLast = Vector3.Dot(plane.Normal, (plane.physicsObject.state.lastPosition - sphere.physicsObject.state.lastPosition).normalized);

			if ((dotCur > 0) == (dotLast > 0)) { return; }
		}

		//Now check step by step
		int steps = 10;
		Vector3 planePath = plane.physicsObject.state.position - plane.physicsObject.state.lastPosition;
		Vector3 spherePath = sphere.physicsObject.state.position - sphere.physicsObject.state.lastPosition;
		Vector3 planeLastPos = plane.physicsObject.state.lastPosition;
		Vector3 sphereLastPos = sphere.physicsObject.state.lastPosition;
		for (int i = 0; i <= steps; i++) {
			Vector3 planePos = plane.physicsObject.state.lastPosition + ((planePath / (float)steps) * i);
			Vector3 spherePos = sphere.physicsObject.state.lastPosition + ((spherePath / (float)steps) * i);
			if (plane.GetDistanceToPoint(planePos, spherePos) <= (sphere.Radius + 0.01)) {
				Vector3 newVelocity = Vector3.Reflect(sphere.physicsObject.state.velocity, plane.Normal);
				plane.physicsObject.state.position = planeLastPos;
				sphere.physicsObject.state.position = sphereLastPos;
				sphere.physicsObject.state.velocity = newVelocity;
				break;
			}
			planeLastPos = planePos;
			sphereLastPos = spherePos;
		}
	}

	private static void ApplyCollision(PhysicsSphere sphereA, PhysicsSphere sphereB) {
		float distance = GeometryHelper.DistanceSegmentSegment(sphereA.physicsObject.state.position, sphereA.physicsObject.state.lastPosition, sphereB.physicsObject.state.position, sphereB.physicsObject.state.lastPosition);
		if (distance >= sphereA.Radius + sphereB.Radius) { return; }

		Vector3 spherePathA = sphereA.physicsObject.state.position - sphereA.physicsObject.state.lastPosition;
		Vector3 spherePathB = sphereB.physicsObject.state.position - sphereB.physicsObject.state.lastPosition;

		Vector3 sphereALastPos = sphereA.physicsObject.state.lastPosition;
		Vector3 sphereBLastPos = sphereB.physicsObject.state.lastPosition;

		int steps = 10;
		for (int i = 0; i <= steps; i++) {

			Vector3 spherePosA = sphereA.physicsObject.state.lastPosition + ((spherePathA / (float)steps) * i);
			Vector3 spherePosB = sphereB.physicsObject.state.lastPosition + ((spherePathB / (float)steps) * i);

			if ((spherePosB - spherePosA).magnitude <= sphereA.Radius + sphereB.Radius) {

				float massA = 1, massB = 1;

				Vector3 collisionNormal = spherePosA - spherePosB;
				collisionNormal.Normalize();

				if (Vector3.Dot(collisionNormal, sphereA.physicsObject.state.velocity) < 0 || Vector3.Dot(collisionNormal, sphereB.physicsObject.state.velocity) > 0) {

					float tempFactorA = Vector3.Dot(sphereA.physicsObject.state.velocity, collisionNormal);
					float tempFactorB = Vector3.Dot(sphereB.physicsObject.state.velocity, collisionNormal);

					Vector3 AxisVelocityA = tempFactorA * collisionNormal;
					Vector3 AxisVelocityB = tempFactorB * collisionNormal;

					sphereA.physicsObject.state.velocity -= AxisVelocityA;
					sphereB.physicsObject.state.velocity -= AxisVelocityB;

					float tempFactor3 = massA + massB;
					float tempFactor4 = massA - massB;

					float tempFactor5 = (2.0f * massB * tempFactorB + tempFactorA * tempFactor4) / tempFactor3;
					float tempFactor6 = (2.0f * massA * tempFactorA - tempFactorB * tempFactor4) / tempFactor3;

					AxisVelocityA = tempFactor5 * collisionNormal;
					AxisVelocityB = tempFactor6 * collisionNormal;

					sphereA.physicsObject.state.velocity += AxisVelocityA;
					sphereB.physicsObject.state.velocity += AxisVelocityB;
					break;
				}
			}

			sphereALastPos = spherePosA;
			sphereBLastPos = spherePosB;
        }
	}

	private static void ApplyCollision(PhysicsPlane planeA, PhysicsPlane planeB) {
		//Debug.LogError("Not yet implemented! Have fun scripting it! Muahahahahahahahaha - Eule");
	}
}
