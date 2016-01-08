using UnityEngine;
using System.Collections;

public class HumanPlayer : Controller {

	Controls controls;

	protected override void Start() {
		base.Start();
		controls = new Controls();
	}

	private void Update() {
		//Get Input
		if (pawn != null) {
			GetInput();
		}
	}

	/// <summary>
	/// Foreach Ingame Control in Controls, listen to Input an call according Method
	/// </summary>
	private void GetInput() {
		if (Input.GetKeyDown(controls.Jump)) {
			pawn.Jump();
		}

		//Get Movement
		Vector3 newDirection = Vector3.zero;

		if (Input.GetKey(controls.Forward)) {
			newDirection += Vector3.forward;
		}

		if (Input.GetKey(controls.Backward)) {
			newDirection += Vector3.back;
		}

		if (Input.GetKey(controls.Left)) {
			newDirection += Vector3.left;
		}

		if (Input.GetKey(controls.Right)) {
			newDirection += Vector3.right;
		}

		if (newDirection == Vector3.zero) { pawn.Stop(); }

		else { Move(Vector3.Normalize(newDirection)); }
	}
}
