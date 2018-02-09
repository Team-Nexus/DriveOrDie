using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameTimer : MonoBehaviour {

    public Text textGameTimer;
    public float minutes, seconds;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        minutes = (int)(Time.timeSinceLevelLoad / 60f);
        seconds = (int)((Time.timeSinceLevelLoad - 5) % 60f);
        textGameTimer.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
