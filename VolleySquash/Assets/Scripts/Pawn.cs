using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pawn : MonoBehaviour {
	//Controller and Collider components.
	private Controller controller;
	private List<SphereCollider> colliders;
	private SphereCollider lowerBody;
	private GameField gameField;

	//The current logical position (differs from drawn position!)
	private Vector3 curPosition;
	private Vector3 curDirection;
	private Vector3 lastPosition;

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
	private float curJumpSpeed;
	private bool grounded;

	//Time
	float LastReceiveDelta;


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
		//Set curPosition
		curPosition = this.transform.position;
	}

	private void Update() {

		//Interpolate position
		InterpolatePosition();

		LastReceiveDelta += Time.deltaTime;
    }

	private void FixedUpdate() {
		//While isMoving accelerate, else deccelerate
		if (isMoving) { Accelerate(); }
		else { Deccelerate(); }
		//Apply Movement
		if (!IsGrounded()) {
			ApplyGravity();
		}
		ApplyMovement();

		LastReceiveDelta = 0;
    }

	/// <summary>
	/// Sets isMoving to true;
	/// Sets curDirection.
	/// </summary>
	/// <param name="direction"> Direction the Pawn will move to. </param>
	public void Move(Vector3 direction) {
		isMoving = true;
		curDirection = direction;
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
	/// Adds the Pawns jumpForce to curJumpSpeed
	/// </summary>
	public void Jump() {
		if (!grounded) { return; }
		curJumpSpeed += jumpForce;
	}

	/// <summary>
	/// Applies gravity to curJumpSpeed, based on elapsed time
	/// </summary>
	private void ApplyGravity() {
		//if (IsGrounded()) { return; }
		curJumpSpeed -= gravity * Time.deltaTime;
	}

	/// <summary>
	/// Checks if the Pawn is touching a ground.
	/// </summary>
	/// <returns>Returns true if the Pawn touches a ground. </returns>
	public bool IsGrounded() {
		if (!lowerBody) { return true; }
		RaycastHit hit;
		if (Physics.Raycast(curPosition + lowerBody.transform.localPosition, Vector3.down, out hit, lowerBody.radius)) {
			if (!colliders.Contains(hit.collider as SphereCollider)) {
				if (hit.distance < lowerBody.radius + lowerBody.transform.localPosition.y) {
					curPosition.y = lowerBody.radius + Mathf.Abs(lowerBody.transform.localPosition.y);
				}
			}
			grounded = true;
			return true;
		}
		grounded = false;
		return false;
	}


	/// <summary>
	/// Adds curDirection * curMoveSpeed to curPosition.
	/// Adds curJumpSpeed to Y-Axis of curPosition.
	/// If curPosition is out of gameField bounds, constrain to bounds.	
	/// (Bounds.contains & Bounds.closestPoint)
	/// If Pawn isGrounded, set curJumpSpeed to 0.
	/// </summary>
	private void ApplyMovement() {
		curPosition += curDirection * curSpeed * Time.deltaTime;
		curPosition.y += curJumpSpeed * Time.deltaTime;
		if (IsGrounded()) { curJumpSpeed = 0; }
	}

	/// <summary>
	/// Interpolates (Lerp) between lastPosition and curPosition.
	/// Help: http://answers.unity3d.com/questions/875886/interpolation-of-players-position-over-rpc-calls.html
	/// </summary>
	private void InterpolatePosition() {
		//TODO: Lerp should change between 0 and 1, I think?!
		transform.position = Vector3.Lerp(lastPosition, curPosition, 1);
		Debug.Log(1 / Time.deltaTime * LastReceiveDelta);
	}



}
