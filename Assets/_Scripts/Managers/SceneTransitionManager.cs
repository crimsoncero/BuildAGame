using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LevelSceneEnum
{
    Dev,
    Menu,
}
public class SceneTransitionManager : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private SceneAsset _menuLevelScene;
    [SerializeField] private SceneAsset _devLevelScene;
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
                return _devLevelScene.name;
            case LevelSceneEnum.Menu:
                return _menuLevelScene.name;
            default:
                throw new ArgumentOutOfRangeException(nameof(scene), scene, null);
        }
    }


    public void TempLoadDev()
    {
        LoadScene(LevelSceneEnum.Dev);
    }
    
}
