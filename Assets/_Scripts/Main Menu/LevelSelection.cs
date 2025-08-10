using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private WindowHandler _windowHandler;
    [SerializeField] private List<LevelData> _unlockedLevels;
    [SerializeField] private Image _levelPreviewImage;
    [SerializeField] private TMP_Text _levelName;
    [SerializeField] private Button _selectButton;
    
    
    [Header("Settings")] 
    [SerializeField] private Sprite _lockedPreviewImage;


    [SerializeField] private List<(LevelData, bool)> _levelList;
    private int _currentLevelIndex;
    
    public WindowHandler WindowHandler => _windowHandler;
    
    public void Show()
    {
        if (_windowHandler.IsOpen)
            return; 
        InitLevelList();
        SetLevelImage();
        _windowHandler.Show();
    }

    public void OnSelectLevel()
    {
        MainMenuManager.Instance.LevelInitData.LevelData = _levelList[_currentLevelIndex].Item1;
        MainMenuManager.Instance.OpenTeamSelection();
        _windowHandler.Hide();
    }
    
    public void ChangeLevel(bool forward)
    {
        int change = forward ? -1 : 1;
        _currentLevelIndex += change;
        
        if (_currentLevelIndex < 0)
            _currentLevelIndex = _levelList.Count - 1;
        
        if (_currentLevelIndex >= _levelList.Count)
            _currentLevelIndex = 0;
        
        SetLevelImage();
    }
    
    private void SetLevelImage()
    {
        (LevelData level, bool unlocked) = _levelList[_currentLevelIndex];
        
        _levelName.text = level.Name;
        // var image = unlocked ? level.PreviewImage : _lockedPreviewImage;
        var image = level.PreviewImage;
        if(image != null)
            _levelPreviewImage.sprite = image;

        _selectButton.interactable = unlocked;
    }
    private void InitLevelList()
    {
        _levelList = new List<(LevelData, bool)>();
        
        foreach (LevelData level in MainMenuManager.Instance.DataCollections.Levels)
        {
            var unlocked = _unlockedLevels.Contains(level);
            _levelList.Add((level, unlocked));
        }
        
        _currentLevelIndex = 0;
    }
    
}
