using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class HealingAbility : BaseAbility
{
    private HealingAbilityData Data { get { return _baseData as HealingAbilityData; } }
    private List<HeroUnit> _heroes;

    private BoolTimer _castTimer;


    public override void Init(BaseAbilityData data, HeroUnit hero)
    {
        base.Init(data, hero);

        _castTimer = new BoolTimer(false, _cooldown);
        _castTimer.SetTimer(_cooldown);
    }

    private void Update()
    {
        if (_heroUnit.IsDead) return;
        if (!GameManager.Instance.IsPaused)
        {
            _castTimer.UpdateTimer();
            if (_castTimer.Value)
            {
                _castTimer.SetTimer();
                Cast();
            }

        }

    }

    private void Cast()
    {
        if(_heroes == null)
        {
            _heroes = PlayerController.Instance.Heroes;
        }

        foreach (var hero in _heroes)
        {
            hero.Heal(_power, true);
        }

    }


   

}
