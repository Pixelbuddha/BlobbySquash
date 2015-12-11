using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhysicsManager : MonoBehaviour {

	//------------FIELDS------------
	private static PhysicsManager _instance;
	private List<PhysicsObject> _listener;
	private float _timeElapsed;

	[SerializeField]
	private float _tickInterval = 0.1f;

	//------------CONSTANTS------------
	public const int ticksToSleep = 5;

	//------------PROPERTIES------------
	public static PhysicsManager Instance {
		get {
			if (_instance == null) {
				GameObject go = new GameObject();
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

	//------------PRIVATE METHODS------------
	private void Update() {
		_timeElapsed += Time.deltaTime;
		if (_timeElapsed >= 1f) { _timeElapsed = 1f; }

		while (_timeElapsed >= _tickInterval) {
			Tick(_tickInterval);
			_timeElapsed -= _tickInterval;
		}
	}

	private void Tick(float deltaTime) {
		foreach (PhysicsObject ob in _listener) {
			ob.Tick(deltaTime);
		}
	}
}
