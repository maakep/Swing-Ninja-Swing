using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public static class GameManager {

    public static float Timer { get; set; }

    public static string LevelToBeLoaded { get; set; }
    public static string LevelToBeEdited { get; set; }
    public static string LevelJsonToBeTested { get; set; }

    public static string LoggedInUser { get; set; }

    // TODO
    public static string[] CampaignLevels { get; set; }


    static GameManager()
    {
        // Load maps in database table. Top 10 levels or so, manually inserted and updated.   CampaignLevels
        // Separate Highscore for these levels. Total highscore.
    }

    
}
