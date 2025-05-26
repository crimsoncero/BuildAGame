using System;
using System.Collections.Generic;
using System.Linq;
using SeraphUtil.ObjectPool;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ChainLightningAbility : BaseAbility
{
    private ChainLightningAbilityData Data { get { return _baseData as ChainLightningAbilityData; } }
    private ObjectPool<LightningVFX> _pool;

    private BoolTimer _attackTimer;

    private ChainLightningAbilityData.ChainLightningStats Stats
    {
        get { return AbilityStats as ChainLightningAbilityData.ChainLightningStats; }
    }

    public override void Init(BaseAbilityData data, HeroUnit hero)
    {
        base.Init(data, hero);
        _pool = new ObjectPool<LightningVFX>(Data.LightningVFXPrefab, transform, (uint)Data.InitVFXCount);
        _attackTimer = new BoolTimer(false, Cooldown);
        _attackTimer.SetTimer();
    }

    private void Update()
    {
        if (_heroUnit.IsDead) return;
        if (!GameManager.Instance.IsGameActive) return;
        
        _attackTimer.UpdateTimer();

        if (_attackTimer.Value)
        {
            Cast();
        }

    }

    private void Cast()
    {
        var targetMatrix = new EnemyUnit[Count][];
        for (var i = 0; i < Count; i++)
        {
            targetMatrix[i] = new EnemyUnit[Pierce];
        }

        var targetList = FindTargets(Count);
        
        
        // Target placement
        for (int i = 0; i < Count; i++)
        {
            
            
        }
        
        
        
    }

    private List<Collider2D> FindTargets(int count)
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        filter.SetLayerMask((Data.EnemyLayer));
        List<Collider2D> targets = new List<Collider2D>();

        int targetCount = Physics2D.OverlapCircle(transform.position, Data.MaxRange, filter, targets);

        if (targetCount <= 0) return null; // No targets found
        
        targets.OrderBy((t) => (t.transform.position - _heroUnit.transform.position));
        
        return targets;
    }
}
