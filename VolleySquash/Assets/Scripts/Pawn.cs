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
	private float acceleration, decceleration;
	private float curSpeed;
	private float maxSpeed;
	private bool isMoving;

	//Jump variables
	private float gravity;
	private float jumpForce;
	private float curJumpSpeed;

	//Transform variables
	private Vector3 velocity;
	
	// Use this for initialization
	public void Start () {
		//Get a controller
		//Get colliders and add to list
		//Set curPosition
	}
	
	private void Update () {

		//Interpolate position
	}

	private void FixedUpdate() {
		//While isMoving accelerate, else deccelerate

		//Apply Movement
	}

	/// <summary>
	/// Sets isMoving to true;
	/// Sets curDirection.
	/// </summary>
	/// <param name="direction"> Direction the Pawn will move to. </param>
	public void Move(Vector3 direction) {

	}

	/// <summary>
	/// Sets isMoving to false;
	/// </summary>
	public void Stop() {

	}

	//Adds acceleration to curMoveSpeed unless maxSpeed = moveSpeed
	private void Accelerate() {

	}

	//Substracts decceleration to curMoveSpeed unless maxSpeed = 0
	private void Deccelerate() {

	}

	/// <summary>
	/// Adds the Pawns jumpForce to curJumpSpeed
	/// </summary>
	public void Jump() {

	}

	/// <summary>
	/// Applies gravity to curJumpSpeed, based on elapsed time
	/// </summary>
	private void ApplyGravity() {

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
	}

	/// <summary>
	/// Interpolates (Lerp) between lastPosition and curPosition.
	/// Help: http://answers.unity3d.com/questions/875886/interpolation-of-players-position-over-rpc-calls.html
	/// </summary>
	private void InterpolatePosition() {

	}



}
