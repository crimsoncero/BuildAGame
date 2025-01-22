using UnityEngine;

public class TestingInit : MonoBehaviour
{
    public LevelData LevelData;

    private void Start()
    {
        LevelManager.Instance.Init(LevelData);
        GameManager.Instance.StartGame();
    }
}
