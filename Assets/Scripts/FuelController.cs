using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelController : MonoBehaviour {

    public int fuelAmount;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Debug.Log("Collected " + fuelAmount + " units of fuel!");
            collider.gameObject.SendMessage("AddFuel", fuelAmount);
            Destroy(gameObject);
        }
    }
}
