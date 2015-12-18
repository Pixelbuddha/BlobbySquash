using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PhysicsCollision : MonoBehaviour {

	public static void Collide(PhysicsObject A, PhysicsObject B) {
		bool aMoving = A.isMoving;
		bool bMoving = B.isMoving;
		if (!aMoving && !bMoving) { return; }

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
		float distanceCur = plane.GetDistanceToPoint(sphere.state.position);
		float distanceLast = plane.GetDistanceToPoint(sphere.state.lastPosition);

		//Check if there could be a collision - fast check
		if (distanceCur >= sphere.Radius && distanceLast >= sphere.Radius) {
			float dotCur = Vector3.Dot(plane.Normal, (plane.state.position - sphere.state.position).normalized);
			float dotLast = Vector3.Dot(plane.Normal, (plane.state.lastPosition - sphere.state.lastPosition).normalized);

			if ((dotCur > 0) == (dotLast > 0)) { return; }
		}

		//Now check step by step
		int steps = 10;
		Vector3 planePath = plane.state.position - plane.state.lastPosition;
		Vector3 spherePath = sphere.state.position - sphere.state.lastPosition;
		Vector3 planeLastPos = plane.state.lastPosition;
		Vector3 sphereLastPos = sphere.state.lastPosition;
		for (int i = 0; i <= steps; i++) {
			Vector3 planePos = plane.state.lastPosition + ((planePath / (float)steps) * i);
			Vector3 spherePos = sphere.state.lastPosition + ((spherePath / (float)steps) * i);
			if (plane.GetDistanceToPoint(planePos, spherePos) <= (sphere.Radius + 0.01)) {
				Vector3 newVelocity = Vector3.Reflect(sphere.state.velocity, plane.Normal);
				plane.state.position = planeLastPos;
				sphere.state.position = sphereLastPos;
				sphere.state.velocity = newVelocity;
				break;
			}
			planeLastPos = planePos;
			sphereLastPos = spherePos;
		}
	}

	private static void ApplyCollision(PhysicsSphere sphereA, PhysicsSphere sphereB) {
		float distance = GeometryHelper.DistanceSegmentSegment(sphereA.state.position, sphereA.state.lastPosition, sphereB.state.position, sphereB.state.lastPosition);
		if (distance >= sphereA.Radius + sphereB.Radius) { return; }

		Vector3 spherePathA = sphereA.state.position - sphereA.state.lastPosition;
		Vector3 spherePathB = sphereB.state.position - sphereB.state.lastPosition;

		Vector3 sphereALastPos = sphereA.state.lastPosition;
		Vector3 sphereBLastPos = sphereB.state.lastPosition;

		int steps = 10;
		for (int i = 0; i <= steps; i++) {

			Vector3 spherePosA = sphereA.state.lastPosition + ((spherePathA / (float)steps) * i);
			Vector3 spherePosB = sphereB.state.lastPosition + ((spherePathB / (float)steps) * i);

			if ((spherePosB - spherePosA).magnitude <= sphereA.Radius + sphereB.Radius) {

				float massA = 1, massB = 1;

				Vector3 collisionNormal = spherePosA - spherePosB;
				collisionNormal.Normalize();

				if (Vector3.Dot(collisionNormal, sphereA.state.velocity) < 0 || Vector3.Dot(collisionNormal, sphereB.state.velocity) > 0) {

					float tempFactorA = Vector3.Dot(sphereA.state.velocity, collisionNormal);
					float tempFactorB = Vector3.Dot(sphereB.state.velocity, collisionNormal);

					Vector3 AxisVelocityA = tempFactorA * collisionNormal;
					Vector3 AxisVelocityB = tempFactorB * collisionNormal;

					sphereA.state.velocity -= AxisVelocityA;
					sphereB.state.velocity -= AxisVelocityB;

					float tempFactor3 = massA + massB;
					float tempFactor4 = massA - massB;

					float tempFactor5 = (2.0f * massB * tempFactorB + tempFactorA * tempFactor4) / tempFactor3;
					float tempFactor6 = (2.0f * massA * tempFactorA - tempFactorB * tempFactor4) / tempFactor3;

					AxisVelocityA = tempFactor5 * collisionNormal;
					AxisVelocityB = tempFactor6 * collisionNormal;

					sphereA.state.velocity += AxisVelocityA;
					sphereB.state.velocity += AxisVelocityB;
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
