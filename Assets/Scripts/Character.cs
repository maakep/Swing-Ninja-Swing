using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Scripts;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour {
    
    DistanceJoint2D joint;
    TrailRenderer trail;
    LineRenderer line;

    GameObject ingamePauseMenu;

    Vector3 targetPos;
    RaycastHit2D hit;
    public LayerMask mask;
    Rigidbody2D rb;
    Animator anim;


    private float _distance = 500;

    private float _moveX;
    private float _speed = 10f;

    private bool grounded;

    private GameObject _canvas;
    #region Boost
    private Image _boostImage;
    private bool _privateHasBoost;
    private bool _hasBoost
    {
        get
        {
            return _privateHasBoost;
        }
        set
        {
            _boostImage.enabled = value;
            _privateHasBoost = value;
        }
    }
    #endregion

    private List<int> hookedToArray;

    private const float JERK_COOLDOWN = 0.5f;
    private float _jerkTimestamp;

    private float _spaceClick;
    private const float BUNNYHOP_DURATION = 0.2f;

    private const string GROUND_TAG = "Ground";

    private Vector3 _startingPos;

    Vector3 prePauseVelocity;

    private float _timer = 0;

    Text timerText;


    enum States { 
        Playing = 1,
        Paused = 2,
    };

    States State { get; set; }

	void Start () {
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        
        rb = GetComponent<Rigidbody2D>();
        
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        line.startWidth = .1f;
        line.startColor = Color.green;

        trail = GetComponent<TrailRenderer>();

        anim = GetComponent<Animator>();

        _canvas = GameObject.FindGameObjectWithTag("MainCanvas");
        _boostImage = _canvas.transform.FindDeepChild("ActiveBoostImage").gameObject.GetComponent<Image>();

        ingamePauseMenu = _canvas.FindObject("PauseMenu");
        ingamePauseMenu.transform.FindChild("Continue").GetComponent<Button>().onClick.AddListener(() => { ResumeGame(); });
        ingamePauseMenu.transform.FindChild("Reset").GetComponent<Button>().onClick.AddListener(() => { ResumeGame(); StartCoroutine(ResetLevel()); });
        ingamePauseMenu.transform.FindChild("Exit").GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene("MainMenu"); });
        
        hookedToArray = new List<int>();

        Camera.main.GetComponent<FollowPlayer>().Init();
        _startingPos = transform.position;
        State = States.Playing;
        timerText = GameObject.Find("TimerText").GetComponent<Text>();
	}
	
	void Update () {
        if (State.Equals(States.Playing))
        {
            PlayerMovement();
            RotateTransformTowardDirection();
            CheckNinjaHook();
            CheckBoost();
            CheckJerk();
            UpdateAnimatorVariables();
            _timer += Time.deltaTime;
        }

        UpdateGUI();
        CheckForPauses();
	}

    private void CheckForPauses()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (State)
            {
                case States.Playing:
                    PauseGame();
                    break;
                case States.Paused:
                    ResumeGame();
                    break;
            }
        }
    }

    private void ResumeGame()
    {
        State = States.Playing;
        rb.isKinematic = false;
        rb.velocity = prePauseVelocity;
        TogglePauseMenu();
    }

    private void PauseGame()
    {
        State = States.Paused;
        prePauseVelocity = rb.velocity;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        TogglePauseMenu();
    }

    private bool _menuStateVisible = false;
    private void TogglePauseMenu()
    {
        _menuStateVisible = !_menuStateVisible;
        ingamePauseMenu.SetActive(_menuStateVisible);
    }
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == GROUND_TAG)
        {
            grounded = true;
        }

        if (_spaceClick > Time.time - BUNNYHOP_DURATION)
        {
            grounded = false;
            rb.AddForce(Vector2.up * 500f);
            if (rb.velocity.x >= 0) {
                rb.AddForce(Vector2.right * 450f);
            } else {
                rb.AddForce(Vector2.left * 450f);
            }
                

        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == GROUND_TAG)
        {
            grounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "DeathZone")
        {
            StartCoroutine(ResetLevel());
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "LifeZone")
        {
            StartCoroutine(ResetLevel());
        }
    }

    private IEnumerator ResetLevel()
    {
        line.enabled = false;
        joint.enabled = false;
        trail.time = 0;
        _hasBoost = true;

        transform.position = _startingPos;
        rb.velocity = Vector3.zero;

        hookedToArray.Clear();
        yield return new WaitForEndOfFrame();
        trail.time = 3;
    }


    private void RotateTransformTowardDirection()
    {
        if (rb.velocity.x < -0.5)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else if (rb.velocity.x > 0.5)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }

    private void PlayerMovement()
    {
        // Hotkey: Check movement keys (A/D) to control movement
        if (grounded)
        {
            _moveX = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(_moveX * _speed, rb.velocity.y);
        }

        // Hotkey: Space for jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!grounded)
                _spaceClick = Time.time;

            if (grounded)
            {
                rb.AddForce(Vector2.up * 500f);
            }       
        }
    }

    private void CheckBoost()
    {
        // Hotkey: Check right click to boost 
        if (Input.GetKeyDown(KeyCode.Mouse1) && joint.enabled)
        {
            if (_hasBoost)
            {
                rb.AddForce(rb.velocity * 100);
                _hasBoost = false;
            }
        }
    }

    private void CheckNinjaHook()
    {

        // Hotkey: Check left click to send ninja hook
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = 0;
            hit = Physics2D.Raycast(transform.position, targetPos - transform.position, _distance, mask);

            if (hit.collider != null && hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
            {
                joint.distance = Vector2.Distance(transform.position, hit.point);
                joint.connectedAnchor = hit.point;
                joint.enabled = true;
                line.SetPosition(1, hit.point);
                line.enabled = true;

                if (!hookedToArray.Contains(hit.collider.GetInstanceID()))
                {
                    hookedToArray.Add(hit.collider.GetInstanceID());
                    _hasBoost = true;
                }

            }
        }
        line.SetPosition(0, transform.position);

        // Hotkey: Check release left click to release ninja hook
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            line.enabled = false;
            joint.enabled = false;
        }
    }

    private void CheckJerk()
    {
        // Hotkey: W to jerk the rope
        if (Input.GetKeyDown(KeyCode.W) && joint.enabled)
        {
            if (hit && Time.time > _jerkTimestamp)
            {
                _jerkTimestamp = Time.time + JERK_COOLDOWN;
                Vector3 direction = hit.point - (Vector2)transform.position;
                rb.AddForce(direction.normalized * 1000f);
                //_hasBoost = false;
            }
        }
    }

    private void UpdateAnimatorVariables()
    {
        anim.SetBool("Running", (Mathf.Abs(rb.velocity.x) > 3f));
        anim.SetBool("Grounded", grounded);
    }

    void UpdateGUI()
    {
        timerText.text = "" + Mathf.Floor(_timer*100) / 100;
    }

    void OnDestroy()
    {
        GameManager.Timer = _timer;
    }

}
