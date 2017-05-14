using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZoneScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (SceneManager.GetActiveScene().name == "LevelEditor")
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
	}
}
