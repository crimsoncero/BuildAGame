using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data Collections", menuName = "Scriptable Objects/Data Collections")]
public class DataCollections : ScriptableObject
{
    [field: SerializeField] public List<LevelData> Levels { get; private set; }
    [field: SerializeField] public List<HeroData> Heroes { get; private set; }
}
