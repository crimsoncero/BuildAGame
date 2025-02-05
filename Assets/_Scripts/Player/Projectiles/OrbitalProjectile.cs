using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class OrbitalProjectile : BaseProjectile
{
    private float _lifeTime;
    private int _damage;
    private ObjectPool<OrbitalProjectile> _pool;

    public void Init(ObjectPool<OrbitalProjectile> pool, Vector3 position, float lifeTime, int damage)
    {
        _pool = pool;
        transform.position = position;
        _lifeTime = lifeTime;
        _damage = damage;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsPaused)
        {
            if (_lifeTime > 0)
                _lifeTime -= Time.deltaTime;
            else
            {
                _pool.Release(this);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyUnit enemyHit = collision.gameObject.GetComponent<EnemyUnit>();
        if (enemyHit.IsUnityNull()) return;

        enemyHit.TakeDamage(_damage);
      
    }
}
