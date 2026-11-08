//Maria Tumidajewicz, Maciej Tuleja
using UnityEngine;

public enum GameState 
{
    [InspectorName("Gameplay")] GAME,
    [InspectorName("Pause")] PAUSE_MENU,
    [InspectorName("Level completed (either successfully or failed)")] LEVEL_COMPLETED
}

public class GameMenager : MonoBehaviour
{
    public static GameMenager Instance { get; private set; }
    
    public GameState currentGameState = GameState.PAUSE_MENU;
    private int score = 0;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
          
        }
        else
        {
            Debug.LogError("Duplicated Game Manager", gameObject);
        }
    }

    public void AddPoints(int points)
    {
        score += points;
        Debug.Log("Score: " + score);
    }



    void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
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
        SetGameState(GameState.LEVEL_COMPLETED);
    }

    public void GameOver()
    {
        SetGameState(GameState.LEVEL_COMPLETED);
    }                                                                                                   


}
