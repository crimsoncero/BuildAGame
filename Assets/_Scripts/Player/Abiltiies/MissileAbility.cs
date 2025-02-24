using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MissileAbility : BaseAbility
{
    private MissileAbilityData Data { get { return _baseData as MissileAbilityData; } }
    private ObjectPool<MissileProjectile> _pool;

    private BoolTimer _castTimer;

    public override void Init(BaseAbilityData data, HeroUnit hero)
    {
        base.Init(data, hero);

        _pool = new ObjectPool<MissileProjectile>(Createprojectile, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, false, Data.InitCount);
        _pool.PreWarm(Data.InitCount);
        _castTimer = new BoolTimer(false, _cooldown);
        _castTimer.SetTimer(_cooldown);
    }

    private void Update()
    {
        if (!GameManager.Instance.IsPaused)
        {
            _castTimer.UpdateTimer();
            if (_castTimer.Value)
            {
                Cast();
                _castTimer.SetTimer(_cooldown);
            }

        }

    }

    private void Cast()
    {

        Collider2D target = FindClosestTarget();
        if (target == null) return; // No target found.
        
        for(int i = 0; i < _count; i++)
        {
            Vector2 targetPos = Vector2.zero;
            targetPos.x = target.bounds.center.x;
            targetPos.y = target.bounds.min.y;
            targetPos.y += Random.Range(0, target.bounds.size.y);

            Vector2 direction = targetPos - (Vector2)transform.position;
            direction.Normalize();
            Vector2 velocity = direction * _speed * Data.SpeedMultipliar;

            MissileProjectile projectile = _pool.Get();

            projectile.Init(_pool, transform.position, _power, _pierce, velocity);
        }
        
    }


    private Collider2D FindClosestTarget()
    {
        // Find targets
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        filter.SetLayerMask(Data.EnemyLayer);
        List<Collider2D> targets = new List<Collider2D>();
        
        // Search half the range
        int targetCount = Physics2D.OverlapCircle(transform.position, Data.Range / 2, filter, targets);
        if (targetCount <= 0) // if didn't find any target, search the whole range.
            targetCount = Physics2D.OverlapCircle(transform.position, Data.Range, filter, targets);

        if (targetCount <= 0) return null; // No target found

        Collider2D closestTarget = targets[0];
        float closestDistance = Vector2.Distance(targets[0].transform.position, transform.position);

        for(int i = 1; i < targets.Count; i++)
        {


            float newDistance = Vector2.Distance(targets[i].transform.position, transform.position);
            if(newDistance < closestDistance)
            {
                closestDistance = newDistance;
                closestTarget = targets[i];
            }
        }

        return closestTarget;
    }
    #region Pool Methods

    private MissileProjectile Createprojectile()
    {
        MissileProjectile projectile = Instantiate(Data.ProjectilePrefab, transform);

        projectile.gameObject.SetActive(false);

        return projectile;
    }

    private void OnTakeFromPool(MissileProjectile projectile)
    {
        projectile.gameObject.SetActive(true);
    }
    private void OnReturnedToPool(MissileProjectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(MissileProjectile projectile)
    {
        Destroy(projectile.gameObject);
    }
    #endregion
}
