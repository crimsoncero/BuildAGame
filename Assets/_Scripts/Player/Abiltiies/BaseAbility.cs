using System;
using UnityEngine;
using static BaseAbilityData;

public abstract class BaseAbility : MonoBehaviour
{
    public event Action OnLevelUp;

    public int CurrentLevel { get; private set; } = -1;

    protected BaseAbilityData _baseData;
    protected HeroUnit _heroUnit;


    public HeroData HeroData
    {
        get { return _heroUnit.Data; }
    }

    protected AbilityStats AbilityStats
    {
        get { return _baseData.GetCurrentStats(CurrentLevel); }
    }

    protected int Power
    {
        get { return HeroManager.Stats.Power.FinalWithAdditive(_heroUnit,AbilityStats.Power00); }
    }

    protected int Count
    {
        get { return HeroManager.Stats.Count.FinalWithAdditive(_heroUnit,AbilityStats.Count); }
    }

    protected float Speed
    {
        get { return HeroManager.Stats.Speed.FinalWithAdditive(_heroUnit, AbilityStats.Speed00); }
    }

    protected float Cooldown
    {
        get { return HeroManager.Stats.Cooldown.FinalWithAdditive(_heroUnit, AbilityStats.Cooldown); }
    }

    protected int Pierce
    {
        get { return HeroManager.Stats.Pierce.FinalWithAdditive(_heroUnit,AbilityStats.Pierce); }
    }

    public int MaxLevel
    {
        get { return _baseData.LevelUpgrades.Count; }
    }

    public BaseAbilityData BaseData
    {
        get { return _baseData; }
    }

    public virtual void Init(BaseAbilityData data, HeroUnit hero)
    {
        _baseData = data;
        _heroUnit = hero;
        CurrentLevel = 0;

    }

    public void LevelUp()
    {
        if (CurrentLevel >= MaxLevel) return;
        CurrentLevel++;
        _heroUnit.Visuals.OnLevelUp();
        OnLevelUp?.Invoke();
    }

    public AbilityStats GetNextLevelStats()
    {
        if (CurrentLevel < 0)
            return _baseData.BaseAbilityStats;

        return _baseData.LevelUpgrades[CurrentLevel];
    }
    
}

   
