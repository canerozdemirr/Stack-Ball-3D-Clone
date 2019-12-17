using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody _rb;
    private float _overpowerBuildUp;
    public GameObject splashEffect;
    [SerializeField] private bool _isClicked, _isOverPowered;
    [SerializeField] private float _moveSpeed = 500f;
    private float _speedLimit = 5f;
    [SerializeField] private float _bounceSpeed = 250f;

    public enum PlayerState
    {
        Prepare,
        Play,
        Dead,
        Finish
    }

    public PlayerState playerState = PlayerState.Prepare;

    public AudioClip bounceClip, deadClip, breakClip, winClip, _overpowerBreakClip;

    private int currentBrokenPlatforms, totalPlatforms;

    public GameObject _overpowerBar;
    public Image _overpowerFill;
    public GameObject _fireEffect;
    public GameObject winEffect;


    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        currentBrokenPlatforms = 0;
    }

    void Start()
    {
        totalPlatforms = FindObjectsOfType<PlatformController>().Length;
    }

    void Update()
    {
        if (playerState == PlayerState.Play)
        {
            ClickCheck();
            OverpowerCheck();
        }

        if (playerState == PlayerState.Finish)
        {
            if (Input.GetMouseButtonDown(0))
            {
                FindObjectOfType<LevelSpawner>().IncreaseTheLevel();
            }
        }
    }

    void FixedUpdate()
    {
        BallMovement();
    }

    private void BallMovement()
    {
        if (playerState == PlayerState.Play)
        {
            if (Input.GetMouseButton(0) && _isClicked == true)
            {
                _rb.velocity = new Vector3(0, -_moveSpeed * Time.fixedDeltaTime, 0);
            }
        }

        if (_rb.velocity.y > _speedLimit)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _speedLimit, _rb.velocity.z);
        }
    }

    public void ClickCheck()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isClicked = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isClicked = false;
        }
    }

    public void IncreaseScore()
    {
        currentBrokenPlatforms++;
        if (!_isOverPowered)
        {
            ScoreHandler.instance.AddScore(1);
            SoundManager.instance.PlaySoundEffect(breakClip, .5f);
        }
        else
        {
            ScoreHandler.instance.AddScore(2);
            SoundManager.instance.PlaySoundEffect(_overpowerBreakClip, .5f);
        }
    }

    void OnCollisionEnter(Collision target)
    {
        if (!_isClicked)
        {
            _rb.velocity = new Vector3(0, _bounceSpeed * Time.deltaTime, 0);
            if (!target.gameObject.CompareTag("Finish"))
            {
                GameObject splash = Instantiate(splashEffect);
                splash.transform.SetParent(target.transform);
                splash.transform.localEulerAngles= new Vector3(90, Random.Range(0, 359), 0);
                float randomScale = Random.Range(0.18f, 0.25f);
                splash.transform.localScale = new Vector3(randomScale, randomScale, 1);
                splash.transform.position =
                    new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z);
                splash.GetComponent<SpriteRenderer>().color = GetComponent<MeshRenderer>().material.color;
            }
            SoundManager.instance.PlaySoundEffect(bounceClip, .5f);
        }
        else
        {
            if (_isOverPowered)
            {
                if (target.gameObject.tag == "GoodPlatform" || target.gameObject.tag == "BadPlatform")
                {
                    target.transform.parent.GetComponent<PlatformController>().BreakAllPlatforms();
                }
            }
            else
            {
                if (target.gameObject.tag == "GoodPlatform")
                {
                    target.transform.parent.GetComponent<PlatformController>().BreakAllPlatforms();
                }

                if (target.gameObject.tag == "BadPlatform")
                {
                    _rb.isKinematic = true;
                    transform.GetChild(0).gameObject.SetActive(false);
                    playerState = PlayerState.Dead;
                    SoundManager.instance.PlaySoundEffect(deadClip, .5f);
                }
            }
        }

        FindObjectOfType<GameUI>().LevelSliderFill(currentBrokenPlatforms / (float) totalPlatforms);

        if (target.gameObject.CompareTag("Finish") && playerState == PlayerState.Play)
        {
            SoundManager.instance.PlaySoundEffect(winClip, 1f);
            playerState = PlayerState.Finish;
            GameObject win = Instantiate(winEffect);
            win.transform.SetParent(Camera.main.transform);
            win.transform.localPosition = Vector3.up * 1.5f;
            win.transform.eulerAngles = Vector3.zero;
        }
    }

    void OnCollisionStay(Collision target)
    {
        if (!_isClicked || target.gameObject.CompareTag("Finish"))
            _rb.velocity = new Vector3(0, _bounceSpeed * Time.deltaTime, 0);
    }

    void OverpowerCheck()
    {
        if (_isOverPowered)
        {
            _overpowerBuildUp -= Time.deltaTime * .3f;
            if (!_fireEffect.activeInHierarchy)
                _fireEffect.SetActive(true);
        }          
        else
        {
            if (_fireEffect.activeInHierarchy)
                _fireEffect.SetActive(false);
            if (_isClicked)
                _overpowerBuildUp += Time.deltaTime * .8f;
            else
                _overpowerBuildUp -= Time.deltaTime * .5f;
        }

        if(_overpowerBuildUp >= 0.3f || _overpowerFill.color == Color.red)
            _overpowerBar.SetActive(true);
        else
            _overpowerBar.SetActive(false);

        if (_overpowerBuildUp >= 1)
        {
            _overpowerBuildUp = 1;
            _isOverPowered = true;
            _overpowerFill.color = Color.red;
        }
        else if (_overpowerBuildUp <= 0)
        {
            _overpowerBuildUp = 0;
            _isOverPowered = false;
            _overpowerFill.color = Color.white;
        }

        if (_overpowerBar.activeInHierarchy)
            _overpowerFill.fillAmount = _overpowerBuildUp;
    }

    
}
