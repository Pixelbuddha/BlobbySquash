using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



public class PhysicsObject : MonoBehaviour {

	public struct State {
		public Vector3 lastPosition;
		public Vector3 position;
		public Vector3 velocity;
	}

	//------------FIELDS------------
	public State state;
	public State frozenState;
	public List<PhysicsCollider> colliders;

	public delegate void CollisionEvent(PhysicsCollider collider);
	public event CollisionEvent OnCollision;

	public bool movable = false;
	public bool applyGravity = true;

	public bool _isGrounded = false;

	public Vector3 startPos;


	//------------PROPERTIES------------

	public bool isMoving {
		get { return movable && (state.velocity.sqrMagnitude > 0); }
	}

	//------------CONSTRUCTOR------------
	protected virtual void Awake() {
		startPos = this.transform.position;
	}

	protected virtual void Start() {
		PhysicsManager.Instance.AddObject(this);
		state.lastPosition = transform.position;
		state.position = state.lastPosition;
		colliders = GetComponentsInChildren<PhysicsCollider>().ToList();
		//		if (GetComponent<PhysicsCollider>() != null) {
		//			collider.Add(GetComponent<PhysicsCollider>());
		//		}
		foreach (PhysicsCollider col in colliders) {
			col.physicsObject = this;
		}
	}

	//------------DESTRUCTOR------------
	protected virtual void OnDestroy() {
		//TODO: Fix me! - Eule
		//PhysicsManager.Instance.RemoveObject(this);
	}

	//------------PUBLIC METHOD------------
	public virtual void Update() {
		transform.position = state.position;
	}

	public void AddForce(Vector3 force) {
		state.velocity += force;
	}

	public virtual void Tick(float deltaTime) {
		Vector3 lastPos = state.position;
		if (!movable) { return; }
		if (!_isGrounded && applyGravity) {
			AddForce(PhysicsManager.gavity * Vector3.down * (deltaTime * deltaTime));
		}
		state.position += state.velocity;
		state.lastPosition = lastPos;
	//Debug.Log(this.name + " Velocity: " + state.velocity + " Position: " + state.position+" g:"+ _isGrounded);
	}

	public void Freeze() {
		frozenState = state;
	}

	public void Unfreeze() {
		state = frozenState;
	}

	public void CallOnCollide(PhysicsCollider collider) {
		if (OnCollision != null) {
			//Debug.Log("Call it!");
			OnCollision(collider);
		}
	}

	public void SetPosition(Vector3 pos) {
		transform.position = pos;
		state.lastPosition = pos;
		state.position = pos;
		state.velocity = Vector3.zero;
	}

	//------------PRIVATE METHODS------------
	//TODO: Interpolate! - Eule
}
