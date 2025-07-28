using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectedHeroPanel : MonoBehaviour
{
    [SerializeField] private Button _addButton;
    [SerializeField] private Button _removeButton;
    [SerializeField] private TMP_Text _heroName;
    [SerializeField] private TMP_Text _heroRole;
    [SerializeField] private TMP_Text _heroDescription;
    [SerializeField] private TMP_Text _abilityName;
    [SerializeField] private TMP_Text _abilityDescription;
    [SerializeField] private Image _abilityIcon;    
    
    private HeroData _selectedHero;
    private TeamSelection _teamSelection;

    private void Awake()
    {
        _selectedHero = null;
    }
    
    public void Init(TeamSelection teamSelection, UnityAction onAdd, UnityAction onRemove)
    {
        _teamSelection = teamSelection;
        _addButton.onClick.AddListener(onAdd);
        _removeButton.onClick.AddListener(onRemove);
    }
    
    public void SetSelectedHero(HeroData selectedHero)
    {
        _selectedHero = selectedHero;
        ToggleButtons();
        
        _heroName.text = selectedHero.Name;
        _heroDescription.text = selectedHero.Description;
        _heroRole.text = selectedHero.Role.ToString();
        _abilityName.text = selectedHero.AbilityData.Name;
        _abilityDescription.text = selectedHero.AbilityData.BaseAbilityStats.Description;
        _abilityIcon.sprite = selectedHero.IconSprite;

    }

    public void ToggleButtons()
    {
        var heroInTeam = MainMenuManager.Instance.LevelInitData.HeroData.Contains(_selectedHero);
        
        _addButton.gameObject.SetActive(!heroInTeam);
        _removeButton.gameObject.SetActive(heroInTeam);
        
    }
    
    public void SetAddButtonState(bool isTeamFull)
    {
        _addButton.interactable = !isTeamFull;
    }
    
}
