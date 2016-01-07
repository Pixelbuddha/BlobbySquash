using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pawn : PhysicsObject {
	//Controller and Collider components.
	private Controller controller;

	private Vector3 moveDirection;
	//Move variables
	[SerializeField]
	private float acceleration, decceleration;
	private float curSpeed;
	[SerializeField]
	private float maxSpeed;
	private bool isMoving;

	//Jump variables
	[SerializeField]
	private float gravity;
	[SerializeField]
	private float jumpForce;

	public float airControl;
	private float distanceToGround;

	//private Vector3 _lastVelocity;

	public float _dynamicFriction = 0.8f;
	public float _airFriction = 0.95f;

	public float groundPosition;



	// Use this for initialization
	public void Start() {
		base.Start();
		//Get a controller
		controller = GetComponent<Controller>();
		OnCollision += OnCollide;

		var lowerBody = transform.FindChild("LowerBody");
		groundPosition = lowerBody.lossyScale.y / 2 - lowerBody.localPosition.y;
	}

	public override void Update() {
		base.Update();

		if (isMoving) { Accelerate(); }
		else { Deccelerate(); }
		ApplyMovement();
	}

	/// <summary>
	/// Sets isMoving to true;
	/// Sets curDirection.
	/// </summary>
	/// <param name="direction"> Direction the Pawn will move to. </param>
	public void Move(Vector3 direction) {
		isMoving = true;
		moveDirection = direction;
	}

	/// <summary>
	/// Sets isMoving to false;
	/// </summary>
	public void Stop() {
		isMoving = false;
	}

	//Adds acceleration to curMoveSpeed unless maxSpeed = moveSpeed
	private void Accelerate() {
		//if (curSpeed >= maxSpeed) { return; }
		//curSpeed += acceleration;
		Vector3 curMoveSpeed = state.velocity;
		curMoveSpeed.y = 0;
		if (curMoveSpeed.magnitude * 10 >= maxSpeed) { return; }
		if (!_isGrounded) { AddForce(moveDirection * Time.deltaTime * acceleration * airControl); return; }
		AddForce(moveDirection * Time.deltaTime * acceleration);
	}

	//Substracts decceleration to curMoveSpeed unless maxSpeed = 0
	private void Deccelerate() {
		if (curSpeed <= 0) { return; }
		curSpeed -= decceleration;
		if (curSpeed < 0) {
			curSpeed = 0;
		}
	}

	/// <summary>
	/// Jump!
	/// </summary>
	public void Jump() {
		if (!IsGrounded()) { return; }
		AddForce(Vector3.up * jumpForce);
		//myRigidbody.velocity += Vector3.up * jumpForce;
	}

	/// <summary>
	/// Checks if the Pawn is touching a ground.
	/// </summary>
	/// <returns>Returns true if the Pawn touches a ground. </returns>
	public bool IsGrounded() {
		//return Physics.Raycast(lowerBody.transform.position, Vector3.down, distanceToGround + 0.001f);
		return state.position.y == groundPosition;
	}


	/// <summary>
	/// Adds curDirection * curMoveSpeed to curPosition.
	/// Adds curJumpSpeed to Y-Axis of curPosition.
	/// If curPosition is out of gameField bounds, constrain to bounds.	
	/// (Bounds.contains & Bounds.closestPoint)
	/// If Pawn isGrounded, set curJumpSpeed to 0.
	/// </summary>
	private void ApplyMovement() {
		//Vector3 direction = Vector3.zero;
		//direction += moveDirection * curSpeed * Time.deltaTime;

		//Vector3 newVelocity = direction;
		////newVelocity.y = state.velocity.y;

		//state.velocity -= _lastVelocity;
		//state.velocity += newVelocity;
		//_lastVelocity = newVelocity;

		//Debug.Log(state.velocity);
	}

	public float GetSpeed() {
		//return myRigidbody.velocity.magnitude;
		return 0;
	}

	private void OnCollide(PhysicsCollider collider) {
		if (collider.physicsObject.name == "Ground") {
			state.velocity.y *= 0.0f;
			state.position.y = groundPosition;
			state.lastPosition.y = groundPosition;
		}

		if (collider.physicsObject.name == "Ball") {
			collider.physicsObject.state.velocity = Vector3.Lerp(collider.physicsObject.state.velocity, Vector3.forward * 0.7f + Vector3.up * 0.25f , 0.5f);
			//collider.physicsObject.state.velocity += Vector3.forward * 0.7f;
		}
    }

	public override void Tick(float deltaTime) {
		_isGrounded = IsGrounded();
		if (_isGrounded) {
			state.velocity.x *= _dynamicFriction;
			state.velocity.z *= _dynamicFriction;
		}
		else {
			state.velocity.x *= _airFriction;
			state.velocity.z *= _airFriction;
			AddForce(PhysicsManager.gavity * Vector3.down * (deltaTime * deltaTime) * 5);

		}
		base.Tick(deltaTime);
	}
}
