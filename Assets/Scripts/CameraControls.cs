using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {
    float speed = 2f;
    float step;

    float distance;
    Vector2 mousePos;

    public LayerMask mask;
    RaycastHit2D hit;
    GameObject selectedObject;
    Camera thisCamera;
    Vector2 offset;
    GameObject spawn;

    Persistent app;

    void Start()
    {
        thisCamera = GetComponent<Camera>();
        try
        {
            app = GameObject.Find("ApplicationManager").GetComponent<Persistent>();
        }
        catch (System.Exception)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("_app");
        }
        if (GameObject.Find("PlayerSpawn") == null && app.LevelToBeEdited == "")
        {
            spawn = (GameObject)Instantiate(Resources.Load("PlayerSpawn"));
            spawn.name = "PlayerSpawn";
        }
    }
	
	void Update () {

        mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Scroll to zoom
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        thisCamera.orthographicSize -= scroll*3;

        // Left click to select 
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            
            hit = Physics2D.Raycast(mousePos, Vector2.zero, mask);
            if (hit.collider != null)
            {
                selectedObject = hit.collider.gameObject;
                offset = mousePos - (Vector2)selectedObject.transform.position;
            }
        } // and move
        else if (Input.GetKey(KeyCode.Mouse0) && hit.collider != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                // Snap to grid
                var newX = Mathf.Round((mousePos.x / 5) * 5);
                var newY = Mathf.Round((mousePos.y / 5) * 5);
                selectedObject.transform.position = (new Vector2(newX, newY));
            }
            else
            {
                selectedObject.transform.position = (mousePos - offset);
            }
            
            

            if (Input.GetKeyDown(KeyCode.W))
            {
                selectedObject.transform.localScale = new Vector3(selectedObject.transform.localScale.x, selectedObject.transform.localScale.y + 1f, selectedObject.transform.localScale.z);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                selectedObject.transform.localScale = new Vector3(selectedObject.transform.localScale.x + 1f, selectedObject.transform.localScale.y, selectedObject.transform.localScale.z);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                selectedObject.transform.localScale = new Vector3(selectedObject.transform.localScale.x - 1f, selectedObject.transform.localScale.y, selectedObject.transform.localScale.z);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                selectedObject.transform.localScale = new Vector3(selectedObject.transform.localScale.x, selectedObject.transform.localScale.y - 1f, selectedObject.transform.localScale.z);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject != spawn)
            {
                Destroy(hit.collider.gameObject);
            }
            
        }
        
        if (Input.GetKey(KeyCode.Mouse2))
        {
            speed = 4f;
            distance = Vector3.Distance(mousePos, Camera.main.transform.position);

            speed += distance;
            step = speed * Time.deltaTime;
            
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(mousePos.x, mousePos.y, -10), step);
        }


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            var spawn = Instantiate(Resources.Load("Ground"), mousePos, Quaternion.identity);
            spawn.name = "Ground";
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            var spawn = Instantiate(Resources.Load("PlayerObjective"), mousePos, Quaternion.identity);
            spawn.name = "PlayerObjective";
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            var spawn = Instantiate(Resources.Load("DeathZone"), mousePos, Quaternion.identity);
            spawn.name = "DeathZone";
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {

        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {

        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {

        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {

        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {

        }

	}
}
