﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhysicsManager : MonoBehaviour {

	//------------FIELDS------------
	private static PhysicsManager _instance;
	public List<PhysicsObject> _listener = new List<PhysicsObject>();
	private float _timeElapsed;

	public static bool simulatedPhysic = false;
	
	public float tickInterval = 0.033f;

	//------------CONSTANTS------------
	public const int ticksToSleep = 5;
	public const float gavity = 9.81f;

	//------------PROPERTIES------------
	public static PhysicsManager Instance {
		get {
			if (_instance == null) {
				GameObject go = new GameObject();
				go.name = "_physicsManager";
				_instance = go.AddComponent<PhysicsManager>();
			}
			return _instance;
		}
	}

	//------------PUBLIC METHODS------------
	public void AddObject(PhysicsObject ob) {
		if (_listener.Contains(ob)) return;
		_listener.Add(ob);
	}

	public void RemoveObject(PhysicsObject ob) {
		if (!_listener.Contains(ob)) return;
		_listener.Remove(ob);
	}

	public void FastForward(float sec) {
		simulatedPhysic = true;
		foreach (PhysicsObject ob in _listener) {
			ob.Freeze();
		}

		while (sec > 0) {
			Tick(tickInterval);
			sec -= tickInterval;
		}
	}

	public void Rewind() {
		foreach (PhysicsObject ob in _listener) {
			ob.Unfreeze();
		}
		simulatedPhysic = false;
	}



	//------------PRIVATE METHODS------------
	private void Update() {
		_timeElapsed += Time.deltaTime;
		if (_timeElapsed >= 1f) { _timeElapsed = 1f; }

		while (_timeElapsed >= tickInterval) {
			Tick(tickInterval);
			_timeElapsed -= tickInterval;
		}
	}

	public void Tick(float deltaTime) {
		foreach (PhysicsObject ob in _listener) {
			ob.Tick(deltaTime);
		}

		CheckCollision();
	}

	//Optimize with multiply lists!
	private void CheckCollision() {
		for (int i = 0; i < _listener.Count; i++ ) {
			for (int j = i+1; j < _listener.Count; j++) {
				PhysicsCollision.Collide(_listener[i], _listener[j]);
			}
		}
	}
}
