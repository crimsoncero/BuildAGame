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
    [field: SerializeField, Min(1)] public float MaxLevelMultiplier { get; private set; } = 2f;
    [field: SerializeField] public float EasingFactor { get; private set; } = 1.5f;
    [field: SerializeField] public int MaxLevelScaled { get; private set; } = 100;


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
        var levelRatio = (float)currentLevel / (float)MaxLevelScaled;
        var xpNeededF = BaseXpToLevelUp * MultiplierCurve(levelRatio);
        
        return Mathf.FloorToInt(xpNeededF);
    }

    private float MultiplierCurve(float t)
    {
        t = Mathf.Clamp01(t);
        var p = Mathf.Pow(t, EasingFactor);
        return (MaxLevelMultiplier - 1) * p / (p + Mathf.Pow((1 - t), EasingFactor)) + 1f;
    }
}
