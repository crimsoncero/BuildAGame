using Unity.VisualScripting;
using UnityEngine;
using System;

public class HeroUnit : MonoBehaviour
{
    public event Action OnHealthChanged;
    
    
    [field: SerializeField] public HeroData Data { get; private set; }


    [Header("Components")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Rigidbody2D _rb2d;
    public PathfindingModule PathfindingModule;

    // STATS::
    private int _currentHealth;
    public int CurrentHealth
    {
        get { return _currentHealth; }
        set
        {
            _currentHealth = Math.Clamp(value, 0, MaxHealth);
            OnHealthChanged?.Invoke();
        }
    }

    // Stats computational properties (if complicated, use an intermediary method)
    public int MaxHealth { get { return Data.BaseMaxHealth; } }
    public float MoveSpeed { get { return Data.BaseMoveSpeed; } }
    public int Power { get { return Data.BasePower; } }
    public float Speed { get { return Data.BaseSpeed; } }
    public float Cooldown { get { return Data.BaseCooldown; } }
    public int Recovery { get { return Data.BaseRecovery; } }



    private void Start()
    {
        // Init the unit automatically if starting with data. (for testing mainly)
        if (!Data.IsUnityNull())
            Init(Data);
    }

    private void Update()
    {
        // Flip sprite according to velocity X
        if (PathfindingModule.AIPath.velocity.x < -1f)
        {
            _spriteRenderer.flipX = true;
        }
        else if (PathfindingModule.AIPath.velocity.x > 1f)
        {
            _spriteRenderer.flipX = false;
        }
    }

    public void Init(HeroData data)
    {
        if (data.IsUnityNull())
            throw new ArgumentNullException("data", "Can't initilize a unit with null data");

        Data = data;
        InitData();

        PathfindingModule.SetMaxSpeed(MoveSpeed);
        PathfindingModule.SetMaxAcceleration(1000);

        AddCallbacks();
    }

    private void AddCallbacks()
    {
        GameManager.Instance.OnGamePaused += PauseHero;
        GameManager.Instance.OnGameResumed += ResumeHero;
    }

    private void InitData()
    {
        gameObject.name = $"Hero - {Data.Name}";
        _spriteRenderer.sprite = Data.Sprite;
        CurrentHealth = MaxHealth;
    }
    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            Debug.Log($"{Data.Name} died");
        }
    }
    #region Pause & Resume

    private void PauseHero()
    {
        // Set speed to zero
        _rb2d.linearVelocity = Vector2.zero;
        PathfindingModule.PausePathfinding();
    }

    private void ResumeHero()
    {
        PathfindingModule.ResumePathfinding();
    }

   


    #endregion


}
