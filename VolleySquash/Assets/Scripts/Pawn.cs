using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pawn : MonoBehaviour {
	//Controller and Collider components.
	private Controller controller;
	private List<SphereCollider> colliders;
	private SphereCollider lowerBody;

	
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

	private float distanceToGround;
	private Rigidbody myRigidbody;


	// Use this for initialization
	public void Start() {
		//Get a controller
		controller = GetComponent<Controller>();
		//Get colliders and add to list
		colliders = new List<SphereCollider>();
		foreach (SphereCollider collider in GetComponentsInChildren<SphereCollider>()) {
			colliders.Add(collider);
			if (collider.name == "LowerBody") {
				lowerBody = collider;
			}
		}

		distanceToGround = lowerBody.bounds.extents.y;
		myRigidbody = GetComponent<Rigidbody>();
	}

	private void Update() {
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
		if (curSpeed >= maxSpeed) { return; }
		curSpeed += acceleration;
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
		myRigidbody.velocity += Vector3.up * jumpForce;
	}

	/// <summary>
	/// Checks if the Pawn is touching a ground.
	/// </summary>
	/// <returns>Returns true if the Pawn touches a ground. </returns>
	public bool IsGrounded() {
		return Physics.Raycast(lowerBody.transform.position, Vector3.down, distanceToGround + 0.001f);
	}


	/// <summary>
	/// Adds curDirection * curMoveSpeed to curPosition.
	/// Adds curJumpSpeed to Y-Axis of curPosition.
	/// If curPosition is out of gameField bounds, constrain to bounds.	
	/// (Bounds.contains & Bounds.closestPoint)
	/// If Pawn isGrounded, set curJumpSpeed to 0.
	/// </summary>
	private void ApplyMovement() {
		Vector3 direction = Vector3.zero;
		direction += moveDirection * curSpeed;

		Vector3 newVelocity = direction;
		newVelocity.y = myRigidbody.velocity.y;

		myRigidbody.velocity = newVelocity;
	}

	public float GetSpeed() {
		return myRigidbody.velocity.magnitude;
	}
}
