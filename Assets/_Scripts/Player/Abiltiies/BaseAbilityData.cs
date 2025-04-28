using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class BaseAbilityData : ScriptableObject
{
    [Serializable]
    public class AbilityStats
    {
        public string Description;
        [FormerlySerializedAs("Power")] public int Power00;
        public int Count;
        [FormerlySerializedAs("Speed")] public int Speed00;
        public float Cooldown;
        public int Pierce;

        public virtual void Add(AbilityStats added)
        {
            Power00 += added.Power00;
            Count += added.Count;
            Speed00 += added.Speed00;
            Cooldown += added.Cooldown;
            Pierce += added.Pierce;
        }

    }
    
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    public virtual AbilityStats BaseAbilityStats { get; }
    public virtual List<AbilityStats> LevelUpgrades { get; }

    public int MaxLevel { get { return LevelUpgrades.Count + 1; } }

    public AbilityStats GetCurrentStats(int level)
    {
        // Non Additive
        return level == 0 ? BaseAbilityStats : LevelUpgrades[level - 1];
        
        // Additive Stats
        // AbilityStats stats = GetStatsZero();
        // stats.Add(BaseAbilityStats);
        //
        // for(int i = 0; i < level; i++)
        // {
        //     stats.Add(LevelUpgrades[i]);
        // }
        //
        // return stats;
    }

    protected virtual AbilityStats GetStatsZero()
    { 
        return new AbilityStats();
    }
    public abstract BaseAbility CreateAbilityComponent(Transform abilityObject);
}
