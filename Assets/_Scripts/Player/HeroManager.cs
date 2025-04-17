using System;
using System.Collections.Generic;
using System.Linq;
using SeraphRandom;
using Unity.Cinemachine;
using UnityEngine;

public class HeroManager : Singleton<HeroManager>
{
    public static HeroStats Stats
    {
        get { return Instance._stats; }
    }
    
    
    [Header("Hero Spawn")]
    [SerializeField] private HeroUnit _heroPrefab;
    [SerializeField] private float _spawnDistance = 1f;
    
    
    [Header("Components")]
    [SerializeField] private CinemachineCamera _cinemachineCamera;
    [SerializeField] private Transform _heroParent;
    [SerializeField] private HeroMover _heroMover;
    
    [Header("Data")]
    [SerializeField] private HeroBaseStats _heroBaseStats;
    
    
    
    // Stats
    private HeroStats _stats;  
    
    
    
    public List<HeroUnit> Heroes { get; private set; }
    public Transform Center { get { return _heroMover.transform; } }
    public Dictionary<int, BaseAbility> AbilitiesDict { get; private set; }
    public bool IsFullyUpgraded { get; private set; }
    public Vector2 CenterPosition { get; private set; } = Vector2.zero;


    private void Start()
    {
        _cinemachineCamera.Follow = _heroMover.transform;
        _stats = new HeroStats(_heroBaseStats);
        SpawnHeroes();
    }

    private void Update()
    {
        CalculateCenterPosition();
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

        int numOfUpgrades = Mathf.Min(Stats.UpgradeCount.Final(Heroes[0]), upgradableDict.Count);
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
            if (hero.IsDead) continue;
            
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

    private void SpawnHeroes()
    {
        var heroes = GameManager.Instance.InitData.HeroData;
        var points = Helpers.GetEqualOrbitLocations(heroes.Count, _spawnDistance);
        
        for (int i = 0; i < heroes.Count; i++)
        {
            Vector3 pos = points[i].position;
            HeroUnit hero = Instantiate(_heroPrefab, pos, Quaternion.identity, _heroParent);
            hero.Init(heroes[i]);
        }
    }
}
