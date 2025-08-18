using System.Collections.Generic;
using Pathfinding;
using SeraphRandom;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Circle Event", menuName = "Scriptable Objects/Level/Events/Enemy Circle Event")]
public class EnemyCircleEvent : EventData
{
    [SerializeField] private float _radius;
    [SerializeField] private float _duration;
    [SerializeField] private int _enemyCount;
    [SerializeField] private List<EnemyData> _enemyDatas;
    [SerializeField] private bool _spawnBoss;
    [SerializeField] private EnemyData _bossData;
    
    public override void Play()
    {
        List<(Vector2 position, float angle)> spawnPoints = Helpers.GetEqualOrbitLocations(_enemyCount, _radius);
        Vector2 centerPos = HeroManager.Instance.CenterPosition;
        var enemyBag = new ShuffleBag<EnemyData>(_enemyDatas);

        foreach (var point in spawnPoints)
        {
            Vector3 relativePosition = centerPos + point.position;
            if(!CheckIfPointValid(relativePosition))
                continue;
            
            var enemy = EnemySpawner.Instance.SpawnEnemy(enemyBag.Pick(), relativePosition);
            enemy.SetDeathTimer(_duration);
        }

        if (_spawnBoss)
        {
            const float adjustment = 0.8f;
            var adjustedRadius = (_radius * adjustment);
            
            Vector2 vector = Random.insideUnitCircle;
           
            if (vector.magnitude < 0.5f)
                vector = vector.normalized * 0.5f;
            
            Vector2 position = HeroManager.Instance.CenterPosition + vector * adjustedRadius;
            
            var constraint = NNConstraint.None;
            constraint.constrainWalkability = true;
            constraint.walkable = true;

            GraphNode nearestNode = AstarPath.active.GetNearest(position, constraint).node;
            if(nearestNode != null)
                EnemySpawner.Instance.SpawnEnemy(_bossData, (Vector3)nearestNode.position);
        }
    }

    
    private bool CheckIfPointValid(Vector2 position)
    {
        const float shortMaxDistance = 1f;
        
        var constraint = NNConstraint.None;
        constraint.constrainWalkability = true;
        constraint.walkable = true;
       
        var currentMaxNearestNodeDistance =  AstarPath.active.maxNearestNodeDistance;
        AstarPath.active.maxNearestNodeDistance = shortMaxDistance;
       
        NNInfo nearestNodeInfo = AstarPath.active.GetNearest(position, constraint);
        AstarPath.active.maxNearestNodeDistance = currentMaxNearestNodeDistance;

        return nearestNodeInfo.node != null;


    }
}
