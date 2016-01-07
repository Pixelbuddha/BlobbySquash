using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhysicsCollider : MonoBehaviour {

	public PhysicsObject physicsObject;
	public float bounciness = 0.99f;

	// Use this for initialization
	protected virtual void Start() {
	}

	public float GetCombinedBounciness(PhysicsCollider otherCol) {
		return (otherCol.bounciness + this.bounciness) / 2;
	}

	// Update is called once per frame
	protected virtual void Update() {
		
	}

	public Vector3 GetPositionOffset () {
		Vector3 worldPos = transform.position;
		return physicsObject.transform.InverseTransformPoint(worldPos);
	}
}
