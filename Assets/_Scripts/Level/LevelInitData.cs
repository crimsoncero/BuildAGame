using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Init Data", menuName = "Scriptable Objects/Level Init Data")]
public class LevelInitData : ScriptableObject
{
    public LevelData LevelData;
    public List<HeroData> HeroData;
}
