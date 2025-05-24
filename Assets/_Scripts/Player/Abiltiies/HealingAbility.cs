using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Pool;

public class HealingAbility : BaseAbility
{
    private HealingAbilityData Data { get { return _baseData as HealingAbilityData; } }
    private List<HeroUnit> _heroes;

    private BoolTimer _castTimer;
    private HealVFX _vfx;
   
    private HealingAbilityData.HealingStats Stats
    {
        get { return AbilityStats as HealingAbilityData.HealingStats; }
    }
    public override void Init(BaseAbilityData data, HeroUnit hero)
    {
        base.Init(data, hero);
        HeroManager.Stats.RespawnTime.Multiplicative += ReduceRespawnTime;
        _castTimer = new BoolTimer(false, Cooldown);
        _castTimer.SetTimer(Cooldown);
        _vfx = Instantiate(Data.VFX, transform);
        _vfx.Stop();
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
            _heroes = HeroManager.Instance.Heroes;
        }
        
        foreach (var hero in _heroes)
        {
            if (hero.IsDead) continue;
            
            hero.Heal((int)(Power * Data.HealMultiplier), true);
        }
        
        _vfx.Play();
    }

    public void ReduceRespawnTime(ref float s, HeroUnit hero)
    {
        if (_heroUnit.IsDead) return;
        
        s *= (100f - Stats.RespawnReduction) / 100f;
    }
}
