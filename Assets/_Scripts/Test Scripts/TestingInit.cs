using System;
using MoreMountains.Tools;
using UnityEngine;

public class TestingInit : MonoBehaviour
{
    public LevelData LevelData;

    public void StartGame()
    {
        LevelManager.Instance.Init(LevelData);
        GameManager.Instance.StartGame();
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30), "Start Game"))
        {
            StartGame();
        }
    }
}
