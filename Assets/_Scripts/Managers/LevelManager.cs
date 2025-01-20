using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static LevelData;
using System.Linq;
using static WaveData;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private EnemySpawner _enemySpawner;



    public LevelData Data { get; private set; }

    // Using IEnumerator to keep track of the current item in the list without a need for an indexer.SS
    public IEnumerator<WaveInfo> WaveList { get; private set; }
    public IEnumerator<BossInfo> BossList { get; private set; }
    public IEnumerator<EventInfo> EventList { get; private set; }

    private List<EnemyData> _currentSpawnGroup;



    private void Start()
    {
        AddEvents();
    }

    public void Init(LevelData data)
    {
        Data = data;
        SetLists();
        _currentSpawnGroup = WaveList.Current.Wave.GetSpawnGroup();

        string check = "";
        foreach(EnemyData enemy in _currentSpawnGroup)
        {
            check += enemy.Name + ", ";
        }
        Debug.Log(check);
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


    private void SetLists()
    {
        if (Data == null) return;

        WaveList = Data.WaveList.GetEnumerator();
        WaveList.MoveNext();
        BossList = Data.BossList.OrderBy((t) => t.SpawnTime).GetEnumerator();
        BossList.MoveNext();
        EventList = Data.EventList.OrderBy((t) => t.SpawnTime).GetEnumerator();
        EventList.MoveNext();
    }
}
