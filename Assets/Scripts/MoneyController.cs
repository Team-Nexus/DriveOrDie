using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyController : MonoBehaviour {

    public int moneyAmount;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Debug.Log("Collected " + moneyAmount + " money!");
            PlayerPrefs.SetInt("PlayerMoney", PlayerPrefs.GetInt("PlayerMoney") + moneyAmount);
            Destroy(gameObject);
        }
    }
}

