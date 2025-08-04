using Pathfinding;
using SeraphRandom;
using System;
using System.Collections.Generic;
using System.Linq;
using SeraphUtil.ObjectPool;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    [Serializable]
    private struct DonutParams
    {
        public float MinimumRadius;
        public float MaximumRadius;
    }

    [Header("Pool Settings")] 
    [SerializeField] private MultiPoolOptions<EnemyUnit> _poolOptions;
    [SerializeField] private Transform _poolParent;

    [Header("Spawn Settings")]
    [Range(2,12)]
    [SerializeField] private int _numberOfSectors;
    [SerializeField] private DonutParams _spawnDonutArea;
    [SerializeField] private int _maximumConcurrentEnemies = 300;
    private Transform _centerPosition;

    private MultiPool<EnemyUnit> Pool { get; set; }
    public List<EnemyUnit> EnemyList { get { return Pool.ActiveList.Where((e)=> e.IsAlive).ToList(); } }    
    private void Start()
    {
        // TODO - start from GameManager
        InitializeSpawner(_poolOptions);
    }

    public void InitializeSpawner(MultiPoolOptions<EnemyUnit> poolOptions)
    {
        _poolOptions = poolOptions;
        _centerPosition = HeroManager.Instance.Center;
        Pool = new MultiPool<EnemyUnit>(poolOptions, _poolParent);
    }

    public EnemyUnit FindEnemy(Transform target)
    {
        return EnemyList.Find((t) => t.transform == target);
    }
    
    #region Spawn Methods

    /// <summary>
    /// Spawns an enemy in a random position according to the spawner.
    /// </summary>
    /// <param name="data"></param>
    public void SpawnEnemy(EnemyData data)
    {
        List<EnemyData> list = new() { data };
        SpawnWave(list, 1);
    }
    /// <summary>
    /// Spawns an enemy at the given position.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="position"></param>
    public EnemyUnit SpawnEnemy(EnemyData data, Vector3 position)
    {
        EnemyUnit enemy = Pool.Take(data.Prefab);
        enemy.Initialize(data, position, Pool);
        return enemy;
    }
    private List<EnemyData> SpawnGroup(List<EnemyData> enemiesToSpawn, int sector)
    {
        ShuffleBag<EnemyData> enemyBag = new ShuffleBag<EnemyData>(enemiesToSpawn);

        // Find an angle in the given sector
        float degreesInSector = 360 / _numberOfSectors;
        int minAngle = (int)Mathf.Floor(degreesInSector * sector);
        int maxAngle = (int)Mathf.Floor(degreesInSector * (sector + 1));
        int angle = UnityEngine.Random.Range(minAngle, maxAngle);

        // Find the node in the graph that is nearest to the desired position.
        Vector3 spawnPoint = GetSpawnPosition(angle);

        var constraint = NNConstraint.None;
        // Constrain the search to walkable nodes only
        constraint.constrainWalkability = true;
        constraint.walkable = true;

        NNInfo nodeInfo = AstarPath.active.GetNearest(spawnPoint, constraint);
        GraphNode nearestNode = nodeInfo.node;

        List<GraphNode> nodeList = new List<GraphNode>();
        Queue<GraphNode> frontier = new Queue<GraphNode>();
        nodeList.Add(nearestNode);
        frontier.Enqueue(nearestNode);

        for(int i = 0; i < enemiesToSpawn.Count; i++)
        {
            // Stop spawning if there are no more eligible nodes;
            if(frontier.Count <= 0)
            {
                // Return the list of enemies that could not spawn.
                return enemyBag.GetCurrentBag();
            }

            // Get the node at the top of the queue, and add its neighbours to the queue.
            GraphNode currentNode = frontier.Dequeue();
            List<GraphNode> neighbours = new List<GraphNode>();
            currentNode.GetConnections(otherNode => { neighbours.Add(otherNode); });
            foreach(GraphNode neighbour in neighbours)
            {
                if (nodeList.Contains(neighbour)) continue; // No repeats
                if (!IsNodeEligible(neighbour)) continue; // Not eligible

                nodeList.Add(neighbour);
                frontier.Enqueue(neighbour);
            }

            SpawnEnemy(enemyBag.Pick(), (Vector3)currentNode.position);
        }
        // Spawned all enemies
        return null;
    }

    public void SpawnWave(List<EnemyData> spawnGroup, int numberOfGroups)
    {
        // Don't spawn a wave if already reached max enemy count;
        if (Pool.ActiveCount >= _maximumConcurrentEnemies) return;
        
        ShuffleBag<int> sectorBag = new ShuffleBag<int>(Enumerable.Range(0, _numberOfSectors - 1).ToList());
        
        for(int i = 0; i < numberOfGroups; i++)
        {
            var enemeisToSpawn = spawnGroup;
            while(enemeisToSpawn != null)
            {
                int sector = sectorBag.Pick();
                enemeisToSpawn = SpawnGroup(enemeisToSpawn, sector);
            }
        }
    }

    private Vector3 GetSpawnPosition(float angle)
    {
        Vector3 spawnPosition = _centerPosition.position;

        var rotation = Quaternion.Euler(0, 0, angle);
        float distance = (_spawnDonutArea.MaximumRadius + _spawnDonutArea.MinimumRadius) / 2;
        var forward = new Vector3(1,0,0) * distance;
        var res = rotation * forward;
        return spawnPosition + res;
    }
    /// <summary>
    /// Checks if a node is eligible for spawning an enemy in
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private bool IsNodeEligible(GraphNode node)
    {
        // ReSharper disable once ReplaceWithSingleAssignment.True

        // The check is an AND test, so the node must pass all checks.
        bool flag = true;

        // Not walkable
        if (!node.Walkable) 
            flag = false;

        // Too close to screen
        if (Vector3.Distance((Vector3)node.position, _centerPosition.position) < _spawnDonutArea.MinimumRadius)
            flag = false;

        return flag;
    }

    #endregion

    private void OnDisable()
    {
    }

    private void OnDrawGizmosSelected()
    {
        if(_centerPosition != null)
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(_centerPosition.position, _spawnDonutArea.MinimumRadius);
            Gizmos.DrawWireSphere(_centerPosition.position, _spawnDonutArea.MaximumRadius);

        }
        else
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(Vector3.zero, _spawnDonutArea.MinimumRadius);
            Gizmos.DrawWireSphere(Vector3.zero, _spawnDonutArea.MaximumRadius);

        }
       
    }
}

