using UnityEngine;
using System.Collections;

public class Ball : PhysicsObject {

	private Transform _ghostBody;

	public static Ball instance;
	private Vector3 _wobble = Vector3.one;
	private Transform body;


	protected override void Start() {
		base.Start();
		instance = this;
		OnCollision += OnCollide;
		body = transform.GetChild(0);
		//_ghostBody = transform.FindChild("GhostBody");
		//_ghostBody.SetParent(transform.parent);
	}

	public override void Update() {
		base.Update();

		var scale = body.localScale;
		scale.x *= _wobble.x;
		scale.y *= _wobble.y;
		scale.z *= _wobble.z;

		//scale = scale.normalized;
		body.localScale = Vector3.Lerp(body.localScale, scale, 0.4f);
		_wobble = Vector3.Lerp(_wobble, Vector3.one, 0.6f);
		body.localScale = Vector3.Lerp(body.localScale, Vector3.one, 0.2f);
		//PhysicsManager.Instance.FastForward(5f);
		//Vector3 futurePosition = _physicsObject.state.position;
		//PhysicsManager.Instance.Rewind();
		//_ghostBody.transform.position = futurePosition;
	}

	public void OnCollide(PhysicsCollider collider) {
		//Debug.Log(state.velocity);
		Game.instance.BallCollide(collider);
		if (PhysicsManager.simulatedPhysic && collider.GetType() == typeof(PhysicsPlane)) {
			if (collider.name == "WallLeft" || collider.name == "WallRight" || collider.name == "WallUp") { return; }
			foreach (var obj in Object.FindObjectsOfType<AIPlayer>()) {

				obj.ballCollisions++;
			}
		}
		if (!PhysicsManager.simulatedPhysic && collider.GetType() == typeof(PhysicsPlane)) {
			var plane = (PhysicsPlane)collider;
			var normal = plane.Normal;
			normal.x = Mathf.Abs(normal.x);
			normal.y = Mathf.Abs(normal.y);
			normal.z = Mathf.Abs(normal.z);

			Vector3 wobble = Vector3.one;
			if (normal.x > 0.5) { wobble = new Vector3(0.5f, 1.25f, 1.25f); }
			if (normal.y > 0.5) { wobble = new Vector3(1.25f, 0.5f, 1.25f); }
			if (normal.z > 0.5) { wobble = new Vector3(1.25f, 1.25f, 0.5f); }

			_wobble = wobble;
		}
	}
}
