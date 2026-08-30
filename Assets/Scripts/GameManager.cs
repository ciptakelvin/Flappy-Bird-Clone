using TMPro;

using UnityEngine;

public class GameManager : MonoBehaviour
{
    const string highScoreKey = "HighScore";

    // A reference to the Game Over text UI element, managed centrally by the game brain.
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text gameOver;
    [SerializeField] private GameObject playButton;
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject obstacleSpawner;

    public int Score
    {
        get; private set;
    }

    public int HighScore
    {
        get; private set;
    }

    [SerializeField] private TMP_Text scoreCounter;

    [SerializeField] private GameObject scoreLabel;
    [SerializeField] private TMP_Text lastScoreLabel;
    [SerializeField] private TMP_Text bestScoreLabel;

    // The static reference to this specific instance. 
    // 'public static' means ANY script can type `GameManager.Instance` to talk to it.
    // 'private set' prevents other scripts from accidentally wiping out or overwriting this reference.
    public static GameManager Instance
    {
        get; private set;
    }

    void Awake()
    {
        // Enforcement of the Singleton Pattern: 
        // If an Instance already exists somewhere else, and it isn't THIS specific component...
        if (Instance != null && Instance != this)
        {
            // ...then this copy is a duplicate. Destroy it immediately to prevent multiple managers conflicting.
            Destroy(gameObject);
            return;
        }

        // If no other instance exists, officially claim the global throne.
        Instance = this;

        HighScore = PlayerPrefs.GetInt(highScoreKey, 0);
    }

    void Start()
    {
        Time.timeScale = 0f;

        playButton.SetActive(true);
        title.gameObject.SetActive(true);
        gameOver.gameObject.SetActive(false);
        scoreLabel.SetActive(false);
        scoreCounter.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        // Clears out the static pointer if this specific object is removed.
        // This prevents "Ghost/Null References" when switching or reloading scenes, preventing memory leaks.
        if (Instance == this)
            Instance = null;
    }

    public void AddScore()
    {
        Score++;
        scoreCounter.text = Score.ToString();

        player.Score();
    }

    // A centralized function that handles the complete state transition into a Game Over.
    public void GameOver()
    {
        // Freezes the game simulation completely (stops physics updates and DeltaTime movements)
        Time.timeScale = 0f;

        player.Die();

        // Reveals the Game Over canvas element/text onto the screen
        gameOver.gameObject.SetActive(true);
        playButton.SetActive(true);

        UpdateHighScore();

        scoreLabel.SetActive(true);
        lastScoreLabel.text = $"Last Score: {Score}";
        bestScoreLabel.text = $"Best Score: {HighScore}";

        scoreCounter.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        Time.timeScale = 1f;

        title.gameObject.SetActive(false);
        gameOver.gameObject.SetActive(false);
        playButton.SetActive(false);
        scoreLabel.SetActive(false);

        player.ResetPlayer();

        ClearObstacle();

        ResetScore();

        scoreCounter.gameObject.SetActive(true);
    }

    private void ClearObstacle()
    {
        foreach (var o in obstacleSpawner.GetComponentsInChildren<ObstacleMovement>())
        {
            Destroy(o.gameObject);
        }
    }

    private void UpdateHighScore()
    {
        if (Score <= HighScore)
            return;

        HighScore = Score;
        PlayerPrefs.SetInt(highScoreKey, HighScore);
        PlayerPrefs.Save();
    }

    private void ResetScore()
    {
        Score = 0;
        scoreCounter.text = "0";
    }
}