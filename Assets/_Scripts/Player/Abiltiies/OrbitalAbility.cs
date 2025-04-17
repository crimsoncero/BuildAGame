using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class OrbitalAbility : BaseAbility
{
    private OrbitalAbilityData Data { get { return _baseData as OrbitalAbilityData; } }
    private ObjectPool<OrbitalProjectile> _pool;

    private BoolTimer _spawnTimer;
    private BoolTimer _isNotActive;
    
    private List<OrbitalProjectile> _projectileList;
    private float _spawnTime { get { return Cooldown + Data.Duration; } }
    public override void Init(BaseAbilityData data, HeroUnit hero)
    {
        base.Init(data, hero);
        _projectileList = new List<OrbitalProjectile>();
        _pool = new ObjectPool<OrbitalProjectile>(CreateProjectile, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, false, Data.InitCount, 50);
        _pool.PreWarm(Data.InitCount);
        _spawnTimer = new BoolTimer(false, _spawnTime);
        _spawnTimer.SetTimer(_spawnTime);
    }

    private void Update()
    {
        if (_heroUnit.IsDead)
        {
            // Removes the projectiles if the player is still alive.
            if (!_isNotActive.Value)
            {
                foreach(OrbitalProjectile projectile in _projectileList.Where((p) => p.IsActive))
                    projectile.ReleaseOrbital();
            }
            
            return;
        }
        if (!GameManager.Instance.IsPaused)
        {
            _spawnTimer.UpdateTimer();
            _isNotActive.UpdateTimer();
            if (_spawnTimer.Value)
            {
                SpawnOrbitals();
            }

            transform.Rotate(0,0, Speed * Data.SpeedMultipliar * Time.deltaTime);
        }
       
    }

    private void SpawnOrbitals()
    {
        _spawnTimer.SetTimer(_spawnTime);
        _isNotActive.SetTimer(Data.Duration);
        
        var posList = Helpers.GetEqualOrbitLocations(Count, Data.Radius);

        foreach(var pos in posList)
        {
            var orbital = _pool.Get();
            Vector2 position = Vector2.zero;
            position.x = transform.position.x + pos.position.x;
            position.y = transform.position.y + pos.position.y;
            orbital.Init(_pool, position, Data.Duration, Power);
        }
    }

   
    #region Pool Methods

    private OrbitalProjectile CreateProjectile()
    {
        OrbitalProjectile projectile = Instantiate(Data.ProjectilePrefab, transform);
        
        projectile.gameObject.SetActive(false);
        _projectileList.Add(projectile);
        
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
        _projectileList.Remove(projectile);
        Destroy(projectile.gameObject);
    }
    #endregion
    
    
}
