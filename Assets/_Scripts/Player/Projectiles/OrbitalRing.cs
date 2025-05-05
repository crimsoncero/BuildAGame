using System;
using UnityEngine;

public class OrbitalRing : MonoBehaviour, IPausable
{
    [SerializeField] private CircleCollider2D _collider;
    
    private int _damage;
    private float _duration;
    private float _tickRate;

    private float _timeToNextTick;
    private void Start()
    {
        GameManager.Instance.RegisterPausable(this);
    }

    public void Initialize(int damage, float duration, float tickRate, float radius)
    {
        // Temporary change scale, later switch to change through vfx and collider
        transform.localScale = new Vector3(radius, radius, radius);
        _collider.enabled = false;
        _damage = damage;
        _duration = duration;
        _tickRate = tickRate;
        _timeToNextTick = 0;
        gameObject.SetActive(true);
        
        Debug.Log(tickRate);
        
        
    }

    private void Update()
    {
        if(GameManager.Instance.IsPaused) return;
        
        if (_duration > 0)
        {
            _duration -= Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);    
        }
        
        
    }

    private void FixedUpdate()
    {
        if(_collider.isActiveAndEnabled)
            _collider.enabled = false;
        
        if(_timeToNextTick > 0)
            _timeToNextTick -= Time.fixedDeltaTime;
        else
        {
            _timeToNextTick = _tickRate;
            _collider.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyUnit enemyHit = collision.gameObject.GetComponent<EnemyUnit>();
        if (enemyHit == null) return;

        Vector2 dir = (Vector2)collision.transform.position - HeroManager.Instance.CenterPosition;
        enemyHit.TakeDamage(_damage, dir, true);
    }

    public void Pause()
    {
    }

    public void Resume()
    {
    }
}
