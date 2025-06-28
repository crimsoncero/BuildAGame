using System.Linq;
using SeraphUtil.ObjectPool;
using UnityEngine;

public class OrbitalAbility : BaseAbility
{
    private OrbitalAbilityData Data { get { return _baseData as OrbitalAbilityData; } }
    private ObjectPool<OrbitalProjectile> _pool;
    
    private BoolTimer _spawnTimer;
    private BoolTimer _isNotActive;

    private OrbitalRing _ringObject; 
    private OrbitalAbilityData.OrbitalStats Stats
    {
        get { return AbilityStats as OrbitalAbilityData.OrbitalStats; }
    }
    private float _spawnTime { get { return Cooldown + Stats.Duration; } }
    public override void Init(BaseAbilityData data, HeroUnit hero)
    {
        base.Init(data, hero);
        _pool = new ObjectPool<OrbitalProjectile>(Data.ProjectilePrefab, transform, (uint)Data.InitCount);
        _spawnTimer = new BoolTimer(false, _spawnTime);
        _spawnTimer.SetTimer(_spawnTime);
        _ringObject = Instantiate(Data.RingPrefab, transform, false);
    }

    private void Update()
    {
        if (_heroUnit.IsDead)
        {
            // Removes the projectiles if the player is still alive.
            if (!_isNotActive.Value)
            {
                foreach (var t in _pool.TotalList)
                {
                    if(t.IsActive)
                        t.ReleaseOrbital();
                }
            }
            
            return;
        }
        if (!GameManager.Instance.IsPaused)
        {
            _spawnTimer.UpdateTimer();
            _isNotActive.UpdateTimer();
            if (_spawnTimer.Value)
            {
                if (Stats.IsRing)
                {
                    SpawnRing();
                }
                else
                {
                    SpawnOrbitals();
                }
            }

            transform.Rotate(0,0, Speed * Data.SpeedMultiplier * Time.deltaTime);
        }
       
    }

    private void SpawnRing()
    {
        _spawnTimer.SetTimer(_spawnTime);
        _isNotActive.SetTimer(Stats.Duration);

        float tickRate = (360f / (Speed * Data.SpeedMultiplier)) / Count;
        _ringObject.Initialize(Power, Stats.Duration, tickRate, Stats.Radius);
        
    }
    private void SpawnOrbitals()
    {
        _spawnTimer.SetTimer(_spawnTime);
        _isNotActive.SetTimer(Stats.Duration);
        
        var posList = Helpers.GetEqualOrbitLocations(Count, Stats.Radius);

        foreach(var pos in posList)
        {
            var orbital = _pool.Take();
            Vector2 position = Vector2.zero;
            position.x = transform.position.x + pos.position.x;
            position.y = transform.position.y + pos.position.y;
            orbital.Init(_pool, position, Stats.Duration, Power);
            orbital.gameObject.SetActive(true);
        }
    }

   
   
    
    
}
