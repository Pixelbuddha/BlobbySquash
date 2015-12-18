using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhysicsObject : MonoBehaviour {
	//------------FIELDS------------
	public Vector3 lastPosition;
	public Vector3 position;
	public Vector3 velocity;

	public bool movable = false;

	//------------PROPERTIES------------

	public bool isMoving {
		get { return movable && (velocity.sqrMagnitude > 0); }
	}

	//------------CONSTRUCTOR------------
	protected virtual void Start() {
		PhysicsManager.Instance.AddObject(this);
		lastPosition = transform.position;
		position = lastPosition;
	}

	//------------DESTRUCTOR------------
	protected virtual void OnDestroy() {
		//TODO: Fix me! - Eule
		//PhysicsManager.Instance.RemoveObject(this);
	}	

	//------------PUBLIC METHOD------------
	public void Update() {
		transform.position = position;
	}

	public void AddForce(Vector3 force) {
		velocity += force;
	}

	public virtual void Tick(float deltaTime) {
		Vector3 lastPos = position;
		if (!movable) { return; }
		AddForce(PhysicsManager.gavity * Vector3.down * (deltaTime * deltaTime));
		position += velocity;
		lastPosition = lastPos;
	}

	//------------PRIVATE METHODS------------
	//TODO: Interpolate! - Eule
}
