using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Collections.Generic;

public class HeroUnit : MonoBehaviour, IPausable
{
    public event Action OnHealthChanged;
    public event Action OnDeath;
    public event Action OnRevive;
    
    [field: SerializeField] public HeroData Data { get; private set; }


    [Header("Components")]
    [SerializeField] private Rigidbody2D _rb2d;
    [SerializeField] private Transform _abilityChild;
    [SerializeField] private List<Collider2D> _colliders;
    public PathfindingModule PathfindingModule;

    [SerializeField] private HeroVisuals _visuals;
    private BaseAbility _ability;
    public BaseAbility Ability { get => _ability; private set => _ability = value; }
    public Rigidbody2D Rigidbody { get => _rb2d;}
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
    public int MaxHealth { get { return HeroManager.Stats.MaxHealth.FinalWithAdditive(this, Data.BaseMaxHealth); } }
    public bool IsDead { get; private set; } = false;
    private float MovementSpeed { get { return HeroManager.Stats.MovementSpeed.Final(this); } }
    private float _respawnHealth = 0;

    private void Update()
    {
        if (GameManager.Instance.IsPaused) return;
        
        // General Update stuff
        
        PathfindingModule.SetMaxSpeed(MovementSpeed);
        
        
        // When dead update stuff
        if (IsDead)
        {
            _respawnHealth += (MaxHealth / HeroManager.Stats.RespawnTime.Final(this)) * Time.deltaTime;
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
        
    }

    public void Init(HeroData data)
    {
        if (data.IsUnityNull())
            throw new ArgumentNullException("data", "Can't initilize a unit with null data");

        Data = data;
        gameObject.name = $"Hero - {Data.Name}";
        CurrentHealth = MaxHealth;
        _visuals.Initialize(this);
        
        // Init ability component
        if(Data.AbilityData != null)
        {
            Ability = Data.AbilityData.CreateAbilityComponent(_abilityChild);
            Ability.Init(Data.AbilityData, this);
        }
        
        PathfindingModule.SetMaxSpeed(MovementSpeed);
        PathfindingModule.SetMaxAcceleration(1000);
        HeroManager.Instance.RegisterHero(this);
        
        GameManager.Instance.RegisterPausable(this);
            
    }

    
    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        _visuals.OnHit();
        if (CurrentHealth <= 0)
        {
            SetDeath(true);
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
            col.enabled = !isDead;
        }

        if (isDead)
        {
            _visuals.OnDeath();
        }
        else
        {
            _visuals.OnRevive();
        }
        
        if(isDead)
            OnDeath?.Invoke();
        else
            OnRevive?.Invoke();
    }
    
    
    public void Pause()
    {
        // Set speed to zero
        _rb2d.linearVelocity = Vector2.zero;
        PathfindingModule.PausePathfinding();    }

    public void Resume()
    {
        PathfindingModule.ResumePathfinding();
    }
}
