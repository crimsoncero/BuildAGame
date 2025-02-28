using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private HeroFrame _heroFramePrefab;
    [SerializeField] private Transform _heroFrameContainter;
    [SerializeField] private TMP_Text _timer;

    private void Start()
    {
        GameManager.Instance.OnTimerTick += UpdateTimer;
    }

    public void AddHeroFrame(HeroUnit hero)
    {
        var frame = Instantiate(_heroFramePrefab, _heroFrameContainter);
        frame.Init(hero);
    }

    private void UpdateTimer(int time)
    {
        _timer.text = Helpers.SecondsToMMSS(time);
    }
}
