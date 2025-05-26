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

    public void Initialize(Vector3 source, Vector3 target, int index, ObjectPool<LightningVFX> pool)
    {
        _pool = pool;
        _lightningVFX.Stop();
        _lightningVFX.SetBool("Is First", index == 0);
        _lightningVFX.SetInt("Sequence Number", index);

        _lifeTimeDuration = _lightningVFX.GetFloat("Duration") * _lifeTimeMultiplier;
        
        var distance = Vector3.Distance(source, target);
        _lightningVFX.SetFloat("Length", distance);
        transform.position = source;
        transform.right = target - source;
        
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
