using System;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEditor;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    private static string LEVEL_INIT_DATA_PATH = "Assets/_SO/DO NOT TOUCH/Level Init Data.asset";

    
    public event Action OnGameStart;
    public event Action OnGameEnd;
    public event Action OnGamePaused;
    public event Action OnGameResumed;
    /// <summary>
    /// Perform an action when the timer ticks a second, use only when totally needed.
    /// </summary>
    public event Action<int> OnTimerTick;

   [SerializeField] private SceneTransitionManager _sceneTransitionManager;

    [field: SerializeField] public LevelInitData InitData { get; private set; }

    /// <summary>
    /// The in game time that has passed until now, counted in seconds.
    /// </summary>
    public int Timer { get; private set; } = 0;
    private float _timerSeconds;

    /// <summary>
    /// Is the game currently paused.
    /// </summary>
    public bool IsPaused { get; private set; } = true;
    public bool IsGameActive { get; private set; } = false;

    
    private List<IPausable> _pausablesList = new List<IPausable>();

    private void Update()
    {
        UpdateTimer();
    }

    public void RegisterPausable(IPausable obj)
    {
        if (_pausablesList.Contains(obj))
            return;
        _pausablesList.Add(obj);
    }

    public void UnregisterPausable(IPausable obj)
    {
        _pausablesList.Remove(obj);
    }
    public void StartGame()
    {
        LevelManager.Instance.Init(InitData.LevelData);
        
        XPManager.Instance.OnLevelUp += LevelUp;
        
        IsGameActive = true;
        IsPaused = false;
        Timer = 0;
        _timerSeconds = 0;
        // Cursor.visible = false;

        OnTimerTick?.Invoke(Timer);
        OnGameStart?.Invoke();
    }

    /// <summary>
    /// Pause all the stuff in the game (projectiles, movement, animation, EVERYTHING)
    /// </summary>
    public void PauseGame(bool isPlayerPause = false)
    {
        // Already Paused or game not active
        if (IsPaused && !IsGameActive) return;
        
        if (isPlayerPause)
        {
            UIManager.Instance.OpenPauseMenu();
        }
        else
        {
            
        }
        
        
        IsPaused = true;
        Cursor.visible = true;
        
        OnGamePaused?.Invoke();
        // Pause all pausables and remove those that are null.
        for (int i = 0; i < _pausablesList.Count; i++)
        {
            if(_pausablesList[i] == null)
                _pausablesList.RemoveAt(i);
            else
                _pausablesList[i].Pause();
        }
    }

    /// <summary>
    /// Resume executing all the things in the game that were paused.
    /// </summary>
    public void ResumeGame()
    {
        // Already resumed or game not active
        if (!IsPaused || !IsGameActive) return;
        
        IsPaused = false;
        Cursor.visible = false;
        
        OnGameResumed?.Invoke();
        // Pause all pausables and remove those that are null.
        for (int i = 0; i < _pausablesList.Count; i++)
        {
            if(_pausablesList[i] == null)
                _pausablesList.RemoveAt(i);
            else
                _pausablesList[i].Resume();
        }
    }

    public void GameOver()
    {
        PauseGame();
        OnGameEnd?.Invoke();
        XPManager.Instance.OnLevelUp -= LevelUp;
        
        MMPlaylistStopEvent.Trigger(0);
        UIManager.Instance.OpenEndScreen();
    }

    private void LevelUp()
    {
        if (HeroManager.Instance.IsFullyUpgraded)
        {
            return;
        }

        PauseGame();
        UIManager.Instance.OpenUpgradeMenu();;
    }


    private void UpdateTimer()
    {
        if (!IsPaused && IsGameActive)
        {
            _timerSeconds += Time.deltaTime;
            if(_timerSeconds > 1)
            {
                _timerSeconds -= 1;
                Timer++;
                OnTimerTick?.Invoke(Timer);
            }
        }
    }
    public void LoadMenu()
    {
        _sceneTransitionManager.LoadScene(LevelSceneEnum.Menu);
    }

    
}
