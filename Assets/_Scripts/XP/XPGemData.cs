using UnityEngine;
using static XPManager;

[CreateAssetMenu(fileName = "XP Gem Data", menuName = "Scriptable Objects/Pick Ups/XP Gem Data")]
public class XPGemData : ScriptableObject
{
    [Header("Parameters")]
    [field: SerializeField] public XPGemTypes Type { get; private set; }
    
    [Header("Visuals")]
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public Color Color { get; private set; }
    
}
