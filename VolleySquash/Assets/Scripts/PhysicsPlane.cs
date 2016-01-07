using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Plane))]
public class PhysicsPlane : PhysicsCollider {

	//------------FIELDS------------
	public Vector3 Normal; 

	//------------CONSTRUCTOR---------
	protected override void Start() {
		base.Start();
		Normal =  transform.rotation * transform.GetComponent<MeshFilter>().mesh.normals[0];
	}

	//------------PUBLIC METHODS------------
	public float GetDistanceToPoint(Vector3 point) {
		return GetDistanceToPoint(transform.position, point);
	}

	public float GetDistanceToPoint(Vector3 planePos, Vector3 point) {
		return Mathf.Abs(Vector3.Dot((point - planePos), Normal));
	}
}
