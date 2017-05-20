using Assets.Scripts;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreenManager : MonoBehaviour {

    public GameObject ScoreEntry;

	void Start () {
        var replayButton = GameObject.Find("ReplayButton").GetComponent<Button>();
        replayButton.onClick.AddListener(Reload);

        var mainMenuButton = GameObject.Find("MainMenuButton").GetComponent<Button>();
        mainMenuButton.onClick.AddListener(LoadMenu);

        var timerText = GameObject.Find("TimerText").GetComponent<Text>();
        timerText.text = GameManager.Timer.ToString();

        var levelTitleText = GameObject.Find("LevelTitle").GetComponent<Text>();
        levelTitleText.text = GameManager.LevelToBeLoaded.Name;

        var highscorePanel = GameObject.Find("HighscorePanel");

        

        StartCoroutine(
            DataLayer.GetAllHighscoresForLevel((highscores) =>
            {
                foreach (Highscore hs in highscores)
                {
                    // Instansiate UI object and set text values. Add to some panel
                    var obj = Instantiate(ScoreEntry, highscorePanel.transform);
                    obj.transform.FindChild("Name").GetComponent<Text>().text = hs.User;
                    obj.transform.FindChild("Score").GetComponent<Text>().text = hs.Score;
                }
            }, GameManager.LevelToBeLoaded.Name)
        );

        StartCoroutine(
            DataLayer.SaveScore((text) =>
            {
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
