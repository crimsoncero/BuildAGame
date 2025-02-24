using MoreMountains.Feedbacks;
using UnityEngine;

public class TestingInit : MonoBehaviour
{
    public LevelData LevelData;

 
    private void TestUpgrade()
    {
        var test = PlayerController.Instance.GetUpgradesToShow();
        foreach (var t in test)
        {
            Debug.Log(t.upgradeInfo.Description);
        }
    }

    private void StartGame()
    {
        LevelManager.Instance.Init(LevelData);
        GameManager.Instance.StartGame();
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10,10,150,100), "Start Game"))
        {
            StartGame(); 
        }
        
    }
}
