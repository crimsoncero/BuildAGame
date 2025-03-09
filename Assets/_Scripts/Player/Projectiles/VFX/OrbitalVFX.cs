using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrbitalVFX : MonoBehaviour, Pausable
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer.sprite = _sprites[Random.Range(0,_sprites.Count)];
    }
    public void Pause()
    {
        _particleSystem.Pause();

    }

    public void Resume()
    {
        _particleSystem.Play();
    }

  
}
