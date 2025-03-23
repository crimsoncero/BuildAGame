using System;
using MoreMountains.Tools;
using UnityEditor;
using UnityEngine;

public enum LevelSceneEnum
{
    Dev,
    Menu,
}
public class SceneTransitionManager : MonoBehaviour
{
    public const string MainMenuScene = "Start";
    public const string DevLevelScene = "Dev Level";
    
    [SerializeField] private MMAdditiveSceneLoadingManagerSettings _settings;
    
    public void LoadScene(LevelSceneEnum scene)
    {
        MMAdditiveSceneLoadingManager.LoadScene(GetSceneName(scene), _settings);
    }
    private string GetSceneName(LevelSceneEnum scene)
    {
        switch(scene)
        {
            case LevelSceneEnum.Dev:
                return DevLevelScene;
            case LevelSceneEnum.Menu:
                return MainMenuScene;
            default:
                throw new ArgumentOutOfRangeException(nameof(scene), scene, null);
        }
    }


    public void TempLoadDev()
    {
        LoadScene(LevelSceneEnum.Dev);
    }
    
}
