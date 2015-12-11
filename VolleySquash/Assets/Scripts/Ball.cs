using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	private Rigidbody myRigidbody;
	private float forceMultiplier;
	
	void Start () {
		myRigidbody = GetComponent<Rigidbody>();
		forceMultiplier = 0.3f;
    }

	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "Player") {
			Vector3 normal = Vector3.zero;
			foreach (ContactPoint contact in other.contacts) {
				normal += contact.normal;
			}
			Vector3 directionOut = Vector3.Reflect(myRigidbody.velocity, normal);
			myRigidbody.AddForce(directionOut.normalized * (other.gameObject.GetComponent<Pawn>().GetSpeed() * GetComponent<SphereCollider>().material.bounciness) * forceMultiplier);
		}
	}
}
