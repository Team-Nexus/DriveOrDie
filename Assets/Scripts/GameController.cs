﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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

    public TextMeshProUGUI textPlayerMoney;
    public TextMeshProUGUI textGameTimer;
    public float minutes, seconds;

    public GameObject pauseMenuUI;
    public GameObject uiCanvas;

    // Use this for initialization
    void Start () {
        playerController.enabled = false;
        playerStopTimeLeft = playerStopLimit;
        textPlayerStopTimer.enabled = false;
        textPlayerStopHolder.enabled = false;

        gameStartTimerCorrection = gameStartTime;

        GetPlayerMoney();

        Restart();
	}
	
	// Update is called once per frame
	void Update () {
        GetPlayerMoney();

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
                // show stop timer
                textPlayerStopTimer.enabled = true;
                textPlayerStopHolder.enabled = true;
                playerStopTimeLeft -= Time.deltaTime;
                int playerStopTimeInt = (int)(playerStopTimeLeft);
                textPlayerStopTimer.text = playerStopTimeInt.ToString();
            }

            if (playerController.GetPlayerSpeed() > playerStopSpeedLimit)
            {
                playerStopped = false;
                // if player moves hide stop timer
                textPlayerStopTimer.enabled = false;
                textPlayerStopHolder.enabled = false;
                playerStopTimeLeft = playerStopLimit;
            }
        }

        // IF player has stopped and has not moved stop the game
        if (playerStopped)
        {
            if (playerStopTimeLeft <= 0)
            {
                Pause();
            }
        }
    }

    public void GetPlayerMoney()
    {
        textPlayerMoney.text = PlayerPrefs.GetInt("PlayerMoney").ToString();
    }

    public void RestartGame()
    {
        Debug.Log("Restart");
        Restart();
        SceneManager.LoadScene("TestMap");
    }

    public void ReturnToMenu()
    {
        PlayerPrefs.Save();
        Debug.Log("Return");
        Restart();
        SceneManager.LoadScene("MainMenu");
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        uiCanvas.SetActive(false);
        Time.timeScale = 0f;
    }

    private void Restart()
    {
        pauseMenuUI.SetActive(false);
        uiCanvas.SetActive(true);
        Time.timeScale = 1f;
        gameIsStarting = true;
    }
}
