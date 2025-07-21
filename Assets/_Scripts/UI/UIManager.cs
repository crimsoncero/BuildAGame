using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private HeroFrame _heroFramePrefab;
    [SerializeField] private Transform _heroFrameContainter;
    [SerializeField] private TMP_Text _timer;
    [SerializeField] private UpgradeMenu _upgradeMenu;
    [SerializeField] private MMF_Player _endScreenPlayerShow;
    [SerializeField] private PauseMenu _pauseMenu;
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

    public void OpenUpgradeMenu()
    {
        _upgradeMenu.ShowUpgradePanel();
    }

    public void OpenEndScreen()
    {
        _endScreenPlayerShow.PlayFeedbacks();
    }

    public void OpenPauseMenu()
    {
        _pauseMenu.Open();
    }
}
