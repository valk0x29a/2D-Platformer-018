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
    public TMP_Text scoreLabel1;
    public TMP_Text highscoreLabel;
    public Canvas pauseMenuCanvas;
    public Canvas levelCompletedCanvas;
    public TMP_Text timerLabel;
    public Image[] keysTab;
    public GameObject[] hearts;
    public TMP_Text EnemyLabel;
    public TMP_Text CongratulationsLabel;


    private int score = 0;
    private int eagles = 0;
    private int livesNum = 3;
    private float timer = 0;
    public int keysFound = 0;
    public int maxKeysNumber = 3;
    private bool keysCompleted = false;

    string keyHighScore = "HighScoreLevel1";


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            scoreLabel.text = score.ToString();
            EnemyLabel.text = eagles.ToString();
            InGame();
            for (int i = 0; i < maxKeysNumber; i++)
            {
                keysTab[i].color = Color.grey;
            }

            if (!PlayerPrefs.HasKey(keyHighScore))
            {
                PlayerPrefs.SetInt(keyHighScore, 0);
            }
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


    public void AddEnemies(int eagleCount)
    {
        eagles += eagleCount;
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

    public void AddKeys(int keyIndex)
    {
        keysFound++;
        switch (keyIndex)
        {
            case 0:
                keysTab[keyIndex].color = Color.blue;
                break;
            case 1:
                keysTab[keyIndex].color = Color.red;
                break;
            case 2:
                keysTab[keyIndex].color = Color.green;
                break;
        }
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

        if(currentGameState == GameState.LEVEL_COMPLETED)
        {
           Scene currentScene = SceneManager.GetActiveScene();
            Debug.Log("Aktywna scena to: '" + currentScene.name + "'"); // Pomoc w sprawdzeniu nazwy
            if (currentScene.name == "Level 1")
            {
                int highScore = PlayerPrefs.GetInt(keyHighScore, 0);
                if (score > highScore)
                {
                    PlayerPrefs.SetInt(keyHighScore, score);
                    PlayerPrefs.Save();
                }
            }

            scoreLabel1.text = "Your score: " + score.ToString();
            highscoreLabel.text = "The best score: " + PlayerPrefs.GetInt(keyHighScore, 0).ToString();
            CongratulationsLabel.text = livesNum > 0 ? "Congratulations! Level Completed" : "Game Over! \n Try again";

        }
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
