using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public static class GameManager {

    public static float Timer { get; set; }

    public static Level LevelToBeLoaded { get; set; }
    public static string LevelToBeEdited { get; set; }
    public static string LevelJsonToBeTested { get; set; }

    public static string LoggedInUser { get; set; }

    // TODO
    public static string[] CampaignLevels { get; set; }


    static GameManager()
    {
        LoggedInUser = SystemInfo.deviceUniqueIdentifier;

        // Load maps in database table. Top 10 levels or so, manually inserted and updated.   CampaignLevels
        // Separate Highscore for these levels. Total highscore.
    }

    
}


public class Level
{
    public string Name { get; set; }
    public string Json { get; set; }

    public Level(string levelName = null, string level = null)
    {
        Name = levelName;
        Json = level;
    }
}


public class Highscore
{
    public string User { get; set; }
    public string LevelName { get; set; }
    public string Score { get; set; }
    public string Date { get; set; }

    public Highscore(
        string user = null,
        string levelName = null,
        string score = null,
        string date = null)
    {
        User = user;
        LevelName = levelName;
        Score = score;
        Date = date;
    }
}
/* 
 * 
 * [{"User":"USR","LevelName":"LVL","Score":"SCR","Date":"DAT"},{"User":"USR2","LevelName":"LVL2","Score":"SCR2","Date":"DAT2"}] 
 *  
 */
