using System;
using TMPro;
using UnityEngine;

public class TestUI : MonoBehaviour
{

    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _pauseText;


    void Start()
    {
        GameManager.Instance.OnTimerTick += UpdateTimerText;
        GameManager.Instance.OnGamePaused += OnPauseChange;
        GameManager.Instance.OnGameResumed += OnPauseChange;
    }

    private void UpdateTimerText(int timer)
    {
       _timerText.text = Helpers.SecondsToMMSS(timer);
    }

    private void OnPauseChange()
    {
        if(GameManager.Instance.IsPaused)
            _pauseText.gameObject.SetActive(true);
        else
            _pauseText.gameObject.SetActive(false);
    }
}
