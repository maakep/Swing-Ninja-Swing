using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerObjective : MonoBehaviour {

    //Persistent app;

	// Use this for initialization
	void Start () {
        /*try
        {
            app = GameObject.Find("ApplicationManager").GetComponent<Persistent>();
        }
        catch (System.Exception)
        {
            SceneManager.LoadScene("_app");
        }*/
	}
	
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            Debug.Log("Win level");
            if (SceneManager.GetActiveScene().name == "TestLevel")
            {
                SceneManager.LoadScene("LevelEditor");
            }
            else
            {
                // TODO: Load "level completed" and add to highscore
                SceneManager.LoadScene("WinScreen");
            }
        }
    }
}
