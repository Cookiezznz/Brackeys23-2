using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct GameState
{
    public float costOfDamageCaused;
    public int objectsSmashed;
    public int floorsTraversed;
    public bool isVictorious;
    public float playTime;
}
public class GameStateManager : Singleton<GameStateManager>
{
    [field: SerializeField]
    public bool isPaused { get; private set; }

    [SerializeField]
    private GameState gameState;

    public bool isGameOver;
    public static event Action OnGameOver;
    public static event Action<bool> OnGamePaused;
    

    [Header("Update Components")]
    public PlayerController playerController;
    public HostileManager hostileManager;

    void OnEnable()
    {
        RoomManager.OnRoomsGenerated += StartGame;
        InputManager.onPause += PauseGame;
    }

    void OnDisable()
    {
        RoomManager.OnRoomsGenerated -= StartGame;
        InputManager.onPause -= PauseGame;
    }

    public void StartGame()
    {
        if(isPaused || Time.timeScale == 0) ResumeGame();
        gameState = new GameState();
        isGameOver = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused) return;
        GameUpdate();
    }

    void GameUpdate()
    {
        if(playerController)
            playerController.PlayerUpdate();
        if (hostileManager)
            hostileManager.UpdateHostiles();
    }

    public void PauseGame()
    {
        if(isPaused || isGameOver) return;
        Time.timeScale = 0;
        isPaused = true;
        OnGamePaused?.Invoke(true);
    }
    
    public void ResumeGame()
    {
        if (!isPaused || isGameOver) return;
        Time.timeScale = 1;
        isPaused = false;
        OnGamePaused?.Invoke(false);
    }

    public void GameOver(bool isVictory)
    {
        isGameOver = true;
        if (isVictory)
        {
            UpdateGameState( 0, 0, 0, Time.timeSinceLevelLoad, true); 
        }
        else
        {
            UpdateGameState( 0, 0, 0, Time.timeSinceLevelLoad);
        }

        OnGameOver?.Invoke();
    }

    public void UpdateGameState(int smashedObjectsToAdd, int floorsTraversed, float damageCostToAdd=0, float playTimeToAdd=0, bool isVictorious=false)
    {
        if (smashedObjectsToAdd > 0)
        {
            gameState.objectsSmashed += smashedObjectsToAdd;
        }
        if (floorsTraversed > 0)
        {
            gameState.floorsTraversed += floorsTraversed;
        }
        if (damageCostToAdd > 0)
        {
            gameState.costOfDamageCaused += damageCostToAdd;
        }

        if (playTimeToAdd > 0)
        {
            gameState.playTime += playTimeToAdd;
        }

        if (isVictorious)
        {
            gameState.isVictorious = true;
        }
    }

    public GameState GetGameState()
    {
        return gameState;
    }

}
