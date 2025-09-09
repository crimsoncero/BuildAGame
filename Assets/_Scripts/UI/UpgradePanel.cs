using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    [FormerlySerializedAs("characterImage")] [FormerlySerializedAs("_abilityIcon")] [SerializeField] private Image _characterImage;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _upgradeText;
    [SerializeField] private Image _iconImage;
    private BaseAbility _ability;
    private UpgradeMenu _upgradeMenu;
    
    public void Initialize(BaseAbility ability, UpgradeMenu upgradeMenu)
    {
        _ability = ability;
        _upgradeMenu = upgradeMenu;
        _characterImage.sprite = ability.HeroData.MugshotSprite;
        _iconImage.sprite = ability.HeroData.IconSprite;
        _nameText.text = _ability.BaseData.Name + " Lv. " + (_ability.CurrentLevel + 1);
        _upgradeText.text = _ability.GetNextLevelStats().Description;

    }


    public void OnClick()
    {
        _ability.LevelUp();
        _upgradeMenu.OnUpgradeClick();
    }
}
