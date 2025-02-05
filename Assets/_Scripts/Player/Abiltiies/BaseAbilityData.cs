using UnityEngine;
using UnityEngine.Pool;

public abstract class BaseAbilityData : ScriptableObject
{
    protected HeroUnit _hero;
    protected AbilityInstance _instance;
    [SerializeField] protected Projectile _projectilePrefab;
    [SerializeField] protected int _initialPoolSize; 
    public virtual void Init(AbilityInstance instance)
    {
        _instance = instance;
        _hero = _instance.Hero;
        _instance.InitPool(new ObjectPool<Projectile>
            (CreateProjectile, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, false, _initialPoolSize));
    }
    public abstract void Update(AbilityInstance instance);


    #region Pool Methods

    protected Projectile CreateProjectile()
    {
        Projectile projectile = Instantiate(_projectilePrefab, _instance.transform);

        projectile.gameObject.SetActive(false);

        return projectile;
    }

    private void OnTakeFromPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);
    }
    private void OnReturnedToPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }
    #endregion
}
