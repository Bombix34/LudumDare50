using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MiniGame : MonoBehaviour
{
    [SerializeField] public string gameName;

    protected GameState currentGameState = GameState.PAUSE;
    public int CurrentDifficulty { get; set; } = 1;

    [SerializeField] private bool isDebugMode = false;

    private void Awake()
    {
        if (isDebugMode)
            currentGameState = GameState.RUNNING;
    }

    public void Launch()
    {
        currentGameState = GameState.RUNNING;
    }

    protected abstract void Win();
    protected abstract void Lose();

    protected virtual void EndGame()
    {
        GameManager.OnEndMiniGame?.Invoke(this);
    }

    public bool IsGameRunning { get => currentGameState == GameState.RUNNING; }

    public bool IsWinning { get => currentGameState == GameState.WIN; }

}

public enum GameState
{
    PAUSE,
    RUNNING,
    WIN,
    LOSE
}
