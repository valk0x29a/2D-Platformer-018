//Maria Tumidajewicz, Maciej Tuleja
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum GameState 
{
    [InspectorName("Gameplay")] GAME,
    [InspectorName("Pause")] PAUSE_MENU,
    [InspectorName("Level completed (either successfully or failed)")] LEVEL_COMPLETED
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public GameState currentGameState = GameState.PAUSE_MENU;
    public Canvas gameCanvas;
    public TMP_Text scoreLabel;
    public Canvas pauseMenuCanvas;
    public Canvas levelCompletedCanvas;
    public TMP_Text timerLabel;
    public GameObject[] hearts;
    public TMP_Text EnemyLabel;

    private int score = 0;
    private int eagles = 0;
    private int livesNum = 3;
    private float timer = 0;
    public int keysFound = 0;
    public int maxKeysNumber = 3;
    private bool keysCompleted = false;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            scoreLabel.text = score.ToString();
            InGame();
        }
        else
        {
            Debug.LogError("Duplicated Game Manager", gameObject);
        }
    }

    void Update()
    {
        if (currentGameState == GameState.GAME)
        {
            timer += Time.deltaTime;
            UpdateTimerUI();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentGameState == GameState.GAME)
            {
                PauseMenu();
            }
            else if (currentGameState == GameState.PAUSE_MENU)
            {
                InGame();
            }
        }
    }

    private void UpdateTimerUI()
    {
        int totalSeconds = Mathf.FloorToInt(timer);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        timerLabel.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void AddPoints(int points)
    {
        score += points;
        Debug.Log("Score: " + score);
        scoreLabel.text = score.ToString();
    }


    public void AddEnemies(int points)
    {
        eagles += points;
        EnemyLabel.text = eagles.ToString();
    } 


    public void AddLives(int lives)
    {
        livesNum = Mathf.Clamp(livesNum + lives, 0, 3);
        UpdateLivesUI();

        if (livesNum <= 0)
        {
            GameOver();
        }
       
    }

    public void AddKeys()
    {
        keysFound++;
        if (keysFound >= maxKeysNumber)
        {
            keysCompleted = true;
            Debug.Log("All keys found!");
        }
    }

    private void UpdateLivesUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < livesNum);
        }
    }

    void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
        gameCanvas.enabled = currentGameState == GameState.GAME;
        pauseMenuCanvas.enabled = currentGameState == GameState.PAUSE_MENU;
        levelCompletedCanvas.enabled = currentGameState == GameState.LEVEL_COMPLETED;
    }

    public void PauseMenu()
    {
        SetGameState(GameState.PAUSE_MENU);
    }

    public void InGame()
    {
        SetGameState(GameState.GAME);
    }

    public void LevelCompleted()
    {
        AddPoints(100 * livesNum);
        SetGameState(GameState.LEVEL_COMPLETED);
    }

    public void GameOver()
    {
        SetGameState(GameState.LEVEL_COMPLETED);
    }   

    public void OnResumeButtonClicked()
    {
        InGame();
    }            

    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }    

    public void OnReturnToMainMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }                                                                                


}
