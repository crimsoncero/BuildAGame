using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Healing Data", menuName = "Scriptable Objects/Abilities/Healing Data")]
public class HealingAbilityData : BaseAbilityData
{
    [Serializable]
    public class HealingStats : AbilityStats
    {
        [Range(0, 100)]public int RespawnReduction;

        public override void Add(AbilityStats added)
        {
            base.Add(added);
            if (added is HealingStats oAdded)
            {
                RespawnReduction += oAdded.RespawnReduction;
            }
        }
    }
    [field: Header("Healing Stats")] 
    [SerializeField] protected HealingStats _baseStats;
    [SerializeField] protected List<HealingStats> _levelUpgrades;
    public override AbilityStats BaseAbilityStats
    {
        get {return _baseStats;}
    }

    public override List<AbilityStats> LevelUpgrades
    {
        get {return new List<AbilityStats>(_levelUpgrades);}
    }
    
    [field: Header("Behavior Stats")]
    [field: SerializeField] public float HealMultiplier {  get; private set; }
    [field: SerializeField] public HealVFX VFX { get; private set; }
    public override BaseAbility CreateAbilityComponent(Transform abilityObject)
    {
        HealingAbility ability = abilityObject.AddComponent<HealingAbility>();
        return ability;
    }
    
}
