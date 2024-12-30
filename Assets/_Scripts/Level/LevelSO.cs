using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSO", menuName = "Scriptable Objects/Level/LevelSO")]


public class LevelSO : ScriptableObject
{
    [Serializable]
    public class WaveInfo
    {
        public WaveSO Wave;
        [Tooltip("The duration of the wave in seconds.")]
        public int Duration;
    }

    [Serializable]
    public class BossInfo
    {
        public string Name; // Change with enemy SO
        [Tooltip("The spawn time of the boss in seconds.")]
        public int SpawnTime;
    }

    public List<WaveInfo> WaveList;
    public List<BossInfo> BossList;




}
