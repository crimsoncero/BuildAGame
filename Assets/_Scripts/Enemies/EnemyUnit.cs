using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyUnit : MonoBehaviour
{
    [field: SerializeField] public EnemyData Data { get; private set; }
    
    [Header("Components")]
    [SerializeField] private Rigidbody2D _rb2d;
    [SerializeField] private LayerMask _attackableLayers;
    public CircleCollider2D Collider;
    public PathfindingModule PathfindingModule;
    
    
    private ObjectPool<EnemyUnit> _pool;

    // STATS::
    public int CurrentHealth { get; private set; }

    // Stats computational properties (if complicated, use an intermediary method)
    public int MaxHealth { get { return Data.BaseMaxHealth; } }
    public float MoveSpeed { get { return Data.BaseMoveSpeed; } }
    public int Power { get { return Data.BasePower; } }
    public float Speed { get { return Data.BaseSpeed; } }

    private BoolTimer _canAttack;
    public void Initialize(EnemyData data, Vector3 position, ObjectPool<EnemyUnit> pool)
    {
        if (data.IsUnityNull())
            throw new ArgumentNullException("data", "Can't initilize a unit with null data");

        _pool = pool;

        Data = data;
        gameObject.name = $"{Data.name}";

        gameObject.transform.position = position;

        CurrentHealth = MaxHealth;
        _canAttack = new BoolTimer(true, Speed);

        PathfindingModule.SetMaxSpeed(MoveSpeed);
        PathfindingModule.SetMaxAcceleration(1000);



        // Set Target
        PathfindingModule.SetTarget(PlayerController.Instance.Center);
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGamePaused += PauseEnemy;
        GameManager.Instance.OnGameResumed += ResumeEnemy;
    }
    private void OnDisable()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.OnGamePaused -= PauseEnemy;
            GameManager.Instance.OnGameResumed -= ResumeEnemy;
        }
       
        _pool?.Release(this);
    }
    private void Update()
    {
        if (!GameManager.Instance.IsPaused)
        {
            _canAttack.UpdateTimer();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
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

    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;
        Debug.Log("Taking damage " + damage);
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
            KillUnit();
            
    }

    public void KillUnit()
    {
        gameObject.SetActive(false);
    }

    #region Pause & Resume

    private void PauseEnemy()
    {
        // Set speed to zero
        _rb2d.linearVelocity = Vector2.zero;
        PathfindingModule.PausePathfinding();
    }

    private void ResumeEnemy()
    {
        PathfindingModule.ResumePathfinding();
    }

    

    #endregion
}
