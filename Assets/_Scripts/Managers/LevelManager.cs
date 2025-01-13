using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{

    public LevelData Data { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        AddEvents();
    }

    private void OnDestroy()
    {
        RemoveEvents();
    }

    
    public void Init(LevelData data)
    {
        Data = data;
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
