using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreenManager : MonoBehaviour {

    Persistent app;

	// Use this for initialization
	void Start () {
        var replayButton = GameObject.Find("ReplayButton").GetComponent<Button>();
        replayButton.onClick.AddListener(Reload);

        var mainMenuButton = GameObject.Find("MainMenuButton").GetComponent<Button>();
        mainMenuButton.onClick.AddListener(LoadMenu);

        app = GameObject.Find("ApplicationManager").GetComponent<Persistent>();
        var timerText = GameObject.Find("TimerText").GetComponent<Text>();
        timerText.text = app.Timer.ToString();
	}

    void Reload()
    {
        SceneManager.LoadScene("LoadLevel");
    }

    void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
