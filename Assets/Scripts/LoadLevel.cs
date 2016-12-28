﻿using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {

    Persistent app;
    string level;
    
	// Use this for initialization
	void Start () {
        app = GameObject.Find("ApplicationManager").GetComponent<Persistent>();
        LoadMap();
	}

    void LoadMap(string levelJson = null)
    {
        levelJson = levelJson ?? GetJson();
        if (!string.IsNullOrEmpty(levelJson)) {
            Debug.Log(levelJson);
            ObjectForJson[] level = JsonConvert.DeserializeObject<ObjectForJson[]>(GetJson());

            foreach (ObjectForJson gb in level)
            {
                UnityEngine.Object block = Resources.Load(gb.Name.Split(' ')[0]);

                Vector3 pos = new Vector3(gb.PositionX, gb.PositionY, gb.PositionZ);
                Vector3 scale = new Vector3(gb.ScaleX, gb.ScaleY, gb.ScaleZ);

                GameObject blockObject = Instantiate(block, pos, Quaternion.identity) as GameObject;
                blockObject.name = blockObject.name.Split('(')[0];
                blockObject.transform.localScale = scale;
            }
        }
        else
        {
            Debug.Log("Couldn't find level");
        }
    }

    private string GetJson()
    {
        level = GetLevelToLoad();

        /*var file = Directory.GetFiles(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\NinjaRope", "level-" + level + "*.json");
        var json = "";
        if (file.Length > 0)
        {
            json = File.ReadAllText(file[0]);
        }*/
        return level;
    }

    private string GetLevelToLoad()
    {
        string level = "";
        switch (SceneManager.GetActiveScene().name)
        {
            case "LevelEditor":
                level = app.LevelToBeEdited;
                break;

            case "TestLevel":
                level = app.LevelJsonToBeTested;
                break;

            default:
                level = app.LevelToBeLoaded;
                break;
        }

        return level;
    }
}