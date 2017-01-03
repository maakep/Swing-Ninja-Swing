using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Persistent : MonoBehaviour {

    public float Timer { get; set; }

    public string LevelToBeLoaded { get; set; }
    public string LevelToBeEdited { get; set; }
    public string LevelJsonToBeTested { get; set; }

    public string LoggedInUser { get; set; }

    // TODO
    public string[] CampaignLevels { get; set; }

    [Header("DataLayer data")]
    public string ApiKey;
    public string DataLayerUrl;

	void Start () {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("MainMenu");

        // Load maps in database table. Top 10 levels or so, manually inserted and updated.   CampaignLevels
        // Separate Highscore for these levels. Total highscore.
	}

    
}
