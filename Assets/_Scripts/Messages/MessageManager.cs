using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MessageManager : MonoBehaviour
{
    [SerializeField] private Message _message;
    
    [Header("Settings")]
    [SerializeField] private float _messageDuration;
    [SerializeField, Tooltip("How much time between each message")] private float _messageTimer;
    [SerializeField, Tooltip("A variance range around the timer, to have some randomness")] private float _messageTimerVariance;
    [SerializeField] private float _timerLowerBound = 1f;
    
    private float _timeToNextMessage;
    
    private void Start()
    {
        GameManager.Instance.OnGameStart += Initialize;
        
    }

    public void Initialize()
    {
        GameManager.Instance.OnGameStart -= Initialize;
        SetTimeToNextMessage();
        
    }

    private void Update()
    {
        if (!GameManager.Instance.IsPaused && GameManager.Instance.IsGameActive)
        {
            _timeToNextMessage -= Time.deltaTime;
            if (_timeToNextMessage <= 0)
            {
                PopMessage();
            }
            
        }
    }

    private void PopMessage()
    {
        if (_message.IsActive)
        {
            // Message is still active, delay popping by a bit.
            _timeToNextMessage += 0.5f;
        }
        else
        {
            HeroData hero = HeroManager.Instance.Heroes.GetRandom().Data;
            _message.PopRandomMessage(hero, _messageDuration);
            SetTimeToNextMessage();
        }
    }
    
    private void SetTimeToNextMessage()
    {
        _timeToNextMessage = _messageTimer + Random.Range(-_messageTimerVariance, _messageTimerVariance) + _messageDuration;
        
        // Can't be a negative duration.
        if(_timeToNextMessage <= 0)
            _timeToNextMessage = _timerLowerBound + _messageDuration;
        
    }
}
