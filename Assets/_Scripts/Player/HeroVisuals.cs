using System;
using UnityEngine;

public class HeroVisuals : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    private HeroUnit _hero;
    private Vector3 _position;
    public void Initialize(HeroUnit hero)
    {
        _hero = hero;
        _animator = Instantiate(_hero.Data.VisualsPrefab, transform);
        _animator.speed = 0;
        _position = transform.position;
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
}
