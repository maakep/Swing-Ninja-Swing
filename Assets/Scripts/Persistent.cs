using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Persistent : MonoBehaviour {


    public string LevelToBeLoaded { get; set; }
    public float Timer { get; set; }
    public string LevelToBeEdited { get; set; }


    public string[] CampaignLevels { get; set; }

	void Start () {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("MainMenu");

        // Load maps in database table. Top 10 levels or so, manually inserted and updated.   CampaignLevels
        // Separate Highscore for these levels. Total highscore.
	}
}
