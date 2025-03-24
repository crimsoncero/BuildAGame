using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealVFX : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _healingEffects;
    [SerializeField] private bool _isPlayingPaused;

    private void Start()
    {
        GameManager.Instance.OnGamePaused += OnPause;
        GameManager.Instance.OnGameResumed += OnResume;
    }

    public void Play()
    {
        foreach (var effect in _healingEffects)
        {
            effect.Play();
        }
    }
    
    public void Stop()
    {
        foreach (var effect in _healingEffects)
        {
            effect.Stop();
        }
    }

    private void OnPause()
    {
        _isPlayingPaused = false;
        foreach (var effect in _healingEffects.Where(effect => effect.isPlaying))
        {
            _isPlayingPaused = true;
            effect.Pause();
        }
    }

    private void OnResume()
    {
        if (_isPlayingPaused == false) return;
        foreach (var effect in _healingEffects)
        {
            effect.Play();
        }
    }
    
}
