
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float acceleration;
    public float revAcceleration;
    public float steering;
    public float playerSpeed;
    private Vector2 currentSpeed;


    public float fuelLeft;
    public float fuelUsage;
    private Rigidbody2D rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (Input.GetButton("Vertical")) {
            if (fuelLeft > 0)
            {
                fuelLeft = fuelLeft - fuelUsage;
                //Debug.Log("Fuel left:" + fuelLeft);
            }
        }
    }
    void FixedUpdate()
    {
        float horizontal = -Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // IF we have fuel, player can accelerate and reverse
        if (fuelLeft > 0)
        {
            if (vertical > 0)
            {
                // Forward
                Vector2 speed = transform.up * (vertical * acceleration);
                rigidBody.AddForce(speed);
            }
            else
            {
                // Reverse
                Vector2 speed = transform.up * (vertical * revAcceleration);
                rigidBody.AddForce(speed);
            }
        }

        float direction = Vector2.Dot(rigidBody.velocity, rigidBody.GetRelativeVector(Vector2.up));
        if (direction >= 0.0f)
        {
            rigidBody.rotation += horizontal * steering * (rigidBody.velocity.magnitude / 5.0f);
            //rb.AddTorque((h * steering) * (rb.velocity.magnitude / 10.0f));
        }
        else
        {
            rigidBody.rotation -= horizontal * steering * (rigidBody.velocity.magnitude / 5.0f);
            //rb.AddTorque((-h * steering) * (rb.velocity.magnitude / 10.0f));
        }

        Vector2 forward = new Vector2(0.0f, 0.5f);
        float steeringRightAngle;
        if (rigidBody.angularVelocity > 0)
        {
            steeringRightAngle = -90;
        }
        else
        {
            steeringRightAngle = 90;
        }

        Vector2 rightAngleFromForward = Quaternion.AngleAxis(steeringRightAngle, Vector3.forward) * forward;
        Debug.DrawLine((Vector3)rigidBody.position, (Vector3)rigidBody.GetRelativePoint(rightAngleFromForward), Color.green);

        float driftForce = Vector2.Dot(rigidBody.velocity, rigidBody.GetRelativeVector(rightAngleFromForward.normalized));

        Vector2 relativeForce = (rightAngleFromForward.normalized * -1.0f) * (driftForce * 10.0f);


        Debug.DrawLine((Vector3)rigidBody.position, (Vector3)rigidBody.GetRelativePoint(relativeForce), Color.red);

        rigidBody.AddForce(rigidBody.GetRelativeVector(relativeForce));

        GetPlayerSpeed();
    }

    public void AddFuel(float fuelAmount)
    {
        fuelLeft = fuelLeft + fuelAmount;
    }

    public float GetPlayerSpeed()
    {
        playerSpeed = rigidBody.velocity.magnitude;
        return playerSpeed;
    }

}