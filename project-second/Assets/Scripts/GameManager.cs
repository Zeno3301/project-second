using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public bool isGameOver;
    public int currentScore;
    public int highScore;

    [Header("Events")]
    public UnityEvent<int> onScoreChanged;
    public UnityEvent<int> onHighScoreChanged;
    public UnityEvent onGameOver;
    public UnityEvent onGameStart;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadHighScore();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        isGameOver = false;
        currentScore = 0;
        onScoreChanged?.Invoke(currentScore);
        onGameStart?.Invoke();
    }

    public void GameOver()
    {
        isGameOver = true;
        if (currentScore > highScore)
        {
            highScore = currentScore;
            SaveHighScore();
            onHighScoreChanged?.Invoke(highScore);
        }
        onGameOver?.Invoke();
    }

    public void AddScore(int points)
    {
        if (isGameOver) return;

        currentScore += points;
        onScoreChanged?.Invoke(currentScore);

        if (currentScore > highScore)
        {
            highScore = currentScore;
            SaveHighScore();
            onHighScoreChanged?.Invoke(highScore);
        }
    }

    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        onHighScoreChanged?.Invoke(highScore);
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
    }
} 