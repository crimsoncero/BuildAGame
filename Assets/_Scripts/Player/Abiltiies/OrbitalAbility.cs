using UnityEngine;
using UnityEngine.Pool;

public class OrbitalAbility : BaseAbility
{


    private OrbitalAbilityData Data { get { return _baseData as OrbitalAbilityData; } }
    private ObjectPool<OrbitalProjectile> _pool;

    private BoolTimer _spawnTimer;

    // Computational properties to adjust ability parameters depending on other factors than the base:
    private int _damage { get { return Data.Damage; } }
    private int _count { get { return Data.Count; } }
    private float _cooldown { get { return Data.Cooldown; } }
    private float _duration { get { return Data.Duration; } }
    private float _spawnTime { get { return _cooldown + _duration; } }
    public override void Init(BaseAbilityData data, HeroUnit hero)
    {
        base.Init(data, hero);
        
        _pool = new ObjectPool<OrbitalProjectile>(Createprojectile, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, false, Data.InitCount, 50);
        _pool.PreWarm(Data.InitCount);
        _spawnTimer = new BoolTimer(false, _spawnTime);
        _spawnTimer.SetTimer(_spawnTime);
    }

    private void Update()
    {
        if (!GameManager.Instance.IsPaused)
        {
            _spawnTimer.UpdateTimer();
            if (_spawnTimer.Value)
            {
                SpawnOrbitals();
            }

            transform.Rotate(0,0, Data.Speed * Time.deltaTime);
        }
       
    }

    private void SpawnOrbitals()
    {
        _spawnTimer.SetTimer(_spawnTime);

        var posList = Helpers.GetEqualOrbitLocations(_count, Data.Radius);

        foreach(var pos in posList)
        {
            var orbital = _pool.Get();
            Vector2 position = Vector2.zero;
            position.x = transform.position.x + pos.position.x;
            position.y = transform.position.y + pos.position.y;
            orbital.Init(_pool, position, _duration, _damage);
        }
    }

    #region Pool Methods

    private OrbitalProjectile Createprojectile()
    {
        OrbitalProjectile projectile = Instantiate(Data.ProjectilePrefab, transform);

        projectile.gameObject.SetActive(false);

        return projectile;
    }

    private void OnTakeFromPool(OrbitalProjectile projectile)
    {
        projectile.gameObject.SetActive(true);
    }
    private void OnReturnedToPool(OrbitalProjectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(OrbitalProjectile projectile)
    {
        Destroy(projectile.gameObject);
    }
    #endregion
}
