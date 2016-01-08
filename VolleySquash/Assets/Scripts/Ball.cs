using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PhysicsObject))]
public class Ball : MonoBehaviour {

	public PhysicsObject physicsObject;
	private Transform _ghostBody;

	public static Ball instance;
	

	public void Start() {
		instance = this;
		physicsObject = GetComponent<PhysicsObject>();
		//_ghostBody = transform.FindChild("GhostBody");
		//_ghostBody.SetParent(transform.parent);
	}
	
	public void Update() {
		//PhysicsManager.Instance.FastForward(5f);
		//Vector3 futurePosition = _physicsObject.state.position;
		//PhysicsManager.Instance.Rewind();
		//_ghostBody.transform.position = futurePosition;
	}
}
