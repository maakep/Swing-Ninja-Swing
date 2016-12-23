using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerObjective : MonoBehaviour {

    Persistent app;

	// Use this for initialization
	void Start () {
        GameObject.Find("ApplicationManager").GetComponent<Persistent>();
	}
	
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            Debug.Log("Win level");
            SceneManager.LoadScene("MainMenu");
            // TODO: Load "level completed" and add to highscore
        }
    }
}
