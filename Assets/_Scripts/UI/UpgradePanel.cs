using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private Image _abilityIcon;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _upgradeText;
    
    private BaseAbility _ability;
    private UpgradeMenu _upgradeMenu;
    public void Initialize(BaseAbility ability, UpgradeMenu upgradeMenu)
    {
        _ability = ability;
        _upgradeMenu = upgradeMenu;
        _abilityIcon.sprite = ability.BaseData.Icon;
        _nameText.text = _ability.BaseData.Name + " Lv. " + (_ability.CurrentLevel + 1);
        _upgradeText.text = _ability.GetNextLevelStats().Description;

    }

    public void OnClick()
    {
        _ability.LevelUp();
        _upgradeMenu.OnUpgradeClick();
    }
}
