using System;
using System.Collections.Generic;
using System.Linq;
using SeraphUtil.ObjectPool;
using Unity.Profiling;
using UnityEngine;
using Random = UnityEngine.Random;

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
            _attackTimer.SetTimer();
        }

    }

    private void Cast()
    {
        
        var targetList = FindTargets(Count);
        if (targetList == null) return; // No targets found.
        
        var targetMatrix = new EnemyUnit[Count][];
        for (var i = 0; i < Count; i++)
        {
            targetMatrix[i] = new EnemyUnit[Pierce + 1];
        }

        var chainLengths = new int[Count];
        
        // Target placement
        for (var i = 0; i < Count; i++)
        {
            var potentialTargets = new List<Collider2D>(targetList);
            
            if(potentialTargets.Count == 0) continue;
            
            // Initial chain target
            try
            {
                int firstTargetIndex = i % targetList.Count;
                targetMatrix[i][0] = FindEnemy(potentialTargets[firstTargetIndex]);
                potentialTargets.RemoveAt(firstTargetIndex);
                chainLengths[i] = 1;
                // Other chain targets
                for (var j = 1; j <= Pierce; j++)
                {
                    if (potentialTargets.Count <= 0) break;

                    var targetIndex = Random.Range(0, potentialTargets.Count);
                    var target = potentialTargets[targetIndex];
                    potentialTargets.RemoveAt(targetIndex);
                    targetMatrix[i][j] = FindEnemy(target);
                    chainLengths[i]++;
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
           
        }
        
        // Create VFX and deal Damage to targets
        var chainDamage = Power * (Pierce + 1);
        for (var i = 0; i < Count; i++)
        {
            Transform source = transform;
            if (chainLengths[i] == 0) continue;
            int damage = chainDamage / chainLengths[i];
            for (var j = 0; j < chainLengths[i]; j++)
            {
                var enemy = targetMatrix[i][j];
                if (enemy == null || !enemy.IsAlive)
                    break;
                
                var vfx = _pool.Take();
                vfx.transform.parent = source;
                vfx.Initialize(source.position, enemy.transform.position, j, _pool);
                enemy.TakeDamage(damage, enemy.transform.position - transform.position, true);
                source = enemy.transform;
            }
        }
        
    }

    private EnemyUnit FindEnemy(Collider2D target)
    {
        return EnemySpawner.Instance.FindEnemy(target.transform);
    }
    private List<Collider2D> FindTargets(int count)
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        filter.SetLayerMask((Data.EnemyLayer));
        List<Collider2D> targets = new List<Collider2D>();

        int targetCount = Physics2D.OverlapBox(transform.position, Data.TargetBox, 0, filter, targets);
        if (targetCount <= 0) return null; // No targets found
        targets.RemoveAll((t) => t.isTrigger == false);
        targets.OrderBy((t) => Vector3.Distance(transform.position, t.transform.position));
        
        return targets;
    }

    private void OnDrawGizmos()
    {
        if (Data.Debug)
        {
            Gizmos.DrawWireCube(transform.position, Data.TargetBox);
        }
    }
}
