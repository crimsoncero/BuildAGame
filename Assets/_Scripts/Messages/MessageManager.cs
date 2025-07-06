using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SeraphRandom;
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
    [SerializeField] private bool _sendMessageWhileDead = true;
    private float _timeToNextMessage;
    
    private Dictionary<HeroData, ShuffleBag<string>> _messageBagsDict { get; } = new Dictionary<HeroData, ShuffleBag<string>>();
    
    private void Start()
    {
        GameManager.Instance.OnGameStart += Initialize;
        
    }

    public void Initialize()
    {
        GameManager.Instance.OnGameStart -= Initialize;
        SetTimeToNextMessage();
        foreach (var hero in HeroManager.Instance.Heroes)
        {
            var messageList = hero.Data.Messages.Messages;
            var bag = new ShuffleBag<string>(messageList.Count);
            foreach (var message in messageList)
                bag.Add(message);
            
            _messageBagsDict.Add(hero.Data, bag);
        }
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
            var index = Random.Range(0, _messageBagsDict.Count);
            var (hero, bag) = _messageBagsDict.ElementAt(index);
            
            if (!_sendMessageWhileDead)
            {
                while(HeroManager.Instance.Heroes.Find(h => h.Data == hero).IsDead)
                {
                    index = Random.Range(0, _messageBagsDict.Count);
                    (hero, bag) = _messageBagsDict.ElementAt(index);
                }
                    
            }
            
            _message.PopRandomMessage(hero,bag.Pick(), _messageDuration);
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
