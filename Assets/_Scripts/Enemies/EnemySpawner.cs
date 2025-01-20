using UnityEngine;
using UnityEngine.Pool;
public class EnemySpawner : Singleton<EnemySpawner>
{
    [SerializeField] private Transform _poolParent;
    [SerializeField] private EnemyUnit _enemyPrefab;
    [SerializeField] private int _initSize = 5;
    [SerializeField] private int _maxSize = 500;

    [SerializeField] private EnemyData _testData;
    [SerializeField] private Vector3 _testSpawnLocation;
    public ObjectPool<EnemyUnit> Pool { get; private set; }

    private void Start()
    {
        Pool = new ObjectPool<EnemyUnit>(CreateEnemy, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, false, _initSize, _maxSize);
       
    }

    #region Spawn Methods
    public void SpawnTest()
    {
        EnemyUnit enemy = Pool.Get();

        enemy.Initialize(_testData, _testSpawnLocation, Pool);
    }
    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnTest();
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
