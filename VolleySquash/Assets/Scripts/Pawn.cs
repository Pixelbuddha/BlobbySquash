using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pawn : MonoBehaviour {
	//Controller and Collider components.
	private Controller controller;
	private List<SphereCollider> colliders;
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
	private float gravity;
	private float jumpForce;
	private float curJumpSpeed;
	
	// Use this for initialization
	public void Start () {
		//Get a controller
		controller = GetComponent<Controller>();
		//Get colliders and add to list
		foreach (Collider collider in GetComponents<Collider>()) {
			colliders.Add((SphereCollider)collider);
		}
		//Set curPosition
		curPosition = this.transform.position;
	}
	
	private void Update () {
		Debug.Log(curPosition + " vs " + transform.position);


		//Interpolate position
		InterpolatePosition();
	}

	private void FixedUpdate() {
		//While isMoving accelerate, else deccelerate
		if (isMoving) { Accelerate(); }
		else { Deccelerate(); }
		//Apply Movement
		ApplyGravity();
		ApplyMovement();
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
	}

	/// <summary>
	/// Adds the Pawns jumpForce to curJumpSpeed
	/// </summary>
	public void Jump() {
		curJumpSpeed += jumpForce;
	}

	/// <summary>
	/// Applies gravity to curJumpSpeed, based on elapsed time
	/// </summary>
	private void ApplyGravity() {
		curJumpSpeed -= gravity * Time.deltaTime;
	}

	/// <summary>
	/// Checks if the Pawn is touching a ground.
	/// </summary>
	/// <returns>Returns true if the Pawn touches a ground. </returns>
	public bool IsGrounded() {
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
	}



}
