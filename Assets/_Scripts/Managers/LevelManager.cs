using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    protected override void Awake()
    {
        base.Awake();
        AddEvents();
    }

    private void OnDestroy()
    {
        RemoveEvents();
    }



    private void CheckTimer(int time)
    {

    }

    private void AddEvents()
    {
        GameManager.Instance.OnTimerTick += CheckTimer;
    }
    private void RemoveEvents()
    {
        GameManager.Instance.OnTimerTick -= CheckTimer;
    }
}
