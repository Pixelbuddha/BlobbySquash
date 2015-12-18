using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PhysicsCollision : MonoBehaviour {

	public static void Collide(PhysicsObject A, PhysicsObject B) {
		bool aMoving = A.Moving;
		bool bMoving = B.Moving;
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
		float distanceCur = plane.GetDistanceToPoint(sphere.transform.position);
		float distanceLast = plane.GetDistanceToPoint(sphere.LastPosition);
		Debug.Log(sphere.gameObject.name + " DistanceCur: " + distanceCur + " DistanceLast: " + distanceLast);
		//Check if there could be a collision - fast check
		if (distanceCur >= sphere.Radius && distanceLast >= sphere.Radius) {
			float dotCur = Vector3.Dot(plane.Normal, (plane.transform.position - sphere.transform.position).normalized);
			float dotLast = Vector3.Dot(plane.Normal, (plane.LastPosition - sphere.LastPosition).normalized);

			//Debug.Log(dotCur + " " + dotLast);
			if ((dotCur > 0) == (dotLast > 0)) { return; }
		}
		//Debug.Log("COLL: " + sphere.transform.position.y);
		//Now check step by step
		int steps = 10;
		Vector3 planePath = plane.transform.position - plane.LastPosition;
		Vector3 spherePath = sphere.transform.position - sphere.LastPosition;
		Vector3 planeLastPos = plane.LastPosition;
		Vector3 sphereLastPos = sphere.LastPosition;
		for (int i = 0; i <= steps; i++) {
			Vector3 planePos = plane.LastPosition + ((planePath / (float)steps) * i);
			Vector3 spherePos = sphere.LastPosition + ((spherePath / (float)steps) * i);
			//Debug.Log("CO: " + spherePos.y + " " + i);
			if (plane.GetDistanceToPoint(planePos, spherePos) <= (sphere.Radius+0.01)) {
				Vector3 newVelocity = Vector3.Reflect(sphere.Velocity, plane.Normal);
				plane.transform.position = planeLastPos;
				sphere.transform.position = sphereLastPos;
				sphere.SetVelocity(newVelocity);
				//Debug.Log("COLLISION POS: " + sphere.transform.position.y);
				//sphere.SetVelocity(Vector3.zero);
				//Debug.Log("I DO COLLIDE! " + newVelocity);
				break;
			}
			planeLastPos = planePos;
			sphereLastPos = spherePos;
		}
	}

	private static void ApplyCollision(PhysicsSphere sphereA, PhysicsSphere sphereB) {

	}

	private static void ApplyCollision(PhysicsPlane planeA, PhysicsPlane planeB) {
		//Debug.LogError("Not yet implemented! Have fun scripting it! Muahahahahahahahaha - Eule");
	}


}
