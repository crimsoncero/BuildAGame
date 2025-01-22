using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyUnit : MonoBehaviour
{
    [field: SerializeField] public EnemyData Data { get; private set; }


    [Header("Components")]
    [SerializeField] private Rigidbody2D _rb2d;
    public CircleCollider2D Collider;
    public PathfindingModule PathfindingModule;
    [SerializeField] private float MaxAcceleration = -100f;

    
    private ObjectPool<EnemyUnit> _pool;

    // STATS::
    public int CurrentHealth { get; private set; }

    // Stats computational properties (if complicated, use an intermediary method)
    public int MaxHealth { get { return Data.BaseMaxHealth; } }
    public float MaxSpeed { get { return Data.BaseMaxSpeed; } }
    public int Power { get { return Data.BasePower; } }


   

    public void Initialize(EnemyData data, Vector3 position, ObjectPool<EnemyUnit> pool)
    {
        if (data.IsUnityNull())
            throw new ArgumentNullException("data", "Can't initilize a unit with null data");

        _pool = pool;

        Data = data;
        gameObject.name = $"{Data.name}";

        gameObject.transform.position = position;

        PathfindingModule.SetMaxSpeed(MaxSpeed);
        PathfindingModule.SetMaxAcceleration(MaxAcceleration);



        // Set Target
        PathfindingModule.SetTarget(PlayerController.Instance.Heroes[0].transform);
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
