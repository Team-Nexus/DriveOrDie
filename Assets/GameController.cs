using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour {

    public PlayerController playerController;

    public float gameStartTime;
    private bool gameIsStarting = true;

    public bool playerStopped = false;
    public float playerStopTimeLeft;
    private float playerStopLimit = 5;

    public Text textGameStartTimer;
    public Text textGameStartHolder;

    public Text textPlayerStopTimer;
    public Text textPlayerStopHolder;

    // Use this for initialization
    void Start () {
        playerController.enabled = false;
        playerStopTimeLeft = playerStopLimit;
        textPlayerStopTimer.enabled = false;
        textPlayerStopHolder.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (gameIsStarting)
        {
            gameStartTime -= Time.deltaTime;
            int gameStartTimeInt = (int) (gameStartTime + 0.5);
            textGameStartTimer.text = gameStartTimeInt.ToString();
            if (gameStartTime <= 0)
            {
                textGameStartTimer.enabled = false;
                textGameStartHolder.enabled = false;
                playerController.enabled = true;
                gameIsStarting = false;
            }
        }

        if (playerController.enabled)
        {
            if (playerController.GetPlayerSpeed() <= 5)
            {
                playerStopped = true;
                textPlayerStopTimer.enabled = true;
                textPlayerStopHolder.enabled = true;
                playerStopTimeLeft -= Time.deltaTime;
                int playerStopTimeInt = (int)(playerStopTimeLeft + 0.5);
                textPlayerStopTimer.text = playerStopTimeInt.ToString();
            }

            if (playerController.GetPlayerSpeed() > 5)
            {
                playerStopped = false;
                textPlayerStopTimer.enabled = false;
                textPlayerStopHolder.enabled = false;
                playerStopTimeLeft = playerStopLimit;
            }
        }

        if (playerStopped)
        {
            if (playerStopTimeLeft <= 0)
            {
                textPlayerStopTimer.enabled = false;
                textPlayerStopHolder.text = "GAME OVER!";
                playerController.enabled = false;
            }
        }
    }
}
