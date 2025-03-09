using System;
using MoreMountains.Tools;
using UnityEngine;

public class TestingInit : MonoBehaviour
{
    public LevelData LevelData;
    [SerializeField] private Canvas _mainmenu;
    [SerializeField] private MMSMPlaylist _playlist;

   

    public void StartGame()
    {
        LevelManager.Instance.Init(LevelData);
        GameManager.Instance.StartGame();
        CloseMenu();
    }

    public void CloseMenu()
    {
        _mainmenu.gameObject.SetActive(false);
    }

    public void OpenMenu()
    {
        _mainmenu.gameObject.SetActive(true);
    }

    
}
