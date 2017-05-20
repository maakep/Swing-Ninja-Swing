using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreenManager : MonoBehaviour {

	void Start () {
        var replayButton = GameObject.Find("ReplayButton").GetComponent<Button>();
        replayButton.onClick.AddListener(Reload);

        var mainMenuButton = GameObject.Find("MainMenuButton").GetComponent<Button>();
        mainMenuButton.onClick.AddListener(LoadMenu);

        var timerText = GameObject.Find("TimerText").GetComponent<Text>();
        timerText.text = GameManager.Timer.ToString();

        //var levelTitleText = GameObject.Find("LevelTitle").GetComponent<Text>();
        //levelTitleText.text = GameManager.LevelToBeLoaded.Name;

        StartCoroutine(
            DataLayer.SaveScore((text) =>
            {
                Debug.Log("Score saved:");
                Debug.Log(text);
            }, GameManager.LevelToBeLoaded.Name, GameManager.Timer)
        );
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
