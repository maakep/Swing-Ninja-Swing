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

    void LoadMap()
    {
        var levelJson = GetJson();
        if (levelJson != "") { 
            ObjectForJson[] level = JsonConvert.DeserializeObject<ObjectForJson[]>(GetJson());

            foreach (ObjectForJson gb in level)

            {
                Debug.Log("Trying to instantiate: " + gb.Name);
                UnityEngine.Object block = Resources.Load(gb.Name.Split(' ')[0]);
                Vector3 pos = new Vector3(gb.PositionX, gb.PositionY, gb.PositionZ);
                Vector3 scale = new Vector3(gb.ScaleX, gb.ScaleY, gb.ScaleZ);

                GameObject blockObject = Instantiate(block, pos, Quaternion.identity) as GameObject;
                blockObject.transform.localScale = scale;
            }
        }
        else
        {
            // TODO: Error handling
            
        }
    }

    private string GetJson()
    {
        if (SceneManager.GetActiveScene().name == "LevelEditor")
        {
            level = app.LevelToBeEdited;
            if (level == "")
                return "";
        } else {
            level = app.LevelToBeLoaded;
        }

        /*var file = Directory.GetFiles(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\NinjaRope", "level-" + level + "*.json");
        var json = "";
        if (file.Length > 0)
        {
            json = File.ReadAllText(file[0]);
        }*/
        return level;
    }
}
