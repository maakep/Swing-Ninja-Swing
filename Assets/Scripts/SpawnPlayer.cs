using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SpawnPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (SceneManager.GetActiveScene().name != "LevelEditor")
        {
            Instantiate(Resources.Load("Character"), transform.position, Quaternion.identity);
        }
	}
}
