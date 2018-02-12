using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour {

    public TextMeshProUGUI textPlayerMoney;

    void Start()
    {
        GetPlayerMoney();
    }

	public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void GetPlayerMoney()
    {
        textPlayerMoney.text = PlayerPrefs.GetInt("PlayerMoney").ToString();
    }
}
