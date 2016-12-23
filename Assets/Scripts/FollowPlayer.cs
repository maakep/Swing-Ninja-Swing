using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
    private GameObject _player;
    Vector3 playerPos; 

	// Use this for initialization
	public void Init () {
        _player = GameObject.FindGameObjectsWithTag("Player")[0];
	}

    void Start()
    {
        GetComponent<Camera>().orthographicSize = 12;
    }
	
	// Update is called once per frame
	void Update () {
        if (_player != null)
        {
            playerPos = new Vector3(_player.transform.position.x, _player.transform.position.y, -10);
            transform.position = playerPos;
        }
	}
}
