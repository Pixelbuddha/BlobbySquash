using UnityEngine;
using System.Collections;

public class BallCanvas : MonoBehaviour {

	[SerializeField]
	private bool followX, followY, followZ;

	[SerializeField]
	private GameObject target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newPos = this.transform.position;
		if (followX) {
			newPos.x = target.transform.position.x;
		}
		if (followY) {
			newPos.y = target.transform.position.y;
		}
		if (followZ) {
			newPos.z = target.transform.position.z;
		}
		this.transform.position = newPos;
	}
}
