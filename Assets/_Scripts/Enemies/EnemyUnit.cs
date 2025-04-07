using System;
using DG.Tweening;
using SeraphUtil.ObjectPool;
using Unity.VisualScripting;
using UnityEngine;
using IPoolable = SeraphUtil.ObjectPool.IPoolable;

public class EnemyUnit : MonoBehaviour, IPoolable, IPausable
{
    
    [field: SerializeField] public EnemyData Data { get; private set; }
    
    [Header("Components")]
    [SerializeField] private Rigidbody2D _rb2d;
    [SerializeField] private LayerMask _attackableLayers;
    [SerializeField] private EnemyVisuals _visuals;
    public CircleCollider2D Collider;
    public PathfindingModule PathfindingModule;
    
    [Header("Properties")]
    [SerializeField] private float _knockbackForce = 1f;
    [SerializeField] private float _knockbackDurationPerForce = 0.2f;
    
    private MultiPool<EnemyUnit> _pool;
    
    // STATS::
    public int CurrentHealth { get; private set; }

    // Stats computational properties (if complicated, use an intermediary method)
    public int MaxHealth { get { return Data.BaseMaxHealth; } }
    public float MoveSpeed { get { return Data.BaseMoveSpeed; } }
    public int Power { get { return Data.BasePower; } }
    public float Speed { get { return Data.BaseSpeed; } }
    
    private BoolTimer _canAttack;
    private bool _isDead = false;
    private Tween _knockbackTween;
    
    public bool IsAlive
    {
        get { return !_isDead; }
    }
    public void Initialize(EnemyData data, Vector3 position, MultiPool<EnemyUnit> pool)
    {
        if (data.IsUnityNull())
            throw new ArgumentNullException("data", "Can't initilize a unit with null data");

        _pool = pool;
        _isDead = false;
        Data = data;
        gameObject.name = $"{Data.name}";
        
        gameObject.transform.position = position;

        CurrentHealth = MaxHealth;
        _canAttack = new BoolTimer(true, Speed);
       
        
        
        
        if (data.IsBoss)
        {
            transform.localScale = Vector3.one * 3;
        }
        else
        {
            transform.localScale = Vector3.one;
        }
        PathfindingModule.ResumePathfinding();
        PathfindingModule.SetMaxSpeed(MoveSpeed);
        PathfindingModule.SetMaxAcceleration(1000);
        
        _visuals.Initialize(this);
        
        // Set Target
        PathfindingModule.SetTarget(PlayerController.Instance.Center);
        
        gameObject.SetActive(true);
    }

    
    private void OnDisable()
    {
        _pool?.Return(Data.Prefab,this);
    }
    private void Update()
    {
        if (!GameManager.Instance.IsPaused)
        {
            _canAttack.UpdateTimer();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!_canAttack.Value) return;
        if (_attackableLayers.Includes(collision.gameObject.layer))
        {
            if (PlayerController.Instance.DamageHero(collision.transform, Power))
            {
                _canAttack.SetTimer();
            }
        }
       
    }

    public void TakeDamage(int damage, Vector2 hitDireciton, bool isKnockback = false)
    {
        // Handle damage
        if (damage <= 0) return;
        CurrentHealth -= damage;
        _visuals.OnHit();

        if (isKnockback)
            Knockback(hitDireciton, _knockbackForce);
        
        // Check for dying
        if (CurrentHealth <= 0)
            KillUnit();
            
    }

    private void Knockback(Vector2 direction, float force)
    {
        if (Data.ImmuneToKnockback) return;
        
        PathfindingModule.PausePathfinding();
        Vector2 endDestination = (Vector2)transform.position + direction.normalized * force;
        
        if(_knockbackTween.IsActive())
            _knockbackTween.Kill();
        
        _knockbackTween = _rb2d.DOMove(endDestination, _knockbackDurationPerForce * _knockbackForce).SetEase(Ease.OutSine)
            .OnComplete(() => PathfindingModule.ResumePathfinding());
        
    }
    
    public void KillUnit(bool spawnGem = true)
    {
        if(_isDead) return;
        _isDead = true;
        
        if (spawnGem)
            XPManager.Instance.SpawnGem(Data.GemDropped, transform.position);
        
        _visuals.OnDeath();
        
    }

    public void OnTakeFromPool()
    {
        GameManager.Instance.RegisterPausable(this);
    }

    public void OnReturnToPool()
    {
        GameManager.Instance?.UnregisterPausable(this);
    }

    public void Pause()
    {
        // Set speed to zero
        _rb2d.linearVelocity = Vector2.zero;
        _rb2d.simulated = false;
        PathfindingModule.PausePathfinding();
    }

    public void Resume()
    {
        PathfindingModule.ResumePathfinding();
        _rb2d.simulated = true;
    }
}
