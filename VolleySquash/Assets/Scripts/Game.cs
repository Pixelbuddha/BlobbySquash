using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Game : MonoBehaviour {

	public static Game instance;

	public Camera mainCamera;
		
	public bool activePlayerA;
	public Pawn playerA, playerB;
	public Ball ball;
	public bool ballTouched;
	public Text ScoreBoard;

	// Use this for initialization
	void Start () {
		instance = this;
		mainCamera = Camera.main;
		StartGame();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void InitGame() {
		SetActivePlayer(activePlayerA);
		playerA.SetPosition(playerA.startPos);
		playerB.SetPosition(playerB.startPos);
		ball.SetPosition(ball.startPos);
		ball.applyGravity = false;
		ballTouched = false;
    }

	private void StartGame() {
		SetActivePlayer(true);
		InitGame();
	}

	public void BallCollide(PhysicsCollider collider) {
		if (PhysicsManager.simulatedPhysic) { return; }
		if (collider.name == "WallLeft" || collider.name == "WallRight") { return; }

		if (collider.name == "ActiveWall") { SetActivePlayer(!activePlayerA); ballTouched = false; return; }

		bool isA = collider.physicsObject.gameObject == playerA.gameObject;
		bool isB = collider.physicsObject.gameObject == playerB.gameObject;
		if (isA || isB) {  ballTouched = false; return; }
		
		if (!ballTouched) { ballTouched = true; return; }

		else {
			var nonActivePlayer = activePlayerA ? playerB : playerA;
			nonActivePlayer.matchPoints++;
			InitGame();
			CheckPoints();
		}
	}

	public void SetActivePlayer(bool A) {
		activePlayerA = A;
		foreach (PhysicsCollider col in playerA.colliders) {
			col.collideWithSphere = A;
		}
		foreach (PhysicsCollider col in playerB.colliders) {
			col.collideWithSphere = !A;
		}
		UpdateScore();
	}

	public void CheckPoints() {
		UpdateScore();
        var activePlayer = activePlayerA ? playerA : playerB;
		var nonActivePlayer = activePlayerA ? playerB : playerA;

		if (playerA.HasWon(playerB)) {
			playerA.score++;
			playerA.matchPoints = 0;
			playerB.matchPoints = 0;
		}

		else if (playerB.HasWon(playerA)) {
			playerB.score++;
			playerA.matchPoints = 0;
			playerB.matchPoints = 0;
		}

		UpdateScore();
    }

	public void UpdateScore() {
		string activePlayer = activePlayerA ? " << " : " >> ";
		ScoreBoard.text = "A " + playerA.score + " - " + playerA.matchPoints + activePlayer + playerB.matchPoints + " - " + playerB.score + " B";
	}
}
