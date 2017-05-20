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
        static string apiKey;

        static DataLayer()
        {
            apiKey = Resources.Load<TextAsset>("api_key").text;
            url = Resources.Load<TextAsset>("api_url").text;
        }


        public static IEnumerator SaveLevel(Action<string> callback, string levelName, string level)
        {
            WWWForm wwwForm = NewForm();
            wwwForm.AddField("insert", "true");
            wwwForm.AddField("user", GameManager.LoggedInUser);
            wwwForm.AddField("name", levelName);
            wwwForm.AddField("level", level);
            
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

        public static IEnumerator SaveScore(Action<string> callback, string levelName, float score)
        {
            if (GameManager.LoggedInUser == SystemInfo.deviceUniqueIdentifier)
            {
                callback("User not logged in");
                yield break;
            }

            Debug.Log("Inserting highscore");

            WWWForm wwwForm = NewForm();
            wwwForm.AddField("insert", "true");
            wwwForm.AddField("user", GameManager.LoggedInUser);
            wwwForm.AddField("name", levelName);
            wwwForm.AddField("highscore", score.ToString());
            

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
        public static IEnumerator GetAllHighscores(Action<string> callback)
        {
            WWWForm wwwForm = NewForm();
            wwwForm.AddField("select", "true");
            wwwForm.AddField("GetAllHighScores", "true");

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
        public static IEnumerator GetAllHighscoresForLevel(Action<string> callback, string levelName)
        {
            WWWForm wwwForm = NewForm();
            wwwForm.AddField("select", "true");
            wwwForm.AddField("GetAllHighscoresForLevel", "true");
            wwwForm.AddField("name", levelName);

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
        public static IEnumerator GetUserScoreForLevel(Action<string> callback, string levelName)
        {
            if (GameManager.LoggedInUser == SystemInfo.deviceUniqueIdentifier)
            {
                callback("User nog logged in");
                yield break;
            }

            WWWForm wwwForm = NewForm();
            wwwForm.AddField("select", "true");
            wwwForm.AddField("user", GameManager.LoggedInUser);
            wwwForm.AddField("GetUserScoreForLevel", "true");
            wwwForm.AddField("name", levelName);
            
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
            WWWForm wwwForm = NewForm();
            wwwForm.AddField("select", "true");
            wwwForm.AddField("getlevel", "true");
            wwwForm.AddField("name", levelName);
            
            
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
            WWWForm wwwForm = NewForm();
            wwwForm.AddField("select", "true");
            wwwForm.AddField("getall", "true");
            

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
            WWWForm wwwForm = NewForm();
            wwwForm.AddField("select", "true");
            wwwForm.AddField("getlevel", "true");
            wwwForm.AddField("user", GameManager.LoggedInUser);
            

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
            WWWForm wwwForm = NewForm();
            wwwForm.AddField("delete", "true");
            wwwForm.AddField("name", levelName);
            wwwForm.AddField("user", GameManager.LoggedInUser);
            

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
            WWWForm wwwForm = NewForm();
            wwwForm.AddField("insert", "true");
            wwwForm.AddField("user", username);
            wwwForm.AddField("pwd", password);
            

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
            WWWForm wwwForm = NewForm();
            wwwForm.AddField("select", "true");
            wwwForm.AddField("user", username);
            wwwForm.AddField("pwd", password);

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

        private static WWWForm NewForm()
        {
            WWWForm wwwForm = new WWWForm();
            wwwForm.AddField("apikey", apiKey);
            return wwwForm;
        }

    }
}
