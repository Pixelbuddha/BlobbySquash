using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

	public static Game instance;

	public Camera mainCamera;

	// Use this for initialization
	void Start () {
		instance = this;
		mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
