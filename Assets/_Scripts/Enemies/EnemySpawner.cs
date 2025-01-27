using MoreMountains.Tools;
using Pathfinding;
using SeraphRandom;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : Singleton<EnemySpawner>
{
    [Serializable]
    private struct DonutParams
    {
        public float MinimumRadius;
        public float MaximumRadius;
    }

    [Header("Pool Settings")]
    [SerializeField] private Transform _poolParent;
    [SerializeField] private EnemyUnit _enemyPrefab;
    [SerializeField] private int _initSize = 5;
    [SerializeField] private int _maxSize = 500;

    [Header("Spawn Settings")]
    [Range(2,12)]
    [SerializeField] private int _numberOfSectors;
    [SerializeField] private DonutParams _spawnDonutArea;

    public ObjectPool<EnemyUnit> Pool { get; private set; }


    private Transform _centerPosition;


    private void Start()
    {
        _centerPosition = PlayerController.Instance.Center;
        Pool = new ObjectPool<EnemyUnit>(CreateEnemy, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, false, _initSize, _maxSize);
        Pool.PreWarm(_initSize);
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
    public void SpawnEnemy(EnemyData data, Vector3 position)
    {
        EnemyUnit enemy = Pool.Get();
        enemy.Initialize(data, position, Pool);
    }
    private void SpawnGroup(List<EnemyData> enemiesToSpawn, int sector)
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
                Debug.LogError("Could not spawn all enemies, not enough eligible nodes");
                break;
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


    }

    public void SpawnWave(List<EnemyData> spawnGroup, int numberOfGroups)
    {
        ShuffleBag<int> sectorBag = new ShuffleBag<int>(Enumerable.Range(0, _numberOfSectors - 1).ToList());
        
        for(int i = 0; i < numberOfGroups; i++)
        {
            int sector = sectorBag.Pick();
            SpawnGroup(spawnGroup, sector);
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


    #region Pool Methods

    private EnemyUnit CreateEnemy()
    {
        EnemyUnit enemy = Instantiate(_enemyPrefab, _poolParent);

        enemy.gameObject.SetActive(false);

        return enemy;
    }

    private void OnTakeFromPool(EnemyUnit enemy)
    {
        enemy.gameObject.SetActive(true);
    }
    private void OnReturnedToPool(EnemyUnit enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(EnemyUnit enemy)
    {
        Destroy(enemy.gameObject);
    }

    #endregion

}

