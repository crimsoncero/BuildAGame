using UnityEngine;
using static XPManager;

[CreateAssetMenu(fileName = "XP Gem Data", menuName = "Scriptable Objects/Pick Ups/XP Gem Data")]
public class XPGemData : ScriptableObject
{
    [Header("Parameters")]
    [field: SerializeField] public XPGemTypes Type { get; private set; }
    
    //[Header("Visuals")]

}
