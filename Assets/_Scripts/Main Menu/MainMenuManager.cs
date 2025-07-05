using UnityEngine;

public class MainMenuManager : Singleton<MainMenuManager>
{
    [field: SerializeField] public DataCollections DataCollections { get; private set; }
    [field: SerializeField] public LevelInitData LevelInitData { get; private set; }
    [SerializeField] private LevelSelection _levelSelection;
    
    public void OpenLevelSelection()
    {
        _levelSelection.Show();
    }

    public void OpenTeamSelection()
    {
        
    }
}
