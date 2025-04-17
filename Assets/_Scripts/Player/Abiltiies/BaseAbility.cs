using System;
using UnityEngine;
using static BaseAbilityData;

public abstract class BaseAbility : MonoBehaviour
{
    public event Action OnLevelUp;

    public int CurrentLevel { get; private set; }

    protected BaseAbilityData _baseData;
    protected HeroUnit _heroUnit;


    protected HeroData _heroData
    {
        get { return _heroUnit.Data; }
    }

    protected BaseAbilityData.Stats _abilityStats
    {
        get { return _baseData.GetCurrentStats(CurrentLevel); }
    }

    protected int _power
    {
        get { return _abilityStats.Power; }
    }

    protected int _count
    {
        get { return _abilityStats.Count; }
    }

    protected float _speed
    {
        get { return _abilityStats.Speed; }
    }

    protected float _cooldown
    {
        get { return _abilityStats.Cooldown; }
    }

    protected int _pierce
    {
        get { return _abilityStats.Pierce; }
    }

    public int MaxLevel
    {
        get { return _baseData.LevelUpgrades.Count + 1; }
    }

    public BaseAbilityData BaseData
    {
        get { return _baseData; }
    }

    public virtual void Init(BaseAbilityData data, HeroUnit hero)
    {
        _baseData = data;
        _heroUnit = hero;
        CurrentLevel = 1;

    }

    public void LevelUp()
    {
        if (CurrentLevel >= MaxLevel) return;
        CurrentLevel++;
        OnLevelUp?.Invoke();
    }

    public Stats GetNextLevelStats()
    {
        if (CurrentLevel == 0)
            return _baseData.BaseStats;

        return _baseData.LevelUpgrades[CurrentLevel - 1];
    }
    
}

   
