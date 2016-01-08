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
				if (Collide(colA, colB)) {
					A.CallOnCollide(colB);
					B.CallOnCollide(colA);
					return;
				}
			}
		}
	}

	private static bool Collide(PhysicsCollider A, PhysicsCollider B) {
		//Optimze mit Enum!
		var AType = A.GetType();
		var BType = B.GetType();

		if (AType == typeof(PhysicsSphere)) {
			if (BType == typeof(PhysicsSphere)) {
				if (!A.collideWithSphere) { return false; }
				if (!B.collideWithSphere) { return false; }
				return ApplyCollision(A as PhysicsSphere, B as PhysicsSphere);
			}

			else if (BType == typeof(PhysicsPlane)) {
				if (!A.collideWithPlane) { return false; }
				if (!B.collideWithSphere) { return false; }
				return ApplyCollision(B as PhysicsPlane, A as PhysicsSphere);
			}
		}
		else if (AType == typeof(PhysicsPlane)) {
			if (BType == typeof(PhysicsSphere)) {
				if (!A.collideWithSphere) { return false; }
				if (!B.collideWithPlane) { return false; }
				return ApplyCollision(A as PhysicsPlane, B as PhysicsSphere);
			}
			else if (BType == typeof(PhysicsPlane)) {
				//ApplyCollision(A as PhysicsPlane, B as PhysicsPlane);
			}
		}
		return false;
	}

	private static bool ApplyCollision(PhysicsPlane plane, PhysicsSphere sphere) {
		Vector3 planeOffset = plane.GetPositionOffset();
		Vector3 planePos = plane.physicsObject.state.position + planeOffset;
		Vector3 planeLastPos = plane.physicsObject.state.lastPosition + planeOffset;

		Vector3 sphereOffset = sphere.GetPositionOffset();
		Vector3 spherePos = sphere.physicsObject.state.position + sphereOffset;
		Vector3 sphereLastPos = sphere.physicsObject.state.lastPosition + sphereOffset;
		//Debug.LogError("Object Pos: " + sphere.physicsObject.state.position + " Offset Pos: " + spherePos);

		float distanceCur = plane.GetDistanceToPoint(spherePos);
		float distanceLast = plane.GetDistanceToPoint(sphereLastPos);

		//Check if there could be a collision - fast check
		if (distanceCur >= sphere.Radius && distanceLast >= sphere.Radius) {
			float dotCur = Vector3.Dot(plane.Normal, (planePos - spherePos).normalized);
			float dotLast = Vector3.Dot(plane.Normal, (planeLastPos - sphereLastPos).normalized);

			if ((dotCur > 0) == (dotLast > 0)) { return false; }
		}

		//Now check step by step
		int steps = 10;
		Vector3 planePath = planePos - planeLastPos;
		Vector3 spherePath = spherePos - sphereLastPos;
		for (int i = 0; i <= steps; i++) {
			Vector3 planeStepPos = planeLastPos + ((planePath / (float)steps) * i);
			Vector3 sphereStepPos = sphereLastPos + ((spherePath / (float)steps) * i);
			//Debug.Log("SpherePos: " + spherePos + "SpereLastPos: " + sphereLastPos);
			if (plane.GetDistanceToPoint(planeStepPos, sphereStepPos) <= (sphere.Radius + 0.01)) {
				Vector3 newVelocity = Vector3.Reflect(sphere.physicsObject.state.velocity, plane.Normal);
				newVelocity *= sphere.GetCombinedBounciness(plane);
				plane.physicsObject.state.position = planeLastPos - planeOffset;
				sphere.physicsObject.state.position = sphereLastPos - sphereOffset;
				sphere.physicsObject.state.velocity = newVelocity;
				//Debug.LogError("new Velo: " + newVelocity + " Plane: " + plane.name + " Sphere: " + sphere.name);
				return true;
			}
			planeLastPos = planeStepPos;
			sphereLastPos = sphereStepPos;
		}
		return false;
	}

	private static bool ApplyCollision(PhysicsSphere sphereA, PhysicsSphere sphereB) {
		Vector3 sphereAOffset = sphereA.GetPositionOffset();
		Vector3 sphereAPos = sphereA.physicsObject.state.position + sphereAOffset;
		Vector3 sphereALastPos = sphereA.physicsObject.state.lastPosition + sphereAOffset;

		Vector3 sphereBOffset = sphereB.GetPositionOffset();
		Vector3 sphereBPos = sphereB.physicsObject.state.position + sphereBOffset;
		Vector3 sphereBLastPos = sphereB.physicsObject.state.lastPosition + sphereBOffset;

		float distance = GeometryHelper.DistanceSegmentSegment(sphereAPos, sphereALastPos, sphereBPos, sphereBLastPos);
		if (distance >= sphereA.Radius + sphereB.Radius) { return false; }

		Vector3 spherePathA = sphereAPos - sphereALastPos;
		Vector3 spherePathB = sphereBPos - sphereBLastPos;
		

		int steps = 10;
		for (int i = 0; i <= steps; i++) {

			Vector3 spherePosA = sphereALastPos + ((spherePathA / (float)steps) * i);
			Vector3 spherePosB = sphereBLastPos + ((spherePathB / (float)steps) * i);

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
					AxisVelocityA *= sphereA.GetCombinedBounciness(sphereB);
					AxisVelocityB *= sphereB.GetCombinedBounciness(sphereA);

					sphereA.physicsObject.state.velocity += AxisVelocityA;
					sphereB.physicsObject.state.velocity += AxisVelocityB;

					sphereA.physicsObject.state.position = sphereALastPos - sphereAOffset;
					sphereB.physicsObject.state.position = sphereBLastPos - sphereBOffset;
					return true;
				}
			}

			sphereALastPos = spherePosA;
			sphereBLastPos = spherePosB;
        }
		return false;
	}

	private static bool ApplyCollision(PhysicsPlane planeA, PhysicsPlane planeB) {
		//Debug.LogError("Not yet implemented! Have fun scripting it! Muahahahahahahahaha - Eule");
		return false;
	}
}
