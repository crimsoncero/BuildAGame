using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AbilityInstance : MonoBehaviour
{
    public HeroUnit Hero { get; set; }
    public BaseAbilityData Ability { get; set; }
    public ObjectPool<Projectile> ProjectilePool { get; private set; }
    
    public void Init(HeroUnit hero, BaseAbilityData ability)
    {
        Hero = hero;
        Ability = ability;

        Ability.Init(this);
    }

    private void Update()
    {
        Ability.Update(this);
    }

    public void InitPool(ObjectPool<Projectile> pool)
    {
        ProjectilePool = pool;
    }

}
