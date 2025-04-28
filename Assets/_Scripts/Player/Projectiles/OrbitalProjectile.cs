using SeraphUtil.ObjectPool;
using Unity.VisualScripting;
using UnityEngine;
using IPoolable = SeraphUtil.ObjectPool.IPoolable;

public class OrbitalProjectile : BaseProjectile, IPoolable, IPausable
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
        _pool.Return(this);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyUnit enemyHit = collision.gameObject.GetComponent<EnemyUnit>();
        if (enemyHit.IsUnityNull()) return;

        Vector2 dir = (Vector2)collision.transform.position - HeroManager.Instance.CenterPosition;
        enemyHit.TakeDamage(_damage, dir, true);
      
    }

    public void Pause()
    {
        _vfx.Pause();
    }

    public void Resume()
    {
        _vfx.Play();
    }

    public void OnTakeFromPool()
    {
        GameManager.Instance.RegisterPausable(this);
    }

    public void OnReturnToPool()
    {
        gameObject.SetActive(false);
    }
}
