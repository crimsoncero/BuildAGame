using System;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public event Action OnGameStart;
    public event Action OnGameEnd;
    public event Action OnGamePaused;
    public event Action OnGameResumed;
    /// <summary>
    /// Perform an action when the timer ticks a second, use only when totally needed.
    /// </summary>
    public event Action<int> OnTimerTick;

    /// <summary>
    /// The in game time that has passed until now, counted in seconds.
    /// </summary>
    public int Timer { get; private set; } = 0;
    /// <summary>
    /// Is the game currently paused.
    /// </summary>
    public bool IsPaused { get; private set; } = false;


    private IEnumerator TickTimer()
    {
        // Wait until the game is not paused. Timer doesn't advance in paused state.
        yield return new WaitUntil(() => !IsPaused);

        yield return new WaitForSeconds(1);

        Timer++;

        OnTimerTick?.Invoke(Timer);
    } 

    public void StartGame()
    {
        TickTimer();
     
        OnGameStart?.Invoke();
    }

    public void EndGame()
    {
        OnGameEnd?.Invoke();
    }

    /// <summary>
    /// Pause all the stuff in the game (projectiles, movement, animation, EVERYTHING)
    /// </summary>
    public void PauseGame()
    {
        IsPaused = true;
        OnGamePaused?.Invoke();
    }

    /// <summary>
    /// Resume executing all the things in the game that were paused.
    /// </summary>
    public void ResumeGame()
    {
        IsPaused = false;
        OnGameResumed?.Invoke();
    }
}
