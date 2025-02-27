using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Collections.Generic;

public class HeroUnit : MonoBehaviour
{
    public event Action OnHealthChanged;
    
    
    [field: SerializeField] public HeroData Data { get; private set; }


    [Header("Components")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Rigidbody2D _rb2d;
    [SerializeField] private Transform _abilityChild;
    [SerializeField] private Transform _visuals;
    [SerializeField] private List<Collider2D> _colliders;
    public PathfindingModule PathfindingModule;

    private BaseAbility _ability;
    public BaseAbility Ability { get => _ability; private set => _ability = value; }
    
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
    public bool IsDead { get; private set; } = false;

    private float _respawnHealth = 0;
    private void Start()
    {
        // Init the unit automatically if starting with data. (for testing mainly)
        if (!Data.IsUnityNull())
            Init(Data);

        PlayerController.Instance.RegisterHero(this);
    }

    private void Update()
    {
        if (GameManager.Instance.IsPaused) return;
        
        // General Update stuff
        
        // When dead update stuff
        if (IsDead)
        {
            _respawnHealth += (MaxHealth / PlayerController.Instance.TimeToRespawn) * Time.deltaTime;
            if (_respawnHealth >= 1f)
            {
                var hpToAdd = Mathf.FloorToInt(_respawnHealth);
                _respawnHealth -= hpToAdd;
                CurrentHealth += hpToAdd;
            }

            if (CurrentHealth >= MaxHealth)
            {
                SetDeath(false);
            }
           
            return;
        }
        
        // When alive update stuff
        
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

        // Init ability component
        if(Data.AbilityData != null)
        {
            Ability = Data.AbilityData.CreateAbilityComponent(_abilityChild);
            Ability.Init(Data.AbilityData, this);
        }
        

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
            SetDeath(true);
            Debug.Log($"{Data.Name} died");
        }
    }

    public void Heal(int healAmount, bool isPercentile = false)
    {
        if (healAmount <= 0) return;
        if (isPercentile)
        {
            CurrentHealth += Mathf.CeilToInt((healAmount / 100f) * MaxHealth);
        }
        else
        {
            CurrentHealth += healAmount;
        }
    }

    private void SetDeath(bool isDead)
    {
        IsDead = isDead;
        foreach (var col in _colliders)
        {
            col.enabled = false;
        }
        _visuals.gameObject.SetActive(!isDead);
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
