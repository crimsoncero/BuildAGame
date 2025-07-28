using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Feedbacks;
using UnityEngine;

public class HealVFX : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _healerVFX;
    [SerializeField] private MMF_Player _otherHeroesVFX;
    [SerializeField] private bool _isPlayingPaused;

    private List<MMF_Player> _healingEffects = new List<MMF_Player>();
    private void Start()
    {
        GameManager.Instance.OnGamePaused += OnPause;
        GameManager.Instance.OnGameResumed += OnResume;
    }

    public void Init(HeroUnit thisHero)
    {
        StartCoroutine(InitOtherHeroesVFX());
    }

    private IEnumerator InitOtherHeroesVFX()
    {
        yield return new WaitUntil(() => HeroManager.Instance.Heroes != null 
                                         && HeroManager.Instance.Heroes.Count == GameManager.Instance.InitData.HeroData.Count);
        
        foreach (var hero in HeroManager.Instance.Heroes)
        {
            var effect = Instantiate(_otherHeroesVFX, Vector3.zero, Quaternion.identity);
            effect.transform.SetParent(hero.transform);
            effect.gameObject.transform.localPosition = Vector3.zero;
            _healingEffects.Add(effect);
        }

        Stop();
    }
    public void Play()
    {
        foreach (var effect in _healingEffects)
        {
            effect.PlayFeedbacks();
        }

        foreach (var effect in _healerVFX)
        {
            effect.Play();
        }
    }
    
    public void Stop()
    {
        foreach (var effect in _healingEffects)
        {
            effect.StopFeedbacks();
        }
        
        foreach (var effect in _healerVFX)
        {
            effect.Stop();
        }
    }

    private void OnPause()
    {
        _isPlayingPaused = false;
        foreach (var effect in _healingEffects.Where(effect => effect.IsPlaying))
        {
            _isPlayingPaused = true;
            effect.PauseFeedbacks();
        }

        foreach (var effect in _healerVFX.Where(effect => effect.isPlaying))
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
            effect.ResumeFeedbacks();
        }
        foreach (var effect in _healerVFX)
        {
            effect.Play();
        }
    }
    
}
