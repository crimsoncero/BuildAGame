using System;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(HeroData))]
public class HeroDataEditor : Editor
{
    private SerializedProperty _name;
    private SerializedProperty _description;
    private SerializedProperty _role;
    private SerializedProperty _ability;
    private SerializedProperty _messages;
    private SerializedProperty _baseHealth;
    private SerializedProperty _visualsPrefab;
    private SerializedProperty _materialPrefab;
    private SerializedProperty _sprite;

    private void OnEnable()
    {
        _name = serializedObject.FindProperty("<Name>k__BackingField");
        _description = serializedObject.FindProperty("<Description>k__BackingField");
        _role = serializedObject.FindProperty("<Role>k__BackingField");
        _ability = serializedObject.FindProperty("<AbilityData>k__BackingField");
        _messages = serializedObject.FindProperty("<Messages>k__BackingField");
        _baseHealth = serializedObject.FindProperty("<BaseMaxHealth>k__BackingField");
        _visualsPrefab = serializedObject.FindProperty("<VisualsPrefab>k__BackingField");
        _materialPrefab = serializedObject.FindProperty("<MaterialPrefab>k__BackingField");
        _sprite = serializedObject.FindProperty("<CharacterSprite>k__BackingField");
        
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_name);
        EditorGUILayout.PropertyField(_description);
        EditorGUILayout.PropertyField(_role);
        EditorGUILayout.PropertyField(_ability);
        EditorGUILayout.PropertyField(_messages);
        EditorGUILayout.PropertyField(_baseHealth);
        EditorGUILayout.PropertyField(_visualsPrefab);
        EditorGUILayout.PropertyField(_materialPrefab);
        EditorGUILayout.PropertyField(_sprite);
        serializedObject.ApplyModifiedProperties();
        GUILayout.Label((_sprite.objectReferenceValue as Sprite).texture);
    }
}
