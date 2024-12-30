using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



[CreateAssetMenu(fileName = "WaveSO", menuName = "Scriptable Objects/Level/WaveSO")]
public class WaveSO : ScriptableObject
{
    [Serializable]
    public class EnemyInfo
    {
        public string Name; // Change to the enemy SO
        public int SpawnChance;
    }
       
    public List<EnemyInfo> EnemyList;


    // Check if the sum of the weights is 100, so the designer can be happy.
    [HideInInspector] public bool _isValid = false;


    private void OnValidate()
    {
        int sumWeights = 0;

        if (_isValid) { } // Remove warnings for not used;
        foreach (EnemyInfo enemy in EnemyList)
        {
            sumWeights += enemy.SpawnChance;
        }
        if(sumWeights == 100)
            _isValid = true;
        else
            _isValid = false;
    }

}
