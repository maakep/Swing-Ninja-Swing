using Assets.Scripts;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLister : MonoBehaviour {

    private GameObject _contentArea;
    public GameObject Button;

	// Use this for initialization
	void Start () {
        _contentArea = GameObject.Find("Content");
        GameObject.Find("BackButton").GetComponent<Button>().onClick.AddListener(LoadMainMenu);

        StartCoroutine(
            DataLayer.GetAllLevels(
                (list) =>
                {
                    Debug.Log(list);
                    LevelList[] levels = JsonConvert.DeserializeObject<LevelList[]>(list);
                    Generate(levels);
                }
            )
        );

	}

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void Generate(LevelList[] levels)
    {
        foreach (var level in levels)
        {
            var btn = Instantiate(Button, _contentArea.transform);
            // TODO: Create multiple text components for name and username
            btn.transform.GetChild(0).GetComponent<Text>().text = level.Name + " [by: " + level.Username + "]";
            btn.GetComponent<Button>().onClick.AddListener(() => LoadLevel(level.SerializedLevel));
            _contentArea.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 30);
        }
    }

    private void LoadLevel(string level)
    {
        GameManager.LevelToBeLoaded = level;
        SceneManager.LoadScene("LoadLevel");
    }

}

class LevelList
{
    public string Username { get; set; }
    public string Name { get; set; }
    public string SerializedLevel { get; set; }
}

/* LevelList[] =
*
*
* [{"Username":"usr","LevelName":"asd","Level":"{some: more, json: hehe}"},{"Username":"aboo","LevelName":"asdasdasd","Level":"{somaae: more, json: hehe}"}]
* 
* 
 */
