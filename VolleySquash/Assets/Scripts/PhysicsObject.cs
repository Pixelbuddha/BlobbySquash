using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class PhysicsObject : MonoBehaviour {

	public struct State {
		public Vector3 lastPosition;
		public Vector3 position;
		public Vector3 velocity;
	}

	//------------FIELDS------------
	public State state;
	public State frozenState;

	public bool movable = false;

	//------------PROPERTIES------------

	public bool isMoving {
		get { return movable && (state.velocity.sqrMagnitude > 0); }
	}

	//------------CONSTRUCTOR------------
	protected virtual void Start() {
		PhysicsManager.Instance.AddObject(this);
		state.lastPosition = transform.position;
		state.position = state.lastPosition;
	}

	//------------DESTRUCTOR------------
	protected virtual void OnDestroy() {
		//TODO: Fix me! - Eule
		//PhysicsManager.Instance.RemoveObject(this);
	}

	//------------PUBLIC METHOD------------
	public void Update() {
		transform.position = state.position;
	}

	public void AddForce(Vector3 force) {
		state.velocity += force;
	}

	public virtual void Tick(float deltaTime) {
		Vector3 lastPos = state.position;
		if (!movable) { return; }
		AddForce(PhysicsManager.gavity * Vector3.down * (deltaTime * deltaTime));
		state.position += state.velocity;
		state.lastPosition = lastPos;
	}

	public void Freeze() {
		frozenState = state;
	}

	public void Unfreeze() {
		state = frozenState;
	}

	//------------PRIVATE METHODS------------
	//TODO: Interpolate! - Eule
}
