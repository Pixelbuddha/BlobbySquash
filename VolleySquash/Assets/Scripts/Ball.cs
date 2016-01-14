using UnityEngine;
using System.Collections;

public class Ball : PhysicsObject {

	private Transform _ghostBody;

	public static Ball instance;
	

	protected override void Start() {
		base.Start();
		instance = this;
		OnCollision += OnCollide;
		//_ghostBody = transform.FindChild("GhostBody");
		//_ghostBody.SetParent(transform.parent);
	}
	
	public override void Update() {
		base.Update();
		//PhysicsManager.Instance.FastForward(5f);
		//Vector3 futurePosition = _physicsObject.state.position;
		//PhysicsManager.Instance.Rewind();
		//_ghostBody.transform.position = futurePosition;
	}

	public void OnCollide(PhysicsCollider collider) {
		//Debug.Log(state.velocity);
		Game.instance.BallCollide(collider);
		if (PhysicsManager.simulatedPhysic && collider.GetType() == typeof(PhysicsPlane)) {
			foreach (var obj in Object.FindObjectsOfType<AIPlayer>()) {
				obj.ballCollisions++;
			}
        }
	}
}
