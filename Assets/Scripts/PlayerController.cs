
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    bool dontUseTouch;
    public float acceleration;
    public float revAcceleration;
    public float steering;
    public float playerSpeed;
    private Vector2 currentSpeed;

    public int fuelLeft;
    public int fuelUsage;
    private Rigidbody2D rigidBody;

    public Text uiTextFuelLeft;
    public Text uiTextSpeed;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        uiTextFuelLeft.text = fuelLeft.ToString();
        uiTextSpeed.text = GetPlayerSpeed().ToString();

        //check if our current system info equals a desktop
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            //we are on a desktop device, so don't use touch
            dontUseTouch = true;
        }
        //if it isn't a desktop, lets see if our device is a handheld device aka a mobile device
        else if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            //we are on a mobile device, so lets use touch input
            dontUseTouch = false;
        }
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

        uiTextSpeed.text = GetPlayerSpeed().ToString();
        uiTextFuelLeft.text = fuelLeft.ToString();
    }
    void FixedUpdate()
    {
        //Computer controls
        float horizontal = -Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (!dontUseTouch)
        {
            if (Input.touchCount == 1)
            {
                vertical = 1; // Accelerate
            }
            else if (Input.touchCount >= 2)
            {
                vertical = -1; // Reverse
            }

            if (Input.GetTouch(0).position.x < (Screen.width / 2) - Screen.width / 5)
            {
                horizontal = 1; // turn left
            }
            else if (Input.GetTouch(0).position.x > (Screen.width / 2) + Screen.width / 5)
            {
                horizontal = -1; // turn right
            }
        }
        
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

    public void AddFuel(int fuelAmount)
    {
        fuelLeft = fuelLeft + fuelAmount;
    }

    public float GetPlayerSpeed()
    {
        playerSpeed = rigidBody.velocity.magnitude;
        return playerSpeed;
    }

}

