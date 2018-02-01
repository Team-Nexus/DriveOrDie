using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour {

    public PlayerController playerController;

    public float gameStartTime;
    private float gameStartTimerCorrection;
    private bool gameIsStarting = true;

    public bool playerStopped = false;
    public float playerStopTimeLeft;
    public float playerStopLimit;
    public float playerStopSpeedLimit;

    public Text textGameStartTimer;
    public Text textGameStartHolder;

    public Text textPlayerStopTimer;
    public Text textPlayerStopHolder;


    public Text textGameTimer;
    public float minutes, seconds;

    // Use this for initialization
    void Start () {
        playerController.enabled = false;
        playerStopTimeLeft = playerStopLimit;
        textPlayerStopTimer.enabled = false;
        textPlayerStopHolder.enabled = false;

        gameStartTimerCorrection = gameStartTime;
	}
	
	// Update is called once per frame
	void Update () {
        if (gameIsStarting)
        {
            gameStartTime -= Time.deltaTime;
            int gameStartTimeInt = (int) (gameStartTime);
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
            minutes = (int)(Time.timeSinceLevelLoad / 60f);
            seconds = (int)((Time.timeSinceLevelLoad - gameStartTimerCorrection) % 60f);
            textGameTimer.text = minutes.ToString("00") + ":" + seconds.ToString("00");

            if (playerController.GetPlayerSpeed() <= playerStopSpeedLimit)
            {
                playerStopped = true;
                textPlayerStopTimer.enabled = true;
                textPlayerStopHolder.enabled = true;
                playerStopTimeLeft -= Time.deltaTime;
                int playerStopTimeInt = (int)(playerStopTimeLeft);
                textPlayerStopTimer.text = playerStopTimeInt.ToString();
            }

            if (playerController.GetPlayerSpeed() > playerStopSpeedLimit)
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
