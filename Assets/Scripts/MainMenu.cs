﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts;
using System;

public class MainMenu : MonoBehaviour {

    void Start()
    {
        var levelEditorButton = GameObject.Find("LevelEditorButton").GetComponent<Button>();
        levelEditorButton.onClick.AddListener(StartLevelEditor);

        var playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        playButton.onClick.AddListener(StartGame);

        var loadButton = GameObject.Find("LoadLevelButton").GetComponent<Button>();
        loadButton.onClick.AddListener(() => LoadLevel());

        var loginButton = GameObject.Find("LoginButton").GetComponent<Button>();
        loginButton.onClick.AddListener(LoadLogin);

        var allLevelButton = GameObject.Find("AllLevelsButton").GetComponent<Button>();
        allLevelButton.onClick.AddListener(LoadAllLevels);

        if (!string.IsNullOrEmpty(GameManager.LoggedInUser) && GameManager.LoggedInUser != SystemInfo.deviceUniqueIdentifier)
        {
            loginButton.transform.FindChild("Text").GetComponent<Text>().text = GameManager.LoggedInUser;
        }
    }

    void StartLevelEditor()
    {

        //if textfield is empty, empty app.leveltobeloaded and load leveleditor.
        //else set app.leveltobeloaded to textfield in leveleditor do similar ifelse where it checks for map and loads it if possible.
        
        string editLevelName = GameObject.Find("LevelEditorLoadLevelText").GetComponent<InputField>().text;

        if (string.IsNullOrEmpty(editLevelName)){
            GameManager.LevelToBeEdited = "";
            SceneManager.LoadScene("LevelEditor");
            return;
        }

        StartCoroutine(DataLayer.GetLevel((text) =>
        {
            if (!string.IsNullOrEmpty(text) && text != "Error")
            {
                GameManager.LevelToBeEdited = text;
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
        levelname = levelname ?? GameObject.Find("LoadLevelText").GetComponent<InputField>().text;

        StartCoroutine(DataLayer.GetLevel((text) =>
        {
            if (!string.IsNullOrEmpty(text) && text != "Error")
            {
                GameManager.LevelToBeLoaded = new Level(levelname, text);
                SceneManager.LoadScene("LoadLevel");
            }
            else
            {
                // Handle error, search file locally?
                Debug.Log("Can't load level. Can't find level or no internet");
            }
        }, levelname));
    }

    void LoadLogin()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerLoggedIn")))
        {
            SceneManager.LoadScene("LoginScreen");
        }
        else
        {
            GameManager.LoggedInUser = PlayerPrefs.GetString("PlayerLoggedIn");
        }
    }

    private void LoadAllLevels()
    {
        SceneManager.LoadScene("AllLevelsBrowser");
    }
}
