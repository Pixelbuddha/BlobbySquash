using UnityEngine;
using System.Collections;

public class ObjectPhysics : MonoBehaviour
{
    private const float gravity = 9.81f;
    public float mass = 100f;
    public float friction = 100f;
    //Bernoulli? Ballistik? etc...
    private Vector3 velocity = new Vector3(0, 0, 0);
    private SphereCollider col;


    /// <summary> =======Air FRICTION CALCULATION=======
    /// FG = m * g
    /// g = gravity, m = Masse
    ///
    /// FL = 1/2 * Rho * A * v² * Cw
    ///
    /// Rho = Luftdichte, A = Fläche Körper, Cw = Widerstandsbeiwert (Geometrieabhängiger Widerstand), v = velocity
    /// 
    /// FL: kg/m³ * m² * (m/s)² = (kg * m) / s² = Newton
    /// FG: kg * m/s²= Newton
    /// 
    /// Cw Kugel = 0,47
    /// Rho = 1,2 (kg/m³)
    /// 
    /// Beschleunigung = FG - FL, im falle von v = 0 ist FL = 0 -> volle gravitation
    /// 
    /// FG = 9,81 * m
    /// FL = A * v² * 0,282
    /// 
    /// Newton ist die Kraft die auf den Körper wirkt: g = F/m = (FG - FL) / m
    /// 
    /// = (m * g - 1/2 * Rho * A * v² * Cw) / m
    /// 
    /// </summary> =======FRICTION CALCULATION=======





    void Awake()
    {
        col = GetComponent<SphereCollider>();
    }

    void Update()
    {
        JumpTest();
    }

    void FixedUpdate()
    {
        Vector3 v = transform.position; //ist das nicht sinnlos?! Es wird nie verwendet
        velocity.y -= gravity * Time.fixedDeltaTime;
        transform.position += velocity * Time.fixedDeltaTime;

        //Debug.Log("" + velocity);

        Vector3 rayStart = col.center + transform.position;
        float rayLength = 100f;
        Debug.DrawLine(rayStart, velocity.normalized * rayLength + rayStart);

        RaycastHit hit;
        if(Physics.Raycast(rayStart, velocity.normalized, out hit, rayLength))
        {
            //Debug.Log(hit.normal);
        }

        // friction:
        // !grounded, velocity *= 0.98?
        // 
    }

    void JumpTest()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = 10;
        }

    }

}
