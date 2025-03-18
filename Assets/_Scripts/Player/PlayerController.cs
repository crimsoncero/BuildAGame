using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using static UnityEngine.InputSystem.InputAction;
using System.Linq;
using UnityEngine.InputSystem;
using SeraphRandom;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private CinemachineCamera _cinemachineCamera;
    [SerializeField] private Transform _heroParent;
    [SerializeField] private HeroMover _heroMover;
    [SerializeField] private PlayerInput _input;

    [Header("Data")]
    [SerializeField] private int _numberOfUpgrades = 3;

    [field: SerializeField] public float TimeToRespawn { get; private set; } = 2f;
    
    public List<HeroUnit> Heroes { get; private set; }
    public Transform Center { get { return _heroMover.transform; } }
    public Dictionary<int, BaseAbility> AbilitiesDict { get; private set; }
    
    public bool IsFullyUpgraded { get; private set; }
    public Vector2 CenterPosition { get; private set; } = Vector2.zero;
    
    
    private void Start()
    {
        _cinemachineCamera.Follow = _heroMover.transform;
        InitHeroes(); // Move this to game manager when init creates heroes.
    }

    private void Update()
    {
        CalculateCenterPosition();
    }

    public void InitHeroes()
    {
        // TODO - create the heroes using the data of which heroes selected and the stats.


       
    }

    public void RegisterHero(HeroUnit hero)
    {
        if (Heroes == null)
            Heroes = new List<HeroUnit>();
        if (AbilitiesDict == null)
            AbilitiesDict = new Dictionary<int, BaseAbility>();
        
        Heroes.Add(hero);
        hero.OnDeath += CheckGameOver;
        hero.PathfindingModule.SetTarget(_heroMover.transform);
        AbilitiesDict.Add(AbilitiesDict.Count, hero.Ability);
        UIManager.Instance.AddHeroFrame(hero);
    }
    #region Player Input Methods
    public void OnMove(CallbackContext context)
    {
    //    if (GameManager.Instance.IsPaused) return; // Cant move while paused;

        Vector2 moveVec = context.ReadValue<Vector2>();
        _heroMover.Move(moveVec);
    }

    public void OnInteract(CallbackContext context)
    {
        if (GameManager.Instance.IsPaused) return; // Can't interact while paused.


        // Interact with stuff.
    }

    public void OnPause(CallbackContext context)
    {
        if (context.started)
        {
            if(!GameManager.Instance.IsPaused)
                GameManager.Instance.PauseGame();
            else // TODO - Remove this later, only for debugging atm, exiting pause will be from the pause menu or after selecing an upgrade.
                GameManager.Instance.ResumeGame();
        }
    }

    public void OnDeviceLost()
    {
        GameManager.Instance.PauseGame();
    }

    #endregion

    public bool DamageHero(Transform heroHit, int damage)
    {
        // Negative Damage
        if (damage < 0) return false;

        HeroUnit hero = Heroes.Find((h) => h.gameObject == heroHit.gameObject);
        if(hero == null) return false; // Didn't hit a hero.

        hero.TakeDamage(damage);
        return true;
    }

    public List<BaseAbility> GetUpgradesToShow()
    {
        var infoList = new List<BaseAbility>();
        var d = AbilitiesDict.Where((t) => t.Value.CurrentLevel < t.Value.MaxLevel);
        
        Dictionary<int, BaseAbility> upgradableDict = new Dictionary<int, BaseAbility>();
        
        foreach(var p in d)
        {
            upgradableDict.Add(p.Key, p.Value);
        }

        ShuffleBag<int> bag = new ShuffleBag<int>(upgradableDict.Keys.ToList());

        int numOfUpgrades = Mathf.Min(_numberOfUpgrades, upgradableDict.Count);
        for(int i = 0; i < numOfUpgrades; i++)
        {
            int x = bag.Pick();
            BaseAbility abilityData = upgradableDict[x];
            infoList.Add(abilityData);
        }

        // Check if it's the final upgrade
        if (numOfUpgrades == 1 && infoList[0].CurrentLevel + 1 == infoList[0].MaxLevel)
        {
            IsFullyUpgraded = true;
        }
        
        if (numOfUpgrades < 1)
        {
            throw new Exception("No upgrades available and not marked Fully Upgraded");
        }
        
        return infoList;
    }

    public HeroUnit GetClosestHero(Vector3 position)
    {
        HeroUnit closestHero = null;
        var closestDistance = Mathf.Infinity;

        foreach (var hero in Heroes)
        {
            var distance = Vector3.Distance(hero.transform.position, position);
            if (distance < closestDistance)
            {
                closestHero = hero;
                closestDistance = distance;
            }
        }
        
        return closestHero;
    }
    
    private void CheckGameOver()
    {
        bool allDead = true;
        foreach (var hero in Heroes)
        {
            if (!hero.IsDead)
                allDead = false;
        }

        if (allDead)
        {
            GameManager.Instance.GameOver();
        }
    }

    private void CalculateCenterPosition()
    {
        float sumX = 0;
        float sumY = 0;

        foreach (var hero in Heroes)
        {
            sumX += hero.transform.position.x;
            sumY += hero.transform.position.y;
        }

        var v = CenterPosition;
        v.x = sumX / Heroes.Count;
        v.y = sumY / Heroes.Count;
        CenterPosition = v;
        
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(CenterPosition, 0.2f);
    }
}
