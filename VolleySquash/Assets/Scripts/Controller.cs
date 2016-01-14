using UnityEngine;
using System.Collections;

public abstract class Controller : MonoBehaviour {

	//Pawn that is controlled
	protected Pawn pawn;

	//Color to identify player
	public Color color;


	// Use this for initialization
	protected virtual void Start () {
		//Get Pawn Component
		pawn = GetComponent<Pawn>();
	}

	/// <summary>
	/// Calls Jump() of controlled pawn;
	/// </summary>
	protected void Jump() {
		pawn.Jump();
	}

	/// <summary>
	/// Calls Move() of controlled pawn;
	/// </summary>
	/// <param name="direction">Direction the pawn shall move to</param>
	protected void Move(Vector3 direction) {
		pawn.Move(direction);
	}
}
