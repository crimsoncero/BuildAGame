using System;
using System.Collections.Generic;
using MoreMountains.Tools;
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
        public EnemyData Data; 
        [Tooltip("The spawn time of the boss in seconds.")]
        public int SpawnTime;
    }

    [Serializable]
    public class EventInfo
    {
        public EventData Data;
        [Tooltip("The spawn time of the event in seconds.")]
        public int SpawnTime;
    }

    [Header("General")] 
    public string Name;
    public int Duration;
    
    [Header("Enemies")]
    public List<WaveInfo> WaveList;
    public List<BossInfo> BossList;
    public List<EventInfo> EventList;

    [Header("Art")]
    public MMSMPlaylist BGM;

    public Material UnitMaterial;


}
