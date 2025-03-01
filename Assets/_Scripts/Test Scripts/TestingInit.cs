using System;
using MoreMountains.Feedbacks;
using UnityEngine;

public class TestingInit : MonoBehaviour
{
    public LevelData LevelData;


    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        LevelManager.Instance.Init(LevelData);
        GameManager.Instance.StartGame();
    }

   
}
