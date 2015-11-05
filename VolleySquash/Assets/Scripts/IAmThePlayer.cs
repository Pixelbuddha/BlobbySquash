using UnityEngine;
using System.Collections;

public class IAmThePlayer : MonoBehaviour
{

    private Vector3 velocity = new Vector3(0, 0, 0);
    private float acceleration = 20;
    private float maxSpeed = 10;


    void MoveMe()
    {

        if (Input.GetKey(KeyCode.A))
        {
            velocity.x -= acceleration * Time.fixedDeltaTime;
        }
        //================
        if (Input.GetKey(KeyCode.D))
        {
            velocity.x += acceleration * Time.fixedDeltaTime;
        }
        velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);

        if (Input.GetKey(KeyCode.S))
        {
            velocity.z -= acceleration * Time.fixedDeltaTime;
        }
        //================
        if (Input.GetKey(KeyCode.W))
        {
            velocity.z += acceleration * Time.fixedDeltaTime;
        }
        velocity.z = Mathf.Clamp(velocity.z, -maxSpeed, maxSpeed);
        //================
        transform.position += velocity * Time.fixedDeltaTime;
    }
    //================

    void FixedUpdate()
    {
        MoveMe();
    }
}
