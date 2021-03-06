﻿using UnityEngine;
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

            
            if (ExtensionMethods.GetAnyOfKeysDown(KeyCode.W, KeyCode.D, KeyCode.A, KeyCode.S) && notModifying)
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
        List<KeyCode> hotkeys = new List<KeyCode>()
        {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9,
            KeyCode.Alpha0,
        };

        foreach (var hotkey in hotkeys)
        {
            if (Input.GetKeyDown(hotkey))
            {
                if (hotkeys.IndexOf(hotkey) < EditorObjects.Length)
                {
                    var obj = EditorObjects[hotkeys.IndexOf(hotkey)];
                    var spawn = Instantiate(obj, mousePos, Quaternion.identity);
                    spawn.name = spawn.name.Replace("(Clone)", string.Empty);
                }
            }
        }
        #endregion
    }

    private bool notModifying = true;
    IEnumerator ModifyScale() {
        notModifying = false;
        Vector3 scaling = selectedObject.transform.localScale;
        int i = 0;
        float incrementor = 1f;
        float waitTime = 0.4f;

        while (ExtensionMethods.GetAnyOfKeys(KeyCode.W, KeyCode.D, KeyCode.A, KeyCode.S))
        {
            
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
                if (scaling.x > 0 && scaling.y > 0)
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

            yield return new WaitForSeconds(waitTime);
            waitTime = 0.1f;
        }
        notModifying = true;
    }
}
