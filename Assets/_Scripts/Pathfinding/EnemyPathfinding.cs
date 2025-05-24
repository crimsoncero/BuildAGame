using System;
using Pathfinding;
using UnityEngine;

public class EnemyPathfinding : PathfindingModule
{
    [SerializeField] private float _distanceToTrack = 5f;

    [SerializeField] private AutoRepathPolicy _closeRangeRepathPolicy;
    [SerializeField] private AutoRepathPolicy _farRangeRepathPolicy;
    
    private bool _isTracking = false;
    private HeroUnit _trackedHero = null;

    
    
    private void Update()
    {
        if (!_isTracking && AIPath.remainingDistance <= _distanceToTrack)
        {
            _isTracking = true;
            _trackedHero = HeroManager.Instance.GetClosestHero(transform.position);
            if (_trackedHero)
            {
                SetTarget(_trackedHero.transform);
                _trackedHero.OnDeath += DropTracking;
            }
        }

        if (_isTracking && AIPath.remainingDistance > _distanceToTrack)
        {
            DropTracking();
        }
    }

    public override void SetTarget(Transform target)
    {
        base.SetTarget(target);
        if(AIPath.remainingDistance <= _distanceToTrack)
            AIPath.autoRepath = _closeRangeRepathPolicy;
        else
            AIPath.autoRepath = _farRangeRepathPolicy;
    }

    private void DropTracking()
    {
        if (_trackedHero)
        {
            _trackedHero.OnDeath -= DropTracking;
            _trackedHero = null;
        }
        
        _isTracking = false;
        SetTarget(HeroManager.Instance.Center);
    }
}
