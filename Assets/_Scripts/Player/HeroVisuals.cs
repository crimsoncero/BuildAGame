using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class HeroVisuals : MonoBehaviour
{
    private static readonly int DestroyPropID = Shader.PropertyToID("_Destroy");
    private static readonly int DamagePropID = Shader.PropertyToID("_DamageSlider");
    
    [SerializeField] private Animator _animator;
    
    [Header("Tween Settings")]
    [SerializeField] private float _deathDuration = 0.5f;
    [SerializeField] private float _hitDuration = 0.2f;
    
    private HeroUnit _hero;
    private Vector3 _position;
    private List<SpriteRenderer> _rendererList;
    private Material _material;
    private Tweener _hitTween;
    private Tweener _deathTween;

    public void Initialize(HeroUnit hero)
    {
        _hero = hero;
        _animator = Instantiate(_hero.Data.VisualsPrefab, transform);
        
        // Get all the renderer components in the children of the visual.
        _rendererList = _animator.GetComponentsInChildren<SpriteRenderer>().ToList();
        
        // Set a global material for all the renderers of the hero.
        _material = new Material(_hero.Data.MaterialPrefab);
        foreach (SpriteRenderer ren in _rendererList)
            ren.material = _material;
        
        
        _material.SetFloat(DestroyPropID, 1f);
        _material.SetFloat(DamagePropID, 0f);
        
        _animator.speed = 0;
        _position = transform.position;
        
        GameManager.Instance.OnGamePaused += OnPause;
        GameManager.Instance.OnGameResumed += OnResume;
    }

    private void Update()
    {
        HandleDirection();
        SetSpeed();
    }

    private void HandleDirection()
    {
        var scale = transform.localScale;
        if (_hero.PathfindingModule.AIPath.velocity.x > 0.5f)
        {
            scale.x = 1;
        }
        else if (_hero.PathfindingModule.AIPath.velocity.x < 0.5f)
        {
            scale.x = -1;
        }

        transform.localScale = scale;

    }

    private void SetSpeed()
    {
        if (_hero.PathfindingModule.AIPath.velocity.magnitude > 0.1f)
        {
            _animator.speed = 1;
        }
        else
        {
            _animator.speed = 0;
        }
    }

    public void OnHit()
    {
        if (_hitTween.IsActive() && _hitTween.IsPlaying()) return;
        _hitTween = _material.DOFloat(1, DamagePropID, _hitDuration / 2f).SetEase(Ease.InOutBounce).SetLoops(2, LoopType.Yoyo);
    }
    
    public void OnDeath()
    { 
        _deathTween = _material.DOFloat(0, DestroyPropID, _deathDuration).SetEase(Ease.OutSine).OnComplete(AfterDeathAnimation);      
        
    }
    
    private void AfterDeathAnimation()
    {
        GameManager.Instance.OnGamePaused -= OnPause;
        GameManager.Instance.OnGameResumed -= OnResume;

        gameObject.SetActive(false);
    }

    public void OnRevive()
    {
        gameObject.SetActive(true);
        
        _deathTween = _material.DOFloat(1, DestroyPropID, _deathDuration * 2f).SetEase(Ease.InQuad); 
        GameManager.Instance.OnGamePaused += OnPause;
        GameManager.Instance.OnGameResumed += OnResume;
    }
    
    private void OnPause()
    {
        if (_hitTween.IsActive())
            _hitTween.Pause();
        if (_deathTween.IsActive())
            _deathTween.Pause();
    }

    private void OnResume()
    {
        if (_hitTween.IsActive())
            _hitTween.Play();
        if (_deathTween.IsActive())
            _deathTween.Play();
    }
}
