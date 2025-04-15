using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class OrbitalProjectile : BaseProjectile
{
    
    [SerializeField] private ParticleSystem _vfx;
    public bool IsActive { get; private set; } = false;
    
    private float _lifeTime;
    private int _damage;
    private ObjectPool<OrbitalProjectile> _pool;
    
    

    public void Init(ObjectPool<OrbitalProjectile> pool, Vector3 position, float lifeTime, int damage)
    {
        _pool = pool;
        transform.position = position;
        _lifeTime = lifeTime;
        _damage = damage;
        IsActive = true;
        
        GameManager.Instance.OnGamePaused += OnPause;
        GameManager.Instance.OnGameResumed += OnResume;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsPaused)
        {
            if (_lifeTime > 0)
                _lifeTime -= Time.deltaTime;
            else
            {
                ReleaseOrbital();
            }
        }
        
    }

    public void ReleaseOrbital()
    {
        IsActive = false;
        _pool.Release(this);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyUnit enemyHit = collision.gameObject.GetComponent<EnemyUnit>();
        if (enemyHit.IsUnityNull()) return;

        Vector2 dir = (Vector2)collision.transform.position - HeroManager.Instance.CenterPosition;
        enemyHit.TakeDamage(_damage, dir, true);
      
    }

    private void OnPause()
    {
        _vfx.Pause();
    }

    private void OnResume()
    {
        _vfx.Play();
    }
    
}
