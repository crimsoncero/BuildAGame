using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Orbital Data", menuName = "Scriptable Objects/Abilities/Orbital Data")]
public class OrbitalAbilityData : BaseAbilityData
{

    [Serializable]
    public class OrbitalStats : AbilityStats
    {
        public float Duration;
        public float Radius;
        public bool IsRing;

        public override void Add(AbilityStats added)
        {
            base.Add(added);
            if (added is OrbitalStats oAdded)
            {
                Duration += oAdded.Duration;
                Radius += oAdded.Radius;
                IsRing = oAdded.IsRing;
            }
        }
    }

    [field: Header("Orbital Stats")] 
    [SerializeField] protected OrbitalStats _baseStats;
    [SerializeField] protected List<OrbitalStats> _levelUpgrades;

    public override AbilityStats BaseAbilityStats
    {
        get {return _baseStats;}
    }

    public override List<AbilityStats> LevelUpgrades
    {
        get {return new List<AbilityStats>(_levelUpgrades);}
    }

    [field: Header("Behavior Stats")]
    [field: SerializeField] public float SpeedMultiplier {  get; private set; }
    
    [field: Header("Pooling")]
    [field: SerializeField] public OrbitalProjectile ProjectilePrefab {  get; private set; }
    [field: SerializeField] public OrbitalRing RingPrefab {  get; private set; }
    [field: SerializeField] public int InitCount {  get; private set; }


    protected override AbilityStats GetStatsZero()
    {
        return new OrbitalStats();
    }

    public override BaseAbility CreateAbilityComponent(Transform abilityObject)
    {
        OrbitalAbility ability = abilityObject.AddComponent<OrbitalAbility>();
        return ability;
    }
}
