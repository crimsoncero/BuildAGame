using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbilityData : ScriptableObject
{
    [Serializable]
    public class AbilityStats
    {
        public string Description;
        public int Power;
        public int Count;
        public float Speed;
        public float Cooldown;
        public int Pierce;

        public virtual void Add(AbilityStats added)
        {
            Power += added.Power;
            Count += added.Count;
            Speed += added.Speed;
            Cooldown += added.Cooldown;
            Pierce += added.Pierce;
        }

    }
    
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public AbilityStats BaseAbilityStats { get; private set; }
    [field: SerializeField] public List<AbilityStats> LevelUpgrades { get; private set; }

    public int MaxLevel { get { return LevelUpgrades.Count + 1; } }

    public AbilityStats GetCurrentStats(int level)
    {
        AbilityStats stats = GetStatsZero();
        stats.Add(BaseAbilityStats);
        
        for(int i = 0; i < level - 1; i++)
        {
            stats.Add(LevelUpgrades[i]);
        }

        return stats;
    }

    protected virtual AbilityStats GetStatsZero()
    { 
        return new AbilityStats();
    }
    public abstract BaseAbility CreateAbilityComponent(Transform abilityObject);
}
