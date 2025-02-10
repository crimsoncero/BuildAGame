using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbilityData : ScriptableObject
{
    [Serializable]
    public struct Stats
    {
        public int Power;
        public int Count;
        public float Speed;
        public float Cooldown;
        public int Pierce;

        public void Add(Stats added)
        {
            Power += added.Power;
            Count += added.Count;
            Speed += added.Speed;
            Cooldown += added.Cooldown;
            Pierce += added.Pierce;
        }
    }

    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Stats BaseStats { get; private set; }
    [field: SerializeField] public List<Stats> LevelUpgrades { get; private set; }

    public int MaxLevel { get { return LevelUpgrades.Count + 1; } }

    public Stats GetCurrentStats(int level)
    {
        Stats stats = BaseStats;
        for(int i = 0; i < level - 1; i++)
        {
            stats.Add(LevelUpgrades[i]);
        }

        return stats;
    }
    public abstract BaseAbility CreateAbilityComponent(Transform abilityObject);
}
