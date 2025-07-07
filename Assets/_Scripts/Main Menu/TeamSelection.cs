using System.Collections.Generic;
using UnityEngine;

public class TeamSelection : MonoBehaviour
{
    [SerializeField] private WindowHandler _windowHandler;
    [SerializeField] private HeroSelectionIcon _heroSelectionIconPrefab;
    [SerializeField] private List<HeroSelectionIcon> _selectedTeamIcons;
    [SerializeField] private RectTransform _heroGrid;
    
    
    [field:Header("Settings")] 
    [field: SerializeField] public List<HeroData> UnlockedHeroes { get; private set; }
    [field: SerializeField] public int MaxTeamSize { get; private set; }


    public HeroData CurrentSelectedHero { get; private set; }
    
    private List<HeroSelectionIcon> _heroGridList = new List<HeroSelectionIcon>();
    public void Show()
    {
        InitGridHeroIcons();
        InitTeamIcons();
        
        _windowHandler.Show();
    }

    public void SelectHero(HeroData selectedHero)
    {
        HeroSelectionIcon icon;
        if (CurrentSelectedHero != null)
        {
            _heroGridList.Find(p => p.Hero == CurrentSelectedHero).SetSelected(false);
            icon = _selectedTeamIcons.Find(p => p.Hero == CurrentSelectedHero);
            icon?.SetSelected(false);
        }
        
        CurrentSelectedHero = selectedHero;
        
        _heroGridList.Find(p => p.Hero == selectedHero).SetSelected(true);
        icon = _selectedTeamIcons.Find(p => p.Hero == selectedHero);
        icon?.SetSelected(true);
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
        foreach (var icon in _selectedTeamIcons)
        {
            if (icon == null) continue;
            
            icon.InitTeamIcon(this);
            icon.SetLockedTeamIcon();
        }

        for (var i = _selectedTeamIcons.Count - MaxTeamSize - 1; i <= MaxTeamSize; i++)
        {
            _selectedTeamIcons[i].SetOpenTeamIcon();
        }
        
    }
}
