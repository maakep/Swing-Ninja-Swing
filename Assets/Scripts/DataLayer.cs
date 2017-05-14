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
        public static string url;
        public static string User { get; set; }
        static string apiKey;

        static DataLayer()
        {
            User = SystemInfo.deviceUniqueIdentifier;
            apiKey = Resources.Load<TextAsset>("api_key").text;
            url = Resources.Load<TextAsset>("api_url").text;
        }


        public static IEnumerator SaveLevel(Action<string> callback, string levelName, string level)
        {
            WWWForm wwwForm = new WWWForm();
            wwwForm.AddField("insert", "true");
            wwwForm.AddField("user", User);
            wwwForm.AddField("name", levelName);
            wwwForm.AddField("level", level);
            wwwForm.AddField("apikey", apiKey);

            WWW www = new WWW(url, wwwForm);
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

        public static IEnumerator GetLevel(Action<string> callback, string levelName){
            WWWForm wwwForm = new WWWForm();
            wwwForm.AddField("select", "true");
            wwwForm.AddField("getlevel", "true");
            wwwForm.AddField("name", levelName);
            wwwForm.AddField("apikey", apiKey);
            
            WWW www = new WWW(url, wwwForm);
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

        public static IEnumerator GetAllLevels(Action<string> callback)
        {
            WWWForm wwwForm = new WWWForm();
            wwwForm.AddField("select", "true");
            wwwForm.AddField("getall", "true");
            wwwForm.AddField("apikey", apiKey);

            WWW www = new WWW(url, wwwForm);
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

        public static IEnumerator GetUserLevels(Action<string> callback)
        {
            WWWForm wwwForm = new WWWForm();
            wwwForm.AddField("select", "true");
            wwwForm.AddField("getlevel", "true");
            wwwForm.AddField("user", User);
            wwwForm.AddField("apikey", apiKey);

            WWW www = new WWW(url, wwwForm);
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


        public static IEnumerator DeleteLevel(Action<string> callback, string levelName)
        {
            WWWForm wwwForm = new WWWForm();
            wwwForm.AddField("delete", "true");
            wwwForm.AddField("name", levelName);
            wwwForm.AddField("user", User);
            wwwForm.AddField("apikey", apiKey);

            WWW www = new WWW(url, wwwForm);
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

        public static IEnumerator CreateUser(Action<string> callback, string username, string password)
        {
            WWWForm wwwForm = new WWWForm();
            wwwForm.AddField("insert", "true");
            wwwForm.AddField("user", username);
            wwwForm.AddField("pwd", password);
            wwwForm.AddField("apikey", apiKey);

            WWW www = new WWW(url, wwwForm);
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

        public static IEnumerator LoginUser(Action<string, string> callback, string username, string password)
        {
            WWWForm wwwForm = new WWWForm();
            wwwForm.AddField("select", "true");
            wwwForm.AddField("user", username);
            wwwForm.AddField("pwd", password);
            wwwForm.AddField("apikey", apiKey);

            WWW www = new WWW(url, wwwForm);
            yield return www;

            if (www.error == null)
            {
                string value;
                www.responseHeaders.TryGetValue("Success", out value);
                callback(www.text, value);
            }
            else
            {
                callback("Something went wrong, try again later", "Error");
            }
        }

        public static void TestLevel(string level)
        {
            GameManager.LevelJsonToBeTested = level;
            GameManager.LevelToBeEdited = level;
        }

    }
}
