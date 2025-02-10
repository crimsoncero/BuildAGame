using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class MissileProjectile : BaseProjectile
{
    private int _damage;
    private int _pierce;
    private ObjectPool<MissileProjectile> _pool;
    [SerializeField] private Rigidbody2D _rb2d;

    public void Init(ObjectPool<MissileProjectile> pool, Vector3 position, int damage, int pierce, Vector2 velocity)
    {
        _pool = pool;
        transform.position = position;
        _damage = damage;
        _pierce = pierce;
        _rb2d.linearVelocity = velocity;

        float angle = Vector2.Angle(Vector2.right, velocity);
        transform.rotation = Quaternion.Euler(0,0,angle);

    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyUnit enemyHit = collision.gameObject.GetComponent<EnemyUnit>();
        if (enemyHit.IsUnityNull()) return;

        enemyHit.TakeDamage(_damage);
        Debug.Log("Missile hit");
        if (_pierce > 0)
            _pierce--;
        else
            _pool.Release(this);

        
    }
}
