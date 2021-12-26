using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts.ConstantsAndEnums;

public class GameManager : MonoBehaviour
{
    public bool isFpsVisible;
    public bool isMobileMode = true;

    public GameObject Dpad;
    public GameObject FireButton;

    [SerializeField] private Text fpsText;
    [SerializeField] private float hudRefreshRate = 1f;

    private float timer;
    private int score = 0;

    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;

    public static GameManager Instance;

    public GameObject startPage;
    public GameObject gameOverPage;
    public GameObject inGamePage;
    public GameObject pausePage;
    public List<Text> scoreText;
    public List<Text> highScoreText;

    private GameState _state = GameState.Start;
    public GameState State
    {
        get
        {
            return _state;
        }

        private set
        {
            _state = value;

            switch (_state)
            {
                case GameState.None:
                    startPage.SetActive(false);
                    gameOverPage.SetActive(false);
                    pausePage.SetActive(false);
                    inGamePage.SetActive(false);
                    Time.timeScale = 0;
                    break;

                case GameState.Start:
                case GameState.GameOver:
                    startPage.SetActive(true);
                    gameOverPage.SetActive(false);
                    pausePage.SetActive(false);
                    inGamePage.SetActive(false);
                    Time.timeScale = 1;
                    break;

                case GameState.Running:
                    startPage.SetActive(false);
                    gameOverPage.SetActive(false);
                    inGamePage.SetActive(true);
                    pausePage.SetActive(false);
                    Time.timeScale = 1;
                    break;

                case GameState.Paused:
                    startPage.SetActive(false);
                    gameOverPage.SetActive(false);
                    inGamePage.SetActive(false);
                    pausePage.SetActive(true);
                    Time.timeScale = 0;
                    break;

                default:
                    break;
            }
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void Awake()
    {
        Debug.Log("GM awakens");
        Instance = this;
        SetHighScore();

        fpsText.enabled = isFpsVisible;
        Dpad.SetActive(isMobileMode);
        FireButton.SetActive(isMobileMode);

        State = GameState.Start;
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape pressed: " + State.ToString());
            if(State == GameState.Paused)
            {
                State = GameState.Running;
            }
            else if (State == GameState.Running)
            {
                State = GameState.Paused;
            }
        }
    }

    private void OnPlayerDied() 
    {
        SetHighScore();

        State = GameState.GameOver;
    }

    private void SetHighScore()
    {
        int savedScore = PlayerPrefs.GetInt("HighScore");

        if (score > savedScore)
        {
            savedScore = score;
            PlayerPrefs.SetInt("HighScore", score);
        }

        var highScore = "High Score: " + savedScore.ToString();
        highScoreText.ForEach(x => x.text = highScore);
    }


    private void OnAsteroidDestroyed() 
    {
        score++;
        scoreText.ForEach( x => x.text = score.ToString());
    }

    public void ConfirmGameOver()
    {
        // Activated when replay button is hit;
        OnGameOverConfirmed(); // event
        scoreText.ForEach(x => x.text = "0");
        State = GameState.Start;
    }

    public void StartGame()
    {
        // Activated when play button is hit;
        State = GameState.Running;
        score = 0;
        scoreText.ForEach( x=> x.text = score.ToString());
        OnGameStarted();
    }

    public void ResumeGame()
    {
        State = GameState.Running;
    }

}
