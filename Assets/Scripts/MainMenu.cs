using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts;
using System;

public class MainMenu : MonoBehaviour {

    Persistent app;
    string levelName;



	// Use this for initialization
    void Start()
    {
        try
        {
            app = GameObject.Find("ApplicationManager").GetComponent<Persistent>();
        }catch(Exception){
            SceneManager.LoadScene("_app");
        }

        var levelEditorButton = GameObject.Find("LevelEditorButton").GetComponent<Button>();
        levelEditorButton.onClick.AddListener(StartLevelEditor);

        var playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        playButton.onClick.AddListener(StartGame);

        var loadButton = GameObject.Find("LoadLevelButton").GetComponent<Button>();
        loadButton.onClick.AddListener(() => LoadLevel());

        var loginButton = GameObject.Find("LoginButton").GetComponent<Button>();
        loginButton.onClick.AddListener(LoadLogin);

        if (!string.IsNullOrEmpty(app.LoggedInUser))
        {
            loginButton.transform.FindChild("Text").GetComponent<Text>().text = app.LoggedInUser;
        }
    }

    void StartLevelEditor()
    {

        //if textfield is empty, empty app.leveltobeloaded and load leveleditor.
        //else set app.leveltobeloaded to textfield in leveleditor do similar ifelse where it checks for map and loads it if possible.
        
        string editLevelName = GameObject.Find("LevelEditorLoadLevelText").GetComponent<InputField>().text;

        if (string.IsNullOrEmpty(editLevelName)){
            app.LevelToBeEdited = "";
            SceneManager.LoadScene("LevelEditor");
            return;
        }

        StartCoroutine(DataLayer.GetLevel((text) =>
        {
            if (!string.IsNullOrEmpty(text) && text != "Error")
            {
                app.LevelToBeEdited = text;
                SceneManager.LoadScene("LevelEditor");
            }
            else
            {
                // Handle error, search file locally?
                Debug.Log("Can't load level. Can't find level or no internet");
            }
        }, editLevelName));
    }

    void StartGame()
    {
        LoadLevel("bullseye");
    }

    void LoadLevel(string levelname = null)
    {
        StartCoroutine(DataLayer.GetLevel((text) =>
        {
            if (!string.IsNullOrEmpty(text) && text != "Error")
            {
                app.LevelToBeLoaded = text;
                SceneManager.LoadScene("LoadLevel");
            }
            else
            {
                // Handle error, search file locally?
                Debug.Log("Can't load level. Can't find level or no internet");
            }
        }, (levelname != null) ? levelname : GameObject.Find("LoadLevelText").GetComponent<InputField>().text));
    }

    void LoadLogin()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerLoggedIn")))
        {
            SceneManager.LoadScene("LoginScreen");
        }
        else
        {
            app.LoggedInUser = PlayerPrefs.GetString("PlayerLoggedIn");
        }
    }
}
