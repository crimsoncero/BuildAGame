using System.Collections.Generic;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroFrame : MonoBehaviour
{
    [SerializeField] private MMProgressBar _healthBar;
    [SerializeField] private MMProgressBar _respawnBar;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _deadText;

    
    private HeroUnit _hero;
    private bool _isDead;
    
    public void Init(HeroUnit hero)
    {
        _hero = hero;
        _nameText.text = hero.Data.name;
        UpdateLevelText();
        _hero.OnHealthChanged += UpdateHealthBar;
        _hero.Ability.OnLevelUp += UpdateLevelText;
        _hero.OnDeath += OnHeroDeathRevive;
        _hero.OnRevive += OnHeroDeathRevive;
    }
    
    public void UpdateHealthBar()
    {
        if(_isDead)
            _respawnBar.UpdateBar(_hero.CurrentHealth, 0, _hero.MaxHealth);
        else
            _healthBar.UpdateBar(_hero.CurrentHealth, 0, _hero.MaxHealth);
    }

    public void UpdateLevelText()
    {
        int level = _hero.Ability.CurrentLevel;
        _levelText.text = "Lv. " + level.ToString();
    }

    public void OnHeroDeathRevive()
    {
        _isDead = _hero.IsDead;
        _levelText.gameObject.SetActive(!_isDead);
        _deadText.gameObject.SetActive(_isDead);

        _healthBar.gameObject.SetActive(!_isDead);
        _respawnBar.gameObject.SetActive(_isDead);
        
        UpdateHealthBar();
    }
}
