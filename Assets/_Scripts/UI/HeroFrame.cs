using MoreMountains.Tools;
using UnityEngine;

public class HeroFrame : MonoBehaviour
{
    [SerializeField] private MMProgressBar _healthBar;


    [SerializeField] private HeroUnit _hero;

    private void Start()
    {
        Init(_hero);
    }

    public void Init(HeroUnit hero)
    {
        _hero = hero;

        _hero.OnHealthChanged += UpdateHealthBar;
    }

    public void UpdateHealthBar()
    {
        _healthBar.UpdateBar(_hero.CurrentHealth, 0, _hero.MaxHealth);
    }

}
