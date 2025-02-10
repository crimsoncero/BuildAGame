using UnityEngine;

public class BaseAbility : MonoBehaviour
{
    public int CurrentLevel { get; private set; }
    
    protected BaseAbilityData _baseData;
    protected HeroUnit _heroUnit;
    

    protected HeroData _heroData { get { return _heroUnit.Data; } }
    protected BaseAbilityData.Stats _abilityStats { get { return _baseData.GetCurrentStats(CurrentLevel); } }
    protected int _power { get { return _abilityStats.Power; } }
    protected int _count { get { return _abilityStats.Count; } }
    protected float _speed { get { return _abilityStats.Speed; } }
    protected float _cooldown { get { return _abilityStats.Cooldown; } }
    protected int _pierce { get { return _abilityStats.Pierce; } }

    public virtual void Init(BaseAbilityData data, HeroUnit hero)
    {
        _baseData = data;
        _heroUnit = hero;
        CurrentLevel = 1;
    }
}
