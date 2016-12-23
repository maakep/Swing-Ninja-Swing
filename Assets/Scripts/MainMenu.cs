using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts;

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
        string level = GameObject.Find("LevelEditorLoadLevelText").GetComponent<InputField>().text;
        if (level == "" || level == null)
        {
            app.LevelToBeEdited = "";
        }
        else
        {
            app.LevelToBeEdited = level;
        }
        SceneManager.LoadScene("LevelEditor");
    }

    void StartGame()
    {
        SceneManager.LoadScene("prototype");
    }

    void LoadLevel()
    {
        StartCoroutine(DataLayer.LoadLevel(GameObject.Find("LoadLevelText").GetComponent<InputField>().text));
    }
}
