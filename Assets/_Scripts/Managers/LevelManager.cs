using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static LevelData;
using System.Linq;
using MoreMountains.Tools;
using static WaveData;
using Unity.VisualScripting;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private EnemySpawner _enemySpawner;


    
    [field: SerializeField] public LevelData Data { get; private set; }

    // Using IEnumerator to keep track of the current item in the list without a need for an indexer.SS
    public IEnumerator<WaveInfo> WaveList { get; private set; }
    public IEnumerator<BossInfo> BossList { get; private set; }
    public IEnumerator<EventInfo> EventList { get; private set; }

    private List<EnemyData> _currentSpawnGroup;

    private int _spawnTarget = 0;
    private int _waveChangeTarget = 0;
    private int _bossTarget = 0;
    
    private void Start()
    {
        AddEvents();

    }

    public void Init(LevelData data)
    {
        Data = data;
        SetLists();
        _currentSpawnGroup = WaveList.Current.Wave.GetSpawnGroup();
        _spawnTarget = 0;
        _waveChangeTarget = 0 + WaveList.Current.Duration;
        if (BossList != null)
            _bossTarget = 0 + BossList.Current.SpawnTime;
        else
            _bossTarget = -1;
    }

    public void Init()
    {
        Init(Data);
    }


    private void CheckTimer(int time)
    {
        // Change Wave
        if(time == _waveChangeTarget)
        {
            WaveList.MoveNext();
            if (WaveList.Current != null)
            {
                _currentSpawnGroup = WaveList.Current.Wave.GetSpawnGroup();
                _spawnTarget = time;
                _waveChangeTarget = time + WaveList.Current.Duration;
            }
            else
            {
                // if the wave is null, we stop spawning groups.
                _spawnTarget = -1;
            }
        }

        // Spawn Wave
        if(time == _spawnTarget)
        {
            _enemySpawner.SpawnWave(_currentSpawnGroup, WaveList.Current.Wave.SpawnGroups);
            _spawnTarget = time + WaveList.Current.Wave.SpawnTime;
        }

        // Spawn Boss
        if(time == _bossTarget)
        {
            _enemySpawner.SpawnEnemy(BossList.Current.Data);
            BossList.MoveNext();
            _bossTarget = BossList.Current.SpawnTime;
        }

    }

    private void AddEvents()
    {
        GameManager.Instance.OnTimerTick += CheckTimer;
        GameManager.Instance.OnGameStart += StartBGM;
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
        if(Data.BossList.Count > 0)
        {
            BossList = Data.BossList.OrderBy((t) => t.SpawnTime).GetEnumerator();
            BossList.MoveNext();
        }
        else
        {
            BossList = null;
        }
       
        EventList = Data.EventList.OrderBy((t) => t.SpawnTime).GetEnumerator();
        EventList.MoveNext();
    }

    private void StartBGM()
    {
        MMPlaylistPlayEvent.Trigger(0);
    }
}
