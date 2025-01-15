using System.IO;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(DataPersistenceManager))]
[CanEditMultipleObjects]
public class DataPersistenceEditor : Editor
{

    private SerializedProperty _fileName;
    private SerializedProperty _useEncryption;
    private SerializedProperty _encryptionCodeWord;

    private void OnEnable()
    {
        _fileName = serializedObject.FindProperty("_fileName");
        _useEncryption = serializedObject.FindProperty("_useEncryption");
        _encryptionCodeWord = serializedObject.FindProperty("_encryptionCodeWord");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_fileName);
        EditorGUILayout.PropertyField(_useEncryption);

        if (_useEncryption.boolValue)
        {
            EditorGUILayout.PropertyField(_encryptionCodeWord);
        }


        serializedObject.ApplyModifiedProperties();
    }

}
