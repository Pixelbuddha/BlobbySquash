using UnityEngine;
using System.Collections;

public class PhysicsSphere : PhysicsObject {

	//------------FIELDS------------
	private float _radius;

	//------------PROPERTIES---------
	public float Radius {
		get { return _radius; }
	}

	//------------CONSTRUCTOR---------
	protected override void Start() {
		base.Start();
		_radius = transform.lossyScale.x * 0.5f;
		movable = true;
	}

	//------------PUBLIC METHODS------------
	public override void Tick(float deltaTime) {
		base.Tick(deltaTime);
	}
}
