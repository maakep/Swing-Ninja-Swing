using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

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

    GameObject[] EditorObjects;

    void Start()
    {
        EditorObjects = Resources.LoadAll<GameObject>("EditorBlocks");

        thisCamera = GetComponent<Camera>();

        if (GameObject.Find("PlayerSpawn") == null && GameManager.LevelToBeEdited == "")
        {
            spawn = (GameObject)Instantiate(Resources.Load("PlayerSpawn"));
            spawn.name = "PlayerSpawn";
        }
    }
	
	void Update () {

        mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        #region Scroll to zoom
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        thisCamera.orthographicSize -= scroll * 3;
        #endregion

        #region Left click
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
                // Snap to grid - BUGGY
                var newX = Mathf.Round((mousePos.x / 5) * 5);
                var newY = Mathf.Round((mousePos.y / 5) * 5);
                selectedObject.transform.position = (new Vector2(newX, newY));
            }
            else
            {
                selectedObject.transform.position = (mousePos - offset);
            }

            
            if (ExtensionMethods.GetAnyOfKeysDown(KeyCode.W, KeyCode.D, KeyCode.A, KeyCode.S))
            {
                StartCoroutine(ModifyScale());
            }
        }
#endregion

        #region Right click
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject != spawn)
            {
                Destroy(hit.collider.gameObject);
            }
        }
#endregion

        #region Middle mouse button
        if (Input.GetKey(KeyCode.Mouse2))
        {
            speed = 4f;
            distance = Vector3.Distance(mousePos, Camera.main.transform.position);

            speed += distance;
            step = speed * Time.deltaTime;
            
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(mousePos.x, mousePos.y, -10), step);
        }
#endregion

        #region Number buttons
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            var spawn = Instantiate(EditorObjects[0], mousePos, Quaternion.identity);
            spawn.name = spawn.name.Replace("(Clone)", string.Empty);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            var spawn = Instantiate(EditorObjects[1], mousePos, Quaternion.identity);
            spawn.name = spawn.name.Replace("(Clone)", string.Empty);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            var spawn = Instantiate(EditorObjects[2], mousePos, Quaternion.identity);
            spawn.name = spawn.name.Replace("(Clone)", string.Empty);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            var spawn = Instantiate(EditorObjects[3], mousePos, Quaternion.identity);
            spawn.name = spawn.name.Replace("(Clone)", string.Empty);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            var spawn = Instantiate(EditorObjects[4], mousePos, Quaternion.identity);
            spawn.name = spawn.name.Replace("(Clone)", string.Empty);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {

        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {

        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {

        }
#endregion
    }

    IEnumerator ModifyScale() {
        Vector3 scaling = selectedObject.transform.localScale;

        if (Input.GetKeyDown(KeyCode.W))
        {
            scaling.y += 1f;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            scaling.x += 1f;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            scaling.x -= 1f;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            scaling.y -= 1f;
        }
        selectedObject.transform.localScale = scaling;
        yield return new WaitForSeconds(0.4f);

        int i = 0;
        float incrementor = 1f;
        while (ExtensionMethods.GetAnyOfKeys(KeyCode.W, KeyCode.D, KeyCode.A, KeyCode.S))
        {
            yield return new WaitForSeconds(0.1f);
            if (Input.GetKey(KeyCode.W))
            {
                scaling.y += incrementor;
            }
            if (Input.GetKey(KeyCode.D))
            {
                scaling.x += incrementor;
            }
            if (Input.GetKey(KeyCode.A))
            {
                scaling.x -= incrementor;
            }
            if (Input.GetKey(KeyCode.S))
            {
                scaling.y -= incrementor;
            }

            if (selectedObject != null)
            {
                selectedObject.transform.localScale = scaling;
                
                i++;

                if (i % 20 == 0)
                {
                    incrementor *= 2;
                }
            }
            else
            {
                break;
            }
        }
    }
}
