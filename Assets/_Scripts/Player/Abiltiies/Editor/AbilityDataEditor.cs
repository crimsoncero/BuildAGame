using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(BaseAbilityData))]
public class AbilityDataEditor<T> : Editor where T : BaseAbilityData.AbilityStats
{
    SerializedProperty _baseStats;
    SerializedProperty _levelUpgrades;

    void OnEnable()
    {
        _baseStats = serializedObject.FindProperty("_baseStats");
        _levelUpgrades = serializedObject.FindProperty("_levelUpgrades");

    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    
    
}


[CustomEditor(typeof(OrbitalAbilityData))]
public class OrbitalAbilityDataEditor : AbilityDataEditor<OrbitalAbilityData.OrbitalStats>
{
    
}