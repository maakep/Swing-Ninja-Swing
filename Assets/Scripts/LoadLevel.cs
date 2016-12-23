using UnityEngine;
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
        if (SceneManager.GetActiveScene().name == "LevelEditor")
        {
            level = app.LevelToBeEdited;
        } else {
            level = app.LevelToBeLoaded;
        }

        if (string.IsNullOrEmpty(level))
            return string.Empty;

        /*var file = Directory.GetFiles(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\NinjaRope", "level-" + level + "*.json");
        var json = "";
        if (file.Length > 0)
        {
            json = File.ReadAllText(file[0]);
        }*/
        return level;
    }
}
