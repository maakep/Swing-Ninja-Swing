using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine.UI;
using Assets.Scripts;

public class LevelEditor : MonoBehaviour {

    string json;
    string levelName;

    bool saved;

    Button saveButton;
    Button backButton;
    Button testButton;



	// Use this for initialization
	void Start () {
        saveButton = GameObject.Find("SaveButton").GetComponent<Button>();
        saveButton.onClick.AddListener(SaveLevel);

        backButton = GameObject.Find("BackButton").GetComponent<Button>();
        backButton.onClick.AddListener(Back);

        /*testButton = GameObject.Find("TestButton").GetComponent<Button>();
        testButton.onClick.AddListener(TestLevel);*/
        
	}

    private void Back()
    {
        if (saved)
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {

        }
    }

    private void TestLevel()
    {

    }

    void SaveLevel()
    {
        levelName = GameObject.Find("LevelName").GetComponent<InputField>().text;
        GameObject[] gbs = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> gbl = new List<GameObject>();
        gbl.AddRange(gbs);
        gbl.RemoveAll(PurgeUnwantedObjects);
        ObjectForJson[] obs = new ObjectForJson[gbl.Count];

        var i = 0;
        foreach (GameObject gb in gbl)
        {              
            ObjectForJson ofj = new ObjectForJson();

            ofj.PositionX = gb.transform.position.x;
            ofj.PositionY = gb.transform.position.y;
            ofj.PositionZ = gb.transform.position.z;

            ofj.ScaleX = gb.transform.localScale.x;
            ofj.ScaleY = gb.transform.localScale.y;
            ofj.ScaleZ = gb.transform.localScale.z;

            ofj.Name = gb.name;

            obs[i] = ofj;
            i++;
        }

        
        json = JsonConvert.SerializeObject(obs);
        /*Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\NinjaRope");
        File.WriteAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\NinjaRope\\level-" + levelName + "-" + SystemInfo.deviceUniqueIdentifier + ".json", json);*/
        StartCoroutine(DataLayer.SaveLevel((cb) => {
            if (cb != "" && cb != "Error")
            {
                SceneManager.LoadScene("MainMenu");
            }
            else
            {
                // Handle the error
            }
        }, levelName, json));
    }

    private bool PurgeUnwantedObjects(GameObject obj)
    {
        return obj.tag == "MainCamera" || obj.tag == "LevelEditor" || obj.layer != SortingLayer.NameToID("UI");
    }
}


class ObjectForJson
{
    public string Name { get; set; }

    public float PositionX { get; set; }
    public float PositionY { get; set; }
    public float PositionZ { get; set; }

    public float ScaleX { get; set; }
    public float ScaleY  { get; set; }
    public float ScaleZ { get; set; }
}
