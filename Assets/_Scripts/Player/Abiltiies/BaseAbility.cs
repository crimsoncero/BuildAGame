using UnityEngine;

public class BaseAbility : MonoBehaviour
{
    public int CurrentLevel { get; private set; }

    protected BaseAbilityData _baseData;
    protected HeroUnit _heroUnit;
    
    protected HeroData _heroData { get { return _heroUnit.Data; } }

    public virtual void Init(BaseAbilityData data, HeroUnit hero)
    {
        _baseData = data;
        _heroUnit = hero;
        CurrentLevel = 1;
    }
}
