using System.Collections.Generic;
using UnityEngine;

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
