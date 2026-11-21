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
    private int score = 0;
    private int livesNum = 3;

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

    public void AddPoints(int points)
    {
        score += points;
        Debug.Log("Score: " + score);
        scoreLabel.text = score.ToString();
    }

    public void AddLives(int lives)
    {
        livesNum += lives;
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
