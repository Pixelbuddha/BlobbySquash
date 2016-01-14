using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public enum CameraStates {
		WORLD,
		BALL,
		CINEMATIC,
	}

	[SerializeField]
	private CameraStates cameraState;

	private Game game;

	private Vector3 worldCameraPosition;
	private Vector3 worldCameraRotation;


	// Use this for initialization
	void Start () {
		game = Game.instance;
	}
	
	// Update is called once per frame
	void Update () {
		switch (cameraState) {
			case CameraStates.WORLD:
				UpdateWorldCamera();
				return;
			case CameraStates.BALL:
				UpdateBallCamera();
				return;
			case CameraStates.CINEMATIC:
				UpdateCinematicCamera();
				return;
		}
	}

	private void UpdateWorldCamera() {

	}

	private void UpdateBallCamera() {

	}

	private void UpdateCinematicCamera() {

	}
}
