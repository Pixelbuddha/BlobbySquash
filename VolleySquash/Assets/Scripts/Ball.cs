using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PhysicsObject))]
public class Ball : MonoBehaviour {

	private PhysicsObject _physicsObject;
	private Transform _ghostBody;

	public void Start() {
		_physicsObject = GetComponent<PhysicsObject>();
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
