using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



[CreateAssetMenu(fileName = "WaveData", menuName = "Scriptable Objects/Level/WaveData")]
public class WaveData : ScriptableObject
{
    [Serializable]
    public class EnemyInfo
    {
        public EnemyData Data; 
        public int Count;
    }
    [Header("Spawn Settings")]
    [Tooltip("The cooldown in seconds between spawns")]
    public int SpawnTime;
    [Tooltip("How many groups spawn together")]
    public int SpawnGroups;
    [Tooltip("The size of each spawn group")]
    public int GroupSize;


    public List<EnemyInfo> EnemyList;
    // Check if the sum of the weights is 100, so the designer can be happy.
    [HideInInspector] public bool _isValid = false;


    /// <summary>
    /// return a matrix of the groups to spawn in the current pulse.
    /// </summary>
    /// <returns></returns>
    public List<EnemyData> GetSpawnGroup()
    {
        if (!IsDataValid())
            throw new Exception("Total group size doesn't match the specified group size");


       List<EnemyData> spawnGroup = new List<EnemyData>();

       foreach(EnemyInfo enemy in EnemyList)
       {
           for(int k = 0; k < enemy.Count; k++)
           {
               spawnGroup.Add(enemy.Data);
           }
       }


        return spawnGroup;
    }



    private void OnValidate()
    {
        _isValid = IsDataValid();
    }

    private bool IsDataValid()
    {
        int enemyCount = 0;

        foreach(EnemyInfo enemyInfo in EnemyList)
        {
            enemyCount += enemyInfo.Count;
        }

        if (enemyCount == GroupSize)
            return true;
        else
            return false;
    }
}
