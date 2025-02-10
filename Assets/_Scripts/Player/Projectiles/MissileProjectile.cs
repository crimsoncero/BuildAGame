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

        //Vector2 fromVector = transform.position;
        //fromVector.x = 0;
        //fromVector.z = 0;
        //Vector2 toPosition = 
        //transform.LookAt(transform.position + (Vector3)velocity, Vector3.forward);
        //float angle = Vector2.Angle(Vector2.right, velocity);
        //transform.rotation = Quaternion.Euler(0,0,angle);
        transform.right = (Vector3)velocity;
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
