using System.Collections.Generic;
using UnityEngine;

public class TeamSelection : MonoBehaviour
{
    [SerializeField] private WindowHandler _windowHandler;
    [SerializeField] private HeroSelectionIcon _heroSelectionIconPrefab;
    [SerializeField] private HeroSelectionIcon[] _selectedTeamIcons;
    [SerializeField] private RectTransform _heroGrid;
    
    
    [field:Header("Settings")] 
    [field: SerializeField] public List<HeroData> UnlockedHeroes { get; private set; }
    [field: SerializeField] public int MaxTeamSize { get; private set; }
    
    
    private List<HeroSelectionIcon> _heroList = new List<HeroSelectionIcon>();
    public void Show()
    {
        InitGridHeroIcons();
        InitTeamIcons();
        
        _windowHandler.Show();
    }


    private void InitGridHeroIcons()
    {
        _heroGrid.DestroyChildren();
        
        _heroList = new List<HeroSelectionIcon>();
        
        foreach (HeroData hero in MainMenuManager.Instance.DataCollections.Heroes)
        {
            var unlocked = UnlockedHeroes.Contains(hero);
            HeroSelectionIcon i = Instantiate(_heroSelectionIconPrefab, _heroGrid);
            i.InitGridIcon(hero, unlocked);
            _heroList.Add(i);
        }
    }

    private void InitTeamIcons()
    {
        foreach (var icon in _selectedTeamIcons)
        {
            icon.SetLockedTeamIcon();
        }

        for (var i = _selectedTeamIcons.Length - MaxTeamSize - 1; i <= MaxTeamSize; i++)
        {
            _selectedTeamIcons[i].SetOpenTeamIcon();
        }
        
    }
}
