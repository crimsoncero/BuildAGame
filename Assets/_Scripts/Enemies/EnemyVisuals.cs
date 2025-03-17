using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyVisuals : MonoBehaviour
{
    private static readonly int DestroyPropID = Shader.PropertyToID("_Destroy");
    private static readonly int DamagePropID = Shader.PropertyToID("_DamageSlider");

    [SerializeField] private float _deathDuration = 0.5f;
    [SerializeField] private float _hitDuration = 0.2f;

    [Header("Walk Animation")] 
    [SerializeField] private float _walkAnimSpeed = 0.2f;
    [SerializeField, Range(0f,1f)] private float _verticalScaleChange = 0.7f;
    
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private EnemyUnit _enemy;
    
    private Tweener _hitTween;
    private Tweener _walkTween;    
    private Tweener _deathTween;

   

    private void Start()
    {
        _spriteRenderer.material = new Material(LevelManager.Instance.Data.UnitMaterial);
        
        
    }

    public void Initialize(EnemyUnit enemy)
    {
        _enemy = enemy;
        _spriteRenderer.sprite = _enemy.Data.Sprite;
        _spriteRenderer.material.SetFloat(DestroyPropID, 1f);
        _spriteRenderer.material.SetFloat(DamagePropID, 0f);
        
        GameManager.Instance.OnGamePaused += OnPause;
        GameManager.Instance.OnGameResumed += OnResume;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsPaused)
        {
            WalkAnimation(_enemy.PathfindingModule.AIPath.velocity.magnitude > 0);
        }

    }

    public void OnHit()
    {
        if (_hitTween.IsActive() && _hitTween.IsPlaying()) return;
        _hitTween = _spriteRenderer.material.DOFloat(1, DamagePropID, _hitDuration / 2f).SetEase(Ease.InOutBounce).SetLoops(2, LoopType.Yoyo);
    }

    public void OnDeath()
    { 
        _deathTween = _spriteRenderer.material.DOFloat(0, DestroyPropID, _deathDuration).OnComplete(AfterDeathAnimation);      
        
    }

    private void AfterDeathAnimation()
    {
        GameManager.Instance.OnGamePaused -= OnPause;
        GameManager.Instance.OnGameResumed -= OnResume;

        _enemy.gameObject.SetActive(false);
    }
    private void WalkAnimation(bool isWalking)
    {
        if (isWalking)
        {
            if (_walkTween.IsActive() && _walkTween.IsPlaying()) return; 
            float adjustedWalkSpeed = _walkAnimSpeed + Random.Range(-0.1f, 0.1f);
            _walkTween = transform.DOScaleY(_verticalScaleChange, adjustedWalkSpeed).SetEase(Ease.InOutBounce).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            if (Mathf.Approximately(transform.localScale.y, 1f)) return;
            _walkTween = transform.DOScaleY(1f, _walkAnimSpeed).SetEase(Ease.InOutBounce);
        }
    }

    private void OnPause()
    {
        if (_hitTween.IsActive())
            _hitTween.Pause();
        if (_walkTween.IsActive())
            _walkTween.Pause();
        if (_deathTween.IsActive())
            _deathTween.Pause();
    }

    private void OnResume()
    {
        if (_hitTween.IsActive())
            _hitTween.Play();
        if (_walkTween.IsActive())
            _walkTween.Play();
        if (_deathTween.IsActive())
            _deathTween.Play();
    }
}
