using System;
using UnityEngine;

public enum GameState
{
    Gameplay,
    Inventory,
    Pause
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public GameState CurrentState { get; private set; }

    public event Action<GameState> OnGameStateChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        CurrentState = GameState.Gameplay;
    }

    public void SetState(GameState state)
    {
        if (CurrentState == state)
            return;

        CurrentState = state;
        OnGameStateChanged?.Invoke(state);
    }

    public bool IsGameplay()
    {
        return CurrentState == GameState.Gameplay;
    }
}