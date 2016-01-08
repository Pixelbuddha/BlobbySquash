using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIPlayer : Controller {

	private Vector3 targetPosition;
	public Transform targetPos;
	public List<Vector4> ballPath;

	public float minY = 1.5f, maxY = 5.5f;

	public int ballCollisions;

	// Use this for initialization
	protected override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	public void Update () {
		if (Game.instance.activePlayerA) {
			ballPath = null;
			Vector3 direction = (targetPos.transform.position - this.transform.position).normalized;
			pawn.Move(direction);
		}

		else {
			if (ballPath == null) { CalcPlayPosition(); }
		}

	}

	/// <summary>
	/// TODO LATER WHEN BALL IS DONE.
	/// </summary>
	private Vector3 CalcPlayPosition() {
		ballCollisions = 0;
		ballPath = new List<Vector4>();
		PhysicsManager.Instance.FastForward(0);
		for (float f = 0; f < 5; f += PhysicsManager.Instance.tickInterval) {
			PhysicsManager.Instance.Tick(PhysicsManager.Instance.tickInterval);
			if (ballCollisions > 1) { break; }
			Vector3 ballPosition = Game.instance.ball.state.position;
            if (ballPosition.y < minY || ballPosition.y > maxY) { continue; }
			ballPath.Add(new Vector4(ballPosition.x, ballPosition.y, ballPosition.z, Time.time + f));
		}
		PhysicsManager.Instance.Rewind();

		for (int i = 1; i < ballPath.Count; i++) {
			Vector3 posLast = ballPath[i - 1];
			Vector3 pos = ballPath[i];
			Debug.DrawLine(posLast, pos, Color.red, 5);
		}

		return Vector3.zero;
	}

	/// <summary>
	/// Calculates Distance between Pawn Position and TargetPosition.
	/// Calls Move() with normalized Distance.
	/// </summary>
	private void MoveTowardsTargetPosition() {

	}

	public void OnGui() {
		if (ballPath == null) { return; }

	}
	

}
