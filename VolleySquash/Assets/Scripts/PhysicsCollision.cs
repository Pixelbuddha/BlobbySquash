using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PhysicsCollision : MonoBehaviour {

	public static void Collide(PhysicsObject A, PhysicsObject B) {
		bool aMoving = A.isMoving;
		bool bMoving = B.isMoving;
		if (!aMoving && !bMoving) { return; }

		if (A.GetType() == typeof(PhysicsSphere)) {
			if (B.GetType() == typeof(PhysicsSphere)) {
				ApplyCollision(A as PhysicsSphere, B as PhysicsSphere);
			}

			else if (B.GetType() == typeof(PhysicsPlane)) {
				ApplyCollision(B as PhysicsPlane, A as PhysicsSphere);
			}
		}
		else if (A.GetType() == typeof(PhysicsPlane)) {
			if (B.GetType() == typeof(PhysicsSphere)) {
				ApplyCollision(A as PhysicsPlane, B as PhysicsSphere);
			}
			else if (B.GetType() == typeof(PhysicsPlane)) {
				ApplyCollision(A as PhysicsPlane, B as PhysicsPlane);
			}
		}

		//Debug.Log(A.GetType() + " " + B.GetType());
	}

	private static void ApplyCollision(PhysicsPlane plane, PhysicsSphere sphere) {
		float distanceCur = plane.GetDistanceToPoint(sphere.position);
		float distanceLast = plane.GetDistanceToPoint(sphere.lastPosition);
		//Debug.Log(sphere.gameObject.name + " DistanceCur: " + distanceCur + " DistanceLast: " + distanceLast);
		//Check if there could be a collision - fast check
		if (distanceCur >= sphere.Radius && distanceLast >= sphere.Radius) {
			float dotCur = Vector3.Dot(plane.Normal, (plane.position - sphere.position).normalized);
			float dotLast = Vector3.Dot(plane.Normal, (plane.lastPosition - sphere.lastPosition).normalized);

			//Debug.Log(dotCur + " " + dotLast);
			if ((dotCur > 0) == (dotLast > 0)) { return; }
		}
		//Debug.Log("COLL: " + sphere.Position.y);
		//Now check step by step
		int steps = 10;
		Vector3 planePath = plane.position - plane.lastPosition;
		Vector3 spherePath = sphere.position - sphere.lastPosition;
		Vector3 planeLastPos = plane.lastPosition;
		Vector3 sphereLastPos = sphere.lastPosition;
		for (int i = 0; i <= steps; i++) {
			Vector3 planePos = plane.lastPosition + ((planePath / (float)steps) * i);
			Vector3 spherePos = sphere.lastPosition + ((spherePath / (float)steps) * i);
			//Debug.Log("CO: " + spherePos.y + " " + i);
			if (plane.GetDistanceToPoint(planePos, spherePos) <= (sphere.Radius + 0.01)) {
				Vector3 newVelocity = Vector3.Reflect(sphere.velocity, plane.Normal);
				plane.position = planeLastPos;
				sphere.position = sphereLastPos;
				sphere.velocity = newVelocity;
				//Debug.Log("COLLISION POS: " + sphere.Position.y);
				//sphere.SetVelocity(Vector3.zero);
				//Debug.Log("I DO COLLIDE! " + newVelocity);
				break;
			}
			planeLastPos = planePos;
			sphereLastPos = spherePos;
		}
	}

	private static void ApplyCollision(PhysicsSphere sphereA, PhysicsSphere sphereB) {
		float distance = GeometryHelper.DistanceSegmentSegment(sphereA.position, sphereA.lastPosition, sphereB.position, sphereB.lastPosition);
		if (distance >= sphereA.Radius + sphereB.Radius) { return; }

		Vector3 spherePathA = sphereA.position - sphereA.lastPosition;
		Vector3 spherePathB = sphereB.position - sphereB.lastPosition;

		Vector3 sphereALastPos = sphereA.lastPosition;
		Vector3 sphereBLastPos = sphereB.lastPosition;

		int steps = 10;
		for (int i = 0; i <= steps; i++) {

			Vector3 spherePosA = sphereA.lastPosition + ((spherePathA / (float)steps) * i);
			Vector3 spherePosB = sphereB.lastPosition + ((spherePathB / (float)steps) * i);

			if ((spherePosB - spherePosA).magnitude <= sphereA.Radius + sphereB.Radius) {

				float massA = 1, massB = 1;

				Vector3 collisionNormal = spherePosA - spherePosB;
				collisionNormal.Normalize();
				//Debug.Log(" normal: " + collisionNormal);

				if (Vector3.Dot(collisionNormal, sphereA.velocity) < 0 || Vector3.Dot(collisionNormal, sphereB.velocity) > 0) {

					float tempFactorA = Vector3.Dot(sphereA.velocity, collisionNormal);
					float tempFactorB = Vector3.Dot(sphereB.velocity, collisionNormal);

					Vector3 AxisVelocityA = tempFactorA * collisionNormal;
					Vector3 AxisVelocityB = tempFactorB * collisionNormal;

					sphereA.velocity -= AxisVelocityA;
					sphereB.velocity -= AxisVelocityB;

					float tempFactor3 = massA + massB;
					float tempFactor4 = massA - massB;

					float tempFactor5 = (2.0f * massB * tempFactorB + tempFactorA * tempFactor4) / tempFactor3;
					float tempFactor6 = (2.0f * massA * tempFactorA - tempFactorB * tempFactor4) / tempFactor3;

					AxisVelocityA = tempFactor5 * collisionNormal;
					AxisVelocityB = tempFactor6 * collisionNormal;

					sphereA.velocity += AxisVelocityA;
					sphereB.velocity += AxisVelocityB;

					//float speedA = sphereA.Velocity.magnitude;
					//float speedB = sphereB.Velocity.magnitude;

					//Vector3 newVelocityA = 2 * (massA * sphereA.Velocity + massB * sphereB.Velocity) / (massA + massB) - sphereA.Velocity;
					//Vector3 newVelocityB = 2 * (massA * sphereA.Velocity + massB * sphereB.Velocity) / (massA + massB) - sphereB.Velocity;

					//Vector3 newDirectionA = Vector3.Reflect(sphereA.Velocity, collisionNormal).normalized;
					//Vector3 newDirectionB = Vector3.Reflect(sphereB.Velocity, collisionNormal).normalized;

					//if (sphereA.Velocity.magnitude < 0.01) { newDirectionA = collisionNormal; }
					//if (sphereB.Velocity.magnitude < 0.01) { newDirectionB = -collisionNormal; }

					//Debug.Log("SphereA: oldVelo " + sphereA.Velocity + " newSpeed " + newVelocityA.magnitude + " newDir " + newDirectionA);
					//Debug.Log("SphereB: oldVelo " + sphereB.Velocity + " newSpeed " + newVelocityB.magnitude + " newDir " + newDirectionB);

					//sphereA.Position = sphereALastPos;
					//sphereB.Position = sphereBLastPos;
					//sphereA.SetCollisionVelocity(newDirectionA * newVelocityA.magnitude);
					//sphereB.SetCollisionVelocity(newDirectionB * newVelocityB.magnitude);

					//Debug.Log(sphereB._collisionVelocity);
					//Debug.LogError("");
					//The direction of the collision plane, perpendicular to the normal
					//var collisionDirection = new Vector3(-collisionNormal.y, collisionNormal.x, collisionNormal.z / 2);
					//collisionDirection = Vector3.Cross(collisionDirection, collisionNormal);
					//Debug.Log("Dir: " + collisionDirection);

					//var v1Parallel = Vector3.Dot(collisionNormal, sphereA.Velocity) * collisionNormal;
					//var v1Ortho = Vector3.Dot(collisionDirection, sphereA.Velocity) * collisionDirection;
					//var v2Parallel = Vector3.Dot(collisionNormal, sphereB.Velocity) * collisionNormal;
					//var v2Ortho = Vector3.Dot(collisionDirection, sphereB.Velocity) * collisionDirection;
					//Debug.Log("A: " + v1Parallel + " " + v1Ortho + " normal: " + collisionNormal);
					//Debug.Log("B: " + v2Parallel + " " + v2Ortho + " normal: " + collisionNormal);
					//Debug.Log("Pre: " + (v1Parallel + v1Ortho).magnitude + " " + (v2Parallel + v2Ortho).magnitude);
					//var v1Length = v1Parallel.magnitude;
					//var v2Length = v2Parallel.magnitude;
					//var commonVelocity = 2 * (massA * v1Length + massB * v2Length) / (massA + massB);
					//var v1LengthAfterCollision = commonVelocity - v1Length;
					//var v2LengthAfterCollision = commonVelocity - v2Length;

					//var totalMass = massA + massB;
					//var v1ParallelNew = (v1Parallel * (massA - massB) + 2 * massB * v2Parallel) / totalMass;
					//var v2ParallelNew = (v2Parallel * (massB - massA) + 2 * massA * v1Parallel) / totalMass;
					//v1Parallel = v1ParallelNew;
					//v2Parallel = v2ParallelNew;

					//sphereA.SetCollisionVelocity(v1Parallel + v1Ortho);
					//sphereB.SetCollisionVelocity(v2Parallel + v2Ortho);
					//Debug.Log("From: " + sphereA.Velocity.magnitude + " " + sphereB.Velocity.magnitude);
					//Debug.Log("To: " + (v1Parallel + v1Ortho).magnitude + " " + (v2Parallel + v2Ortho).magnitude);
					//Debug.LogError( sphereA.gameObject.name + " " + v1Parallel + v1Ortho + " B: " + v2Parallel + v2Ortho);
					break;
				}
				//Debug.Log("BALLS FLY APART! " + collisionNormal + "Dot A: " + Vector3.Dot(collisionNormal, sphereA.Velocity) + " Dot B: " + Vector3.Dot(collisionNormal, sphereB.Velocity));
			}

			sphereALastPos = spherePosA;
			sphereBLastPos = spherePosB;
        }
	}

	private static void ApplyCollision(PhysicsPlane planeA, PhysicsPlane planeB) {
		//Debug.LogError("Not yet implemented! Have fun scripting it! Muahahahahahahahaha - Eule");
	}
}
