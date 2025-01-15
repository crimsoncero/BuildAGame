using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WaveData))]
public class WaveSoEditor :Editor
{
    SerializedProperty waveList;
    SerializedProperty isValid;


    void OnEnable()
    {
        waveList = serializedObject.FindProperty("EnemyList");
        isValid = serializedObject.FindProperty("_isValid");
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //serializedObject.Update();
        //EditorGUILayout.PropertyField(waveList);
        //serializedObject.ApplyModifiedProperties();

        if(!isValid.boolValue)
            EditorGUILayout.HelpBox("ERROR", MessageType.Error);

    }
}
