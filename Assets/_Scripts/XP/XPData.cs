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
    [field: SerializeField] public int BaseXpToLevelUp { get; private set; } = 1000;

    [field: SerializeField] public int XPAddedPerLevel { get; private set; } = 500;



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
        var xpNeeded = BaseXpToLevelUp + currentLevel * XPAddedPerLevel;
        
        return Mathf.FloorToInt(xpNeeded);
    }

    
}
