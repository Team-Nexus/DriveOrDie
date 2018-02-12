
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // vehicle speed and control variables
    bool dontUseTouch;
    public float acceleration;
    public float revAcceleration;
    public float steering;
    public float steeringTouchMultiplier;
    float playerSpeed;
    public float playerSpeedMax;
    private Vector2 currentSpeed;

    public int fuelLeft;
    public int fuelUsage;
    private Rigidbody2D rigidBody;

    // player related UI = speed / fuel
    public Text uiTextFuelLeft;
    public Text uiTextSpeed;

    // touch variables
    float touchArea = Screen.width / 10;
    float screenHalf = Screen.width / 2;

    // camera variables
    public Camera mainCamera;
    public float camZoomMin;
    public float camZoomMax;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        uiTextFuelLeft.text = fuelLeft.ToString();
        uiTextSpeed.text = GetPlayerSpeed().ToString();

        mainCamera.orthographicSize = camZoomMin;

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
            //textPos.text = "x: " + Input.GetTouch(0).position.x + " y: " + Input.GetTouch(0).position.y;

            if (Input.touchCount == 1)
            {
                vertical = 1; // Accelerate
            }
            else if (Input.touchCount >= 2)
            {
                vertical = -1; // Reverse
            }

            if (Input.GetTouch(0).position.x < screenHalf - touchArea)
            {

                steering = Map(screenHalf, Screen.width, 0, screenHalf, Input.GetTouch(0).position.x) * -steeringTouchMultiplier;
                horizontal = 1; // turn left
            }
            else if (Input.GetTouch(0).position.x > screenHalf + touchArea)
            {

                steering = Map(0, screenHalf, -600, 0, Input.GetTouch(0).position.x) * steeringTouchMultiplier;
                horizontal = -1; // turn right
            }
        }
        
        // IF we have fuel, player can accelerate and reverse
        if (fuelLeft > 0)
        {
            if (vertical > 0)
            {
                if (GetPlayerSpeed() < playerSpeedMax) {
                    // Forward
                    Vector2 speed = transform.up * (vertical * acceleration);
                    rigidBody.AddForce(speed);
                }
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

        if (GetPlayerSpeed() < playerSpeedMax)
        {
            mainCamera.orthographicSize = Map(0, playerSpeedMax, camZoomMin, camZoomMax, GetPlayerSpeed());
        }

        GetPlayerSpeed();
    }

    public void AddFuel(int fuelAmount)
    {
        fuelLeft = fuelLeft + fuelAmount;
        rigidBody.AddForce(transform.up * 30, ForceMode2D.Impulse);
    }

    public float GetPlayerSpeed()
    {
        playerSpeed = rigidBody.velocity.magnitude;
        return playerSpeed;
    }

    public float Map(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

}



