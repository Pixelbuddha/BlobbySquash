using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIPlayer : Controller {

	private Vector3 targetPosition;
	public Transform targetPos;
	public List<Vector4> ballPath;
	private Vector4 playPosition = Vector4.zero;
	private float _dashTime = 0f;

	public float playDistance = 1.5f;

	public float minY = 1.5f, maxY = 5.5f;

	public int ballCollisions;

	// Use this for initialization
	protected override void Start() {
		base.Start();
	}

	// Update is called once per frame
	public void Update() {
		if (Game.instance.activePlayerA) {
			ballPath = null;
			Vector3 direction = (targetPos.transform.position - this.transform.position).normalized;
			pawn.Move(direction);
		}

		else {
			if (Game.instance.ball.applyGravity == false) { playPosition = Game.instance.ball.transform.position - (Vector3.forward * 3); }
			if (ballPath == null) { playPosition = CalcPlayPosition(); _dashTime = 0; }
			if (playPosition != Vector4.zero) {
				var direction = (Vector3)playPosition - this.transform.position;
				pawn.Move((direction).normalized);
				Vector3 direction2D = direction;
				direction2D.y = 0;
				//Include Ball Pos Y!
				var interpol = (playPosition.y - minY) / (maxY - minY);
				//Debug.Log(direction2D.magnitude);
				//Debug.Log(playPosition.y);
				playDistance = 1.5f + interpol * 1.5f;
				//if (direction2D.magnitude < playDistance) {
				if (Time.time >= playPosition.w) { 
					pawn.Jump();
					//Debug.LogError("");
					playPosition = Vector4.zero;
					//pawn.Dash();
					_dashTime = Time.time + 0.1f;
				}
			}

			if (_dashTime != 0 && Time.time >= _dashTime) {
				pawn.Dash();
				_dashTime = 0;
			}
		}

	}

	/// <summary>
	/// TODO LATER WHEN BALL IS DONE.
	/// </summary>
	private Vector4 CalcPlayPosition() {
		ballCollisions = 0;
		ballPath = new List<Vector4>();
		var invalidPoints = new List<int>();
		PhysicsManager.Instance.FastForward(0);
		for (float f = 0; f < 5; f += PhysicsManager.Instance.tickInterval) {
			PhysicsManager.Instance.Tick(PhysicsManager.Instance.tickInterval);
			if (ballCollisions > 1) { break; }
			Vector3 ballPosition = Game.instance.ball.state.position;
			int invalid = 1;
			if (ballPosition.y < minY || ballPosition.y > maxY) { invalid = 0; ; }
			ballPath.Add(new Vector4(ballPosition.x, ballPosition.y, ballPosition.z, Time.time + f));
			invalidPoints.Add(invalid);
		}
		PhysicsManager.Instance.Rewind();

		var offset = Vector3.forward * 3;
		for (int i = 0; i < ballPath.Count; i++) {
			Vector4 pos = ballPath[i];
			if (invalidPoints[i] <= 0) { continue; }
			var distance = Vector3.Distance(transform.position, (Vector3)pos - offset);
			var timeDif = pos.w - Time.time;

			var speed = distance / timeDif;
			//Debug.Log("Req Speed: " + speed + " my Speed: " + (_pawn.maxSpeed * 0.9f));
			if (speed >= pawn.maxSpeed * 0.9f) {
				invalidPoints[i] = -1;
			}
		}
		var tarPos = Vector4.zero;

		//Debug Draw
		for (int i = 1; i < ballPath.Count; i++) {
			Vector4 posLast = ballPath[i - 1];
			Vector4 pos = ballPath[i];
			var col = Color.green;
			if (invalidPoints[i] == 0) { col = Color.red; }
			if (invalidPoints[i] == -1) { col = Color.yellow; }
			Debug.DrawLine(posLast, pos, col, 5);

			if (invalidPoints[i] > 0 && tarPos == Vector4.zero) {
				var interpol = (pos.y - minY) / (maxY - minY);
				playDistance = 1.5f + interpol * 1.5f;
				tarPos = pos - (Vector4)offset;
				var ballSpeed = ((Vector3)(pos - posLast)).magnitude / (pos.w - posLast.w);
				tarPos.w = pos.w - playDistance / ballSpeed;
				//Debug.Log(tarPos.w + " = " + ballSpeed + "Time: " + Time.time + "i: " + i);
			}
		}
		
		//ballPath = new List<Vector4>();
		return tarPos;
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
