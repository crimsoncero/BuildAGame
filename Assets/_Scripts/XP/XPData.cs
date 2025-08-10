using System;
using UnityEngine;

[CreateAssetMenu(fileName = "XP Data", menuName = "Scriptable Objects/XPData")]
public class XPData : ScriptableObject
{
    [field: Header("XP Gem Amounts")]
    [field: SerializeField] public int BaseGemAmount { get; private set; } = 100;
    [field: SerializeField] public int RareGemMultiplier { get; private set; } = 5;
    [field: SerializeField] public int EpicGemMultiplier { get; private set; } = 5;

    [field: Space]
    [field: Header("Levels Data")]
    [field: SerializeField] public float GemsPerPoint  { get; private set; } = 2;



    public int GetGemXpAmount(XPManager.XPGemTypes type)
    {
        switch (type)
        {
            case XPManager.XPGemTypes.Common:
                return BaseGemAmount;
            case XPManager.XPGemTypes.Rare:
                return BaseGemAmount * RareGemMultiplier;
            case XPManager.XPGemTypes.Epic:
                return BaseGemAmount * RareGemMultiplier * EpicGemMultiplier;
            case XPManager.XPGemTypes.Unique:
                return 0;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }    
    
    public int CalculateXpNeededToLevelUp(int currentLevel)
    {
        var level = currentLevel + 1;
        var pointsNeeded = (Mathf.Pow(level, 2) - level) / 2f;
        var baseGemsNeeded = pointsNeeded * GemsPerPoint;
        var xpNeeded = baseGemsNeeded * BaseGemAmount;
        
        return Mathf.CeilToInt(xpNeeded);
    }

    
}
