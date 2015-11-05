using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]

public class Ball : MonoBehaviour {
    Vector3 curPos;
    Vector3 previousPos;
    float timeAtPreviousFrame;
    float timeBetweenFrames;
    Vector3 velocity;
    float gravity = 9.81f;

    private void Awake()
    {
        velocity = new Vector3(0, -0.5f, 0);
        curPos = transform.position;
        previousPos = curPos;
        timeAtPreviousFrame = Time.time;
        timeBetweenFrames = Time.fixedDeltaTime;
    }

    private void Update()
    {
        Debug.Log(timeBetweenFrames);
        transform.position = Vector3.Lerp(previousPos, curPos, (Time.time - timeAtPreviousFrame) / Time.fixedDeltaTime);
    }

    private void FixedUpdate()
    {
        previousPos = curPos;

        // Update ball position
        curPos += velocity;
        timeBetweenFrames = Time.time - timeAtPreviousFrame;
        timeAtPreviousFrame = Time.time;
    }
}
