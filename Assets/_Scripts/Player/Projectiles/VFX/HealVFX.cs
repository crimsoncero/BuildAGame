using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealVFX : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _healerVFX;
    [SerializeField] private ParticleSystem _otherHeroesVFX;
    [SerializeField] private bool _isPlayingPaused;

    private List<ParticleSystem> _healingEffects = new List<ParticleSystem>();
    private void Start()
    {
        GameManager.Instance.OnGamePaused += OnPause;
        GameManager.Instance.OnGameResumed += OnResume;
    }

    public void Init(HeroUnit thisHero)
    {
        _healingEffects.AddRange(_healerVFX);
        foreach (var hero in HeroManager.Instance.Heroes)
        {
            var effect = Instantiate(_otherHeroesVFX, Vector3.zero, Quaternion.identity);
            effect.transform.SetParent(hero.transform);
            _healingEffects.Add(effect);
        }
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
