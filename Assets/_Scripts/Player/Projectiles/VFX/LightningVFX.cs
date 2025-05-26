using System;
using SeraphUtil.ObjectPool;
using UnityEngine;
using UnityEngine.VFX;

public class LightningVFX : MonoBehaviour, IPausable, IPoolable
{
    [SerializeField] private VisualEffect _lightningVFX;
    [SerializeField] private float _lifeTimeMultiplier = 2f;
    private float _lifeTimeDuration;
    
    private ObjectPool<LightningVFX> _pool;
    
    private void Awake()
    {
        GameManager.Instance.RegisterPausable(this);
    }

    public void Initialize(Transform source, Transform target, int index, ObjectPool<LightningVFX> pool)
    {
        _pool = pool;

        _lightningVFX.SetBool("Is First", index == 1);
        _lightningVFX.SetInt("Sequence Number", index);

        _lifeTimeDuration = _lightningVFX.GetFloat("Duration") * _lifeTimeMultiplier;
        transform.position = source.position;
        transform.LookAt(target);
        
        gameObject.SetActive(true);
        _lightningVFX.Play();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameActive) return;
        
        _lifeTimeDuration -= Time.deltaTime;
        if (_lifeTimeDuration <= 0)
            _pool.Return(this);
            
    }

    public void Pause()
    {
        _lightningVFX.pause = true;
    }

    public void Resume()
    {
        _lightningVFX.pause = false;
    }

    public void OnTakeFromPool()
    {
    }

    public void OnReturnToPool()
    {
        _lightningVFX.Stop();
        gameObject.SetActive(false);
    }
}
