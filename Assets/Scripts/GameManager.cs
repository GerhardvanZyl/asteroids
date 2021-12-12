using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isFpsVisible;
    public bool isMobileMode = true;

    public GameObject Dpad;
    public GameObject FireButton;

    [SerializeField] private Text fpsText;
    [SerializeField] private float hudRefreshRate = 1f;

    private float timer;

    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;

    public static GameManager Instance;

    public GameObject startPage;
    public GameObject gameOverPage;
    public Text scoreText;
    public Text highScoreText;

    enum PageState
    {
        None,
        Start,
        GameOver
    }

    int score = 0;
    bool gameOver = false;

    public bool IsGameOver { get => gameOver; }

    private void Awake()
    {
        Debug.Log("GM awakens");
        Instance = this;
        SetHighScore();

        fpsText.enabled = isFpsVisible;
        Dpad.SetActive(isMobileMode);
        FireButton.SetActive(isMobileMode);        
    }

    private void OnEnable()
    {
        PlayerController.OnPlayerDied += OnPlayerDied;
        AsteroidController.OnAsteroidDestroyed += OnAsteroidDestroyed;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerDied -= OnPlayerDied;
        AsteroidController.OnAsteroidDestroyed -= OnAsteroidDestroyed;
    }

    void Update()
    {
        if (isFpsVisible && Time.unscaledTime > timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            fpsText.text = fps + " FPS on " + SceneManager.GetActiveScene().path;
            timer = Time.unscaledTime + hudRefreshRate;
        }
    }

    void SetPageState(PageState state)
    {
        switch(state)
        {
            case PageState.None:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                break;

            case PageState.Start:
                startPage.SetActive(true);
                gameOverPage.SetActive(false);
                break;

            case PageState.GameOver:
                startPage.SetActive(true);
                gameOverPage.SetActive(false);
                break;
        }
    }

    private void OnPlayerDied() 
    {
        gameOver = true;
        
        SetHighScore();

        SetPageState(PageState.GameOver);
    }

    private void SetHighScore()
    {
        int savedScore = PlayerPrefs.GetInt("HighScore");

        if (score > savedScore)
        {
            savedScore = score;
            PlayerPrefs.SetInt("HighScore", score);
        }

        highScoreText.text = "High Score: " + savedScore.ToString();
    }


    private void OnAsteroidDestroyed() 
    {
        score++;
        scoreText.text = score.ToString();
    }

    public void ConfirmGameOver()
    {
        // Activated when replay button is hit;
        OnGameOverConfirmed(); // event
        scoreText.text = "0";
        SetPageState(PageState.Start);
    }

    public void StartGame()
    {
        // Activated when play button is hit;

        SetPageState(PageState.None);
        gameOver = false;
        score = 0;
        scoreText.text = score.ToString();

        OnGameStarted();
    }

}
