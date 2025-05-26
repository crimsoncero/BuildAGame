using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Chain Lightning Data", menuName = "Scriptable Objects/Abilities/Chain Lightning Data")]
public class ChainLightningAbilityData : BaseAbilityData
{
    [Serializable]
    public class ChainLightningStats : BaseAbilityData.AbilityStats
    {
        public float Forks;

        public override void Add(BaseAbilityData.AbilityStats stats)
        {
            base.Add(stats);
            if (stats is ChainLightningStats added)
            {
                Forks += added.Forks;
            }
        }
    }

    [field: Header("Chain Lightning Stats")]
    [SerializeField] protected ChainLightningStats _baseStats;
    [SerializeField] protected List<ChainLightningStats> _levelUpgrades;

    public override AbilityStats BaseAbilityStats
    {
        get { return _baseStats;}
    }
    public override List<AbilityStats> LevelUpgrades
    {
        get { return new List<AbilityStats>(_levelUpgrades); }
    }
    
    [field: Header("Behavior Stats")]
    [field: SerializeField] public Vector2 TargetBox { get; private set; }
    [field: SerializeField] public LightningVFX LightningVFXPrefab { get; private set; }    
    [field: SerializeField] public int InitVFXCount { get; private set; }
    [field: SerializeField] public LayerMask EnemyLayer { get; private set; }
    [field: SerializeField] public bool Debug { get; private set; }
    protected override AbilityStats GetStatsZero()
    {
        return new ChainLightningStats();
    }

    public override BaseAbility CreateAbilityComponent(Transform abilityObject)
    {
        ChainLightningAbility ability = abilityObject.AddComponent<ChainLightningAbility>();
        return ability;
    }
}
