using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour
{
    public static ScoreHandler instance;
    public int score = 0;
    private Text _scoreText;

    void Awake()
    {
        _scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }      
    }

    void Start()
    {
        AddScore(0);
    }

    void Update()
    {
        if (_scoreText == null)
        {
            _scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
            _scoreText.text = score.ToString();
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        if (score > PlayerPrefs.GetInt("Highscore", 0))
        {
            PlayerPrefs.SetInt("Highscore", score);
        }
        _scoreText.text = score.ToString();
    }

    public void ResetScore()
    {
        score = 0;
    }
}
