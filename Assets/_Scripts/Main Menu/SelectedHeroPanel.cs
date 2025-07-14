using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectedHeroPanel : MonoBehaviour
{
    [SerializeField] private Button _addButton;
    [SerializeField] private Button _removeButton;
    
    
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
        
        // Add the data to set here.
        
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
