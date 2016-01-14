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
		if (Input.GetButtonDown("Jump")) {
			pawn.Jump();
		}

		//Get Movement
		Vector3 newDirection = Vector3.zero;
		
		newDirection += Vector3.forward * Input.GetAxis("Vertical");
		newDirection += Vector3.right * Input.GetAxis("Horizontal");

		if (newDirection == Vector3.zero) { pawn.Stop(); }

		else { Move(Vector3.Normalize(newDirection)); }
	}
}
