using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Plane))]
public class PlanePhysics : PhysicsObject {

	//------------PROPERTIES------------
	public Vector3 Normal {
		get { return GetComponent<Plane>().normal; }
	}

	//------------CONSTRUCTOR---------
	protected override void Start () {
		base.Start();
	}

	//------------PUBLIC METHODS------------
	public float GetDistanceToPoint(Vector3 point) {
		return Vector3.Dot((point - transform.position), Normal);
	}
}
