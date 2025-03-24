using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class MissileProjectile : BaseProjectile
{
    private int _damage;
    private int _pierce;
    private ObjectPool<MissileProjectile> _pool;
    [SerializeField] private Rigidbody2D _rb2d;
    [SerializeField] private ParticleSystem _vfx;
    public void Init(ObjectPool<MissileProjectile> pool, Vector3 position, int damage, int pierce, Vector2 velocity)
    {
        _pool = pool;
        transform.position = position;
        _damage = damage;
        _pierce = pierce;
        _rb2d.linearVelocity = velocity;
        transform.right = (Vector3)velocity;

        GameManager.Instance.OnGamePaused += PauseProjectile;
        GameManager.Instance.OnGameResumed += ResumeProjectile;
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyUnit enemyHit = collision.gameObject.GetComponent<EnemyUnit>();
        if (enemyHit.IsUnityNull()) return;

        Vector2 dir = collision.transform.position - transform.position; 
        enemyHit.TakeDamage(_damage, dir, true);
        
        if (_pierce > 0)
            _pierce--;
        else
        {
            GameManager.Instance.OnGamePaused -= PauseProjectile;
            GameManager.Instance.OnGameResumed -= ResumeProjectile;
            _pool.Release(this);
        }
        
    }
    
    private void PauseProjectile()
    {
        _vfx.Pause();
        _rb2d.simulated = false;
    }

    private void ResumeProjectile()
    {
        _vfx.Play();
        _rb2d.simulated = true;
    }
}
