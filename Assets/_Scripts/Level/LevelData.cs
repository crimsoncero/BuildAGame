using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/Level/LevelData")]


public class LevelData : ScriptableObject
{
    [Serializable]
    public class WaveInfo
    {
        public WaveData Wave;
        [Tooltip("The duration of the wave in seconds.")]
        public int Duration;
    }

    [Serializable]
    public class BossInfo
    {
        public EnemyData BossData; // Change with enemy SO
        [Tooltip("The spawn time of the boss in seconds.")]
        public int SpawnTime;
    }

    public List<WaveInfo> WaveList;
    public List<BossInfo> BossList;




}
