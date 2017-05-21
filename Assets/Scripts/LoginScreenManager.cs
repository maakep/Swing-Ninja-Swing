using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginScreenManager : MonoBehaviour {

    Canvas suCanvas;
    Canvas lgCanvas;

    Canvas currentStateCanvas;

    Text signupFeedbackText;
    Text loginFeedbackText;

    EventSystem system;

    SuUserPass suUserPass;
    LgUserPass lgUserPass;    

    public struct SuUserPass
    {
        public InputField Username { get; set; }
        public InputField Password { get; set; }
    }

    public struct LgUserPass
    {
        public InputField Username { get; set; }
        public InputField Password { get; set; }
    }

	void Start () {

        suCanvas = GameObject.Find("SignupCanvas").GetComponent<Canvas>();
        suCanvas.enabled = false;
        lgCanvas = GameObject.Find("LoginCanvas").GetComponent<Canvas>();
        
        signupFeedbackText = suCanvas.transform.FindChild("FeedbackText").GetComponent<Text>();
        loginFeedbackText = lgCanvas.transform.FindChild("FeedbackText").GetComponent<Text>();

        suUserPass = new SuUserPass();
        suUserPass.Username = suCanvas.transform.FindChild("SignupUsernameField").GetComponent<InputField>();
        suUserPass.Password = suCanvas.transform.FindChild("SignupPasswordField").GetComponent<InputField>();

        lgUserPass = new LgUserPass();
        lgUserPass.Username = lgCanvas.transform.FindChild("LoginUsernameField").GetComponent<InputField>();
        lgUserPass.Password = lgCanvas.transform.FindChild("LoginPasswordField").GetComponent<InputField>();

        suCanvas.transform.FindChild("SignupSignupButton").GetComponent<Button>().onClick.AddListener(SignupSubmit);
        suCanvas.transform.FindChild("SignupBackButton").GetComponent<Button>().onClick.AddListener(Back);

        lgCanvas.transform.FindChild("LoginLoginButton").GetComponent<Button>().onClick.AddListener(LoginSubmit);
        lgCanvas.transform.FindChild("LoginBackButton").GetComponent<Button>().onClick.AddListener(Back);
        lgCanvas.transform.FindChild("LoginSignupButton").GetComponent<Button>().onClick.AddListener(LoginSignup);

        system = EventSystem.current;
	}
    private void LoginSignup()
    {
        suCanvas.enabled = true;
        lgCanvas.enabled = false;
        currentStateCanvas = suCanvas;
    }

    private void Back()
    {
        if (suCanvas.enabled)
        {
            suCanvas.enabled = false;
            lgCanvas.enabled = true;
            currentStateCanvas = lgCanvas;
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void LoginSubmit()
    {
        var username = lgUserPass.Username.text;
        var password = lgUserPass.Password.text;

        if (ValidateInput(username, password))
        {
            // TODO: hash password
            StartCoroutine(DataLayer.LoginUser((cb, cb2) =>
            {
                if (cb2 == "Error" || cb2 != "true")
                {
                    Debug.Log(cb);
                    StartCoroutine(FeedbackText(cb));
                }
                else
                {
                    GameManager.LoggedInUser = cb;
                    Debug.Log("Welcome " + GameManager.LoggedInUser);
                    SceneManager.LoadScene("MainMenu");
                }
            }, username, password));
        }
    }

    private void SignupSubmit()
    {
        var username = suUserPass.Username.text;
        var password = suUserPass.Password.text;

        if (ValidateInput(username, password)) {
            StartCoroutine(DataLayer.CreateUser(cb => {
                if (cb != "error")
                {
                    StartCoroutine(FeedbackText(cb));
                }
            }, username, password));
        }
    }

    private bool ValidateInput(string user, string password)
    {
        var allowed = Regex.IsMatch(user, @"[a-zA-Z0-9]+") && Regex.IsMatch(password, @"[a-zA-Z0-9!#¤%&/()=@£$€{}]{5,}");
        if(!allowed){
            StartCoroutine(FeedbackText("Illegal characters. Please use [a-z, A-Z, 0-9]. Password has to be at least 5 characters"));
        }
        return allowed;
    }

    private IEnumerator FeedbackText(string text)
    {
        Text activeFeedbackText = (currentStateCanvas == suCanvas) ? signupFeedbackText : loginFeedbackText;
        activeFeedbackText.text = text;
        yield return new WaitForSeconds(7);
        activeFeedbackText.text = string.Empty;
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (currentStateCanvas == suCanvas)
            {
                SignupSubmit();
            }
            else
            {
                LoginSubmit();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (currentStateCanvas == suCanvas)
            {
                if (suUserPass.Username.isFocused)
                {
                    system.SetSelectedGameObject(suUserPass.Password.gameObject, new BaseEventData(system));
                }
                else
                {
                    system.SetSelectedGameObject(suUserPass.Username.gameObject, new BaseEventData(system));
                }   
            } else {
                if (lgUserPass.Username.isFocused)
                {
                    system.SetSelectedGameObject(lgUserPass.Password.gameObject, new BaseEventData(system));
                }
                else
                {
                    system.SetSelectedGameObject(lgUserPass.Username.gameObject, new BaseEventData(system));
                }
            }
        }
	}
}
