using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class HeroFrame : MonoBehaviour
{
    [SerializeField] private MMProgressBar _healthBar;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _levelText;

    [SerializeField] private HeroUnit _hero;

    public void Init(HeroUnit hero)
    {
        _hero = hero;
        _nameText.text = hero.Data.name;
        UpdateLevelText();
        _hero.OnHealthChanged += UpdateHealthBar;
        _hero.Ability.OnLevelUp += UpdateLevelText;
    }
    
    public void UpdateHealthBar()
    {
        _healthBar.UpdateBar(_hero.CurrentHealth, 0, _hero.MaxHealth);
    }

    public void UpdateLevelText()
    {
        int level = _hero.Ability.CurrentLevel;
        _levelText.text = "Lv. " + level.ToString();
    }
}
