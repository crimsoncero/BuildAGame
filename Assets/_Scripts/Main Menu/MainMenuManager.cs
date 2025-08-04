using UnityEngine;

public class MainMenuManager : Singleton<MainMenuManager>
{
    [field: SerializeField] public DataCollections DataCollections { get; private set; }
    [field: SerializeField] public LevelInitData LevelInitData { get; private set; }
    [SerializeField] private LevelSelection _levelSelection;
    [SerializeField] private TeamSelection _teamSelection;
    [SerializeField] private WindowHandler _creditsHandler;
    [SerializeField] private WindowHandler _upgradesHandler;
    [SerializeField] private WindowHandler _settingsHandler;
    [SerializeField] private WindowHandler _achievmentsHandler;
    

    public void CloseAll()
    {
        _levelSelection.WindowHandler.Hide();
        _teamSelection.WindowHandler.Hide();
        _creditsHandler.Hide();
        _upgradesHandler.Hide();
        _settingsHandler.Hide();
        _achievmentsHandler.Hide();
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

    public void OpenUpgrades()
    {
        CloseAll();
        _upgradesHandler.Show();
    }

    public void OpenSettings()
    {
        CloseAll();
        _settingsHandler.Show();
    }

    public void OpenAchievements()
    {
        CloseAll();
        _achievmentsHandler.Show();
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
