using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhysicsObject : MonoBehaviour {
	//------------CONSTRUCTOR------------
	private bool _sleeping;
	private int _ticksSleeping;
	private Vector3 _lastTransform;

	//------------CONSTRUCTOR------------
	protected virtual void Start() {
		PhysicsManager.Instance.AddObject(this);
		_ticksSleeping = 0;
		_sleeping = true;
	}

	//------------DESTRUCTOR------------
	protected virtual void OnDestroy() {
		PhysicsManager.Instance.RemoveObject(this);
	}

	protected void CheckSleeping() {
		if (_lastTransform != transform.position) {
			_ticksSleeping++;
		}
		else {
			_ticksSleeping = 0;
			_sleeping = false;
		}

		if (_ticksSleeping >= PhysicsManager.ticksToSleep) { _sleeping = true; }
		_lastTransform = transform.position;
	}

	//------------PUBLIC METHOD------------
	public virtual void Tick(float deltaTime) {
		CheckSleeping();
		if (_sleeping) { return; }
	}
}
