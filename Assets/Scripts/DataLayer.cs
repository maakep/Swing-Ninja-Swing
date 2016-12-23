using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    static class DataLayer
    {
        const string URL = "http://hajkep.se/unity/NinjaRope/DbLayer.php";
        public static string User { get; set; }

        static Persistent app;

        static DataLayer()
        {
            User = SystemInfo.deviceUniqueIdentifier;
            app = GameObject.Find("ApplicationManager").GetComponent<Persistent>();
        }


        public static IEnumerator SaveLevel(Action<string> callback, string levelName, string level)
        {
            WWWForm wwwForm = new WWWForm();

            wwwForm.AddField("insert", "true");
            wwwForm.AddField("user", User);
            wwwForm.AddField("name", levelName);
            wwwForm.AddField("level", level);

            WWW www = new WWW(URL, wwwForm);
            yield return www;
            if (www.error == null)
            {
                callback(www.text);
            }
            else
            {
                callback("Error");
            }
        }

        public static IEnumerator LoadLevel(string levelName){
            WWWForm wwwForm = new WWWForm();
            wwwForm.AddField("select", "true");
            wwwForm.AddField("name",levelName);
            WWW www = new WWW(URL, wwwForm);
            yield return www;
            Debug.Log(www.text);

            app.LevelToBeLoaded = www.text;
            SceneManager.LoadScene("LoadLevel");
            
        }
    }
}
