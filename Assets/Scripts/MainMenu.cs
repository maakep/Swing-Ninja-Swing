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
        app = GameObject.Find("ApplicationManager").GetComponent<Persistent>();

        var levelEditorButton = GameObject.Find("LevelEditorButton").GetComponent<Button>();
        var playButton = GameObject.Find("PlayButton").GetComponent<Button>();
    
        levelEditorButton.onClick.AddListener(StartLevelEditor);
        playButton.onClick.AddListener(StartGame);

        var loadButton = GameObject.Find("LoadLevelButton").GetComponent<Button>();

        loadButton.onClick.AddListener(LoadLevel);
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

        StartCoroutine(DataLayer.LoadLevel((text) =>
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
        SceneManager.LoadScene("prototype");
    }

    void LoadLevel()
    {
        StartCoroutine(DataLayer.LoadLevel((text) =>
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
        }, GameObject.Find("LoadLevelText").GetComponent<InputField>().text));
    }
}
