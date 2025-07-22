using UnityEngine;

public class MainMenuManager : Singleton<MainMenuManager>
{
    [field: SerializeField] public DataCollections DataCollections { get; private set; }
    [field: SerializeField] public LevelInitData LevelInitData { get; private set; }
    [SerializeField] private LevelSelection _levelSelection;
    [SerializeField] private TeamSelection _teamSelection;
    [SerializeField] private WindowHandler _creditsHandler;

    public void CloseAll()
    {
        _levelSelection.WindowHandler.Hide();
        _teamSelection.WindowHandler.Hide();
        _creditsHandler.Hide();
    }
    
    public void OpenLevelSelection()
    {
        CloseAll();
        LevelInitData.HeroData.Clear();
        LevelInitData.LevelData = null;
        _levelSelection.Show();
    }
    public void OpenTeamSelection()
    {
        _teamSelection.Show();        
    }

    public void OpenCredits()
    {
        CloseAll();
        _creditsHandler.Show();
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
