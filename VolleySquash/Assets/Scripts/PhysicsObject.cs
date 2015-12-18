using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhysicsObject : MonoBehaviour {
	//------------FIELDS------------
	private Vector3 _lastPosition;
	private Vector3 _force;
	private Vector3? _collisionVelocity = null;

	public bool movable = false;

	//------------PROPERTIES------------
	public Vector3 LastPosition {
		get { return _lastPosition; }
	}

	public bool Moving {
		get { return movable && (Velocity.sqrMagnitude > 0); }
	}

	public Vector3 Velocity {
		get { return transform.position - LastPosition; }
	}

	//------------CONSTRUCTOR------------
	protected virtual void Start() {
		PhysicsManager.Instance.AddObject(this);
		_lastPosition = transform.position;
	}

	//------------DESTRUCTOR------------
	protected virtual void OnDestroy() {
		//TODO: Fix me! - Eule
		//PhysicsManager.Instance.RemoveObject(this);
	}

	

	//------------PUBLIC METHOD------------

	public void AddForce(Vector3 force) {
		_force += force;
	}

	public virtual void Tick(float deltaTime) {
		Vector3 lastPos = transform.position;
		if (!movable) { return; }
		AddForce(PhysicsManager.gavity * Vector3.down * (deltaTime * deltaTime));
		AddForce(Velocity * 1);
		transform.position += _force;
		_force = Vector3.zero;
		Debug.Log("POSITIONS: " + transform.position.y + " " + lastPos.y);
		_lastPosition = lastPos;
	}

	public void SetCollisionVelocity(Vector3 newVelocity) {
		_collisionVelocity = newVelocity;
		//_lastPosition = transform.position - newVelocity;
		//Debug.Log(_lastPosition + " " + transform.position + " " + Velocity);
	}

	public void ApplyCollisionVelocity() {
		if (_collisionVelocity == null) { return; }
		_lastPosition = transform.position - _collisionVelocity.Value;
		_collisionVelocity = null;
	}

	//------------PRIVATE METHODS------------
	//TODO: Interpolate! - Eule
}
