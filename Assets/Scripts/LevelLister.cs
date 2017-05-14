using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

public class LevelLister : MonoBehaviour {

	// Use this for initialization
	void Start () {
       
        // TODO: Support for this in DBlayer.php
        DataLayer.GetAllLevels(
            (list) => {
                LevelList[] levels = JsonConvert.DeserializeObject<LevelList[]>(list);
                Generate(levels);
            }
        );
	}

    private void Generate(LevelList[] levels)
    {
        foreach (var level in levels)
        {
            // TODO: do this
        }
    }

}

class LevelList
{
    public string Username { get; set; }
    public string LevelName { get; set; }
    public string Level { get; set; }
}

/* LevelList[] =
*
*
* [{"Username":"usr","LevelName":"asd","Level":"{some: more, json: hehe}"},{"Username":"aboo","LevelName":"asdasdasd","Level":"{somaae: more, json: hehe}"}]
* 
* 
 */
