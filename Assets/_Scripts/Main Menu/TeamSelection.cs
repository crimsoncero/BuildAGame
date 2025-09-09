using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TeamSelection : MonoBehaviour
{
    [SerializeField] private WindowHandler _windowHandler;
    [SerializeField] private HeroSelectionIcon _heroSelectionIconPrefab;
    [FormerlySerializedAs("_TeamIcons")] [FormerlySerializedAs("_selectedTeamIcons")] [SerializeField] private List<HeroSelectionIcon> _AllTeamIcons;
    [SerializeField] private RectTransform _heroGrid;
    [SerializeField] private SelectedHeroPanel _selectedHeroPanel;
    [SerializeField] private Button _startButton;
    
    [field:Header("Settings")] 
    [field: SerializeField] public List<HeroData> UnlockedHeroes { get; private set; }
    [field: SerializeField] public int MaxTeamSize { get; private set; }

    public WindowHandler WindowHandler => _windowHandler;
    public HeroData CurrentSelectedHero { get; private set; }
    
    private List<HeroSelectionIcon> _heroGridList = new List<HeroSelectionIcon>();
    private List<HeroSelectionIcon> _teamIcons = new List<HeroSelectionIcon>();
    
    public int CurrentTeamSize
    {
        get { return _teamIcons.Count(p => p.Hero != null); }
    }
    private void Start()
    {
        _selectedHeroPanel.Init(this, AddHeroToTeam, RemoveHeroFromTeam);
    }

    public void Show()
    {
        InitGridHeroIcons();
        InitTeamIcons();
        SetStartButtonState();
        _windowHandler.Show();
        
        SelectHero(_heroGridList[0].Hero);

    }

    public void SelectHero(HeroData selectedHero)
    {
        HeroSelectionIcon icon;
        if (CurrentSelectedHero != null)
        {
            _heroGridList.Find(p => p.Hero == CurrentSelectedHero).SetSelected(false);
            icon = _AllTeamIcons.Find(p => p.Hero == CurrentSelectedHero);
            icon?.SetSelected(false);
        }
        
        CurrentSelectedHero = selectedHero;
        _selectedHeroPanel.SetSelectedHero(selectedHero);
        _heroGridList.Find(p => p.Hero == selectedHero).SetSelected(true);
        icon = _AllTeamIcons.Find(p => p.Hero == selectedHero);
        icon?.SetSelected(true);
    }
    
    public void AddHeroToTeam()
    {
        if (MainMenuManager.Instance.LevelInitData.HeroData.Contains(CurrentSelectedHero))
            return;
        var icon = _teamIcons.Find(p => p.Hero == null);
        icon.SetTeamIcon(CurrentSelectedHero);
        icon.SetSelected(true);
        
        MainMenuManager.Instance.LevelInitData.HeroData.Add(CurrentSelectedHero);
        _selectedHeroPanel.SetAddButtonState(CurrentTeamSize >= MaxTeamSize);
        _selectedHeroPanel.ToggleButtons();
        SetStartButtonState();
    }

    public void RemoveHeroFromTeam()
    {
        var icon = _teamIcons.Find(p => p.Hero == CurrentSelectedHero);
        icon.SetSelected(false);
        icon.SetOpenTeamIcon();
        
        MainMenuManager.Instance.LevelInitData.HeroData.Remove(CurrentSelectedHero);
        _selectedHeroPanel.SetAddButtonState(CurrentTeamSize >= MaxTeamSize);
        _selectedHeroPanel.ToggleButtons();
        SetStartButtonState();
    }

    private void InitGridHeroIcons()
    {
        _heroGrid.DestroyChildren();
        
        _heroGridList = new List<HeroSelectionIcon>();
        
        foreach (HeroData hero in MainMenuManager.Instance.DataCollections.Heroes)
        {
            var unlocked = UnlockedHeroes.Contains(hero);
            HeroSelectionIcon i = Instantiate(_heroSelectionIconPrefab, _heroGrid);
            i.InitGridIcon(this, hero, unlocked);
            _heroGridList.Add(i);
        }
    }

    private void InitTeamIcons()
    {
        _teamIcons.Clear();
        
        foreach (var icon in _AllTeamIcons)
        {
            if (icon == null) continue;
            
            icon.InitTeamIcon(this);
            icon.SetLockedTeamIcon();
        }

        for (var i = 0; i < MaxTeamSize; i++)
        {
            _AllTeamIcons[i].SetOpenTeamIcon();
            _teamIcons.Add(_AllTeamIcons[i]);
        }
    }

    private void SetStartButtonState()
    {
        if(CurrentTeamSize > MaxTeamSize)
            throw new ArgumentOutOfRangeException($"Team size can't be greater than max team size");
        
        _startButton.interactable = CurrentTeamSize >= 3;
        
    }
}
