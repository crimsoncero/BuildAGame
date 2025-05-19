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

    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }

}
