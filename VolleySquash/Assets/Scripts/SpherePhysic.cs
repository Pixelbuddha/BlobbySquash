using UnityEngine;
using System.Collections;

public class SpherePhysic : PhysicsObject {

	//------------FIELDS------------
	private float _radius;

	//------------PROPERTIES---------
	public float Radius {
		get { return _radius; }
	}

	//------------CONSTRUCTOR---------
	protected override void Start () {
		base.Start();
		_radius = transform.lossyScale.x * 0.5f;
	}
}
