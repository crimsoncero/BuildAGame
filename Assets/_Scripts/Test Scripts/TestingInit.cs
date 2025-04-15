using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

public class TestingInit : MonoBehaviour
{
    public LevelData LevelData;

    private void Start()
    {
        StartGame();
    }

    public void Init()
    {
        
    }
    public void StartGame()
    {
        LevelManager.Instance.Init(LevelData);
        GameManager.Instance.StartGame();
    }

}
