using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]

public class Ball : MonoBehaviour
{
    Vector3 curPos;
    Vector3 previousPos;
    float timeAtPreviousFrame;
    float timeBetweenFrames;
    Vector3 velocity;
    float gravity = 9.81f;

    new SphereCollider collider;

    private void Awake()
    {
        velocity = new Vector3(0, 0, 0);
        curPos = transform.position;
        previousPos = curPos;
        timeAtPreviousFrame = Time.time;
        timeBetweenFrames = Time.fixedDeltaTime;
        collider = GetComponent<SphereCollider>();
    }

    private void Update() {
		transform.position = Vector3.Lerp(previousPos, curPos, (Time.time - timeAtPreviousFrame) / Time.fixedDeltaTime);
    }

    private void FixedUpdate()
    {
        previousPos = curPos;

        // Update ball position
        ApplyGravitation();
		//MoveWithCollision();
		Move(velocity);

		timeBetweenFrames = Time.time - timeAtPreviousFrame;
        timeAtPreviousFrame = Time.time;
    }

    private void Move(Vector3 distance)
    {
        curPos += distance;
		Debug.Log("MOVING!");
    }

    private void ApplyGravitation()
    {
        velocity.y -= gravity * Time.fixedDeltaTime / 100f;
    }

    private void MoveWithCollision()
    {
        RaycastHit hit;
        bool collided = false;
        while (Physics.SphereCast(curPos, collider.radius, velocity.normalized, out hit, velocity.magnitude))
        {
            if (hit.collider != collider)
            {
                collided = true;
                break;
            }
        }

        if (collided)
        {
            //float rest = velocity.magnitude - hit.distance;
            //Move(hit.distance * velocity.normalized);
            //Vector3 refDir = Vector3.Reflect(velocity.normalized, hit.normal);
            //refDir *= velocity.magnitude;
            //velocity = refDir;
            //Move(refDir * rest);
        }
        else
        {
            //Move(velocity);
        }

    }

	public void OnCollisionEnter(Collision collision) {
		Vector3 collisionNormal = Vector3.zero;
		foreach (ContactPoint contact in collision.contacts) {
			collisionNormal += contact.normal;
		}
		//Debug.Log(collisionNormal.normalized);
		Vector3 refDir = Vector3.Reflect(velocity, collisionNormal.normalized);
		velocity = refDir;

		//TESTING
		if (collision.transform.parent == null) { return; }
		else if (collision.transform.parent.name == "Pawn") {
			velocity += collision.transform.parent.GetComponent<Pawn>().curVelocity * Time.fixedDeltaTime;
			Debug.Log(collision.transform.parent.GetComponent<Pawn>().curVelocity);
        }
	}
}
