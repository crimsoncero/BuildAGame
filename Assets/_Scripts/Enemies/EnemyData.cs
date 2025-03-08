using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [field: Header("General")]
    [field: SerializeField]
    public string Name { get; private set; }


    [field: Header("Base Stats")]
    [field: SerializeField]
    public int BaseMaxHealth { get; private set; }
    [field: SerializeField]
    public float BaseMoveSpeed { get; private set; } = 4f;
    [field: SerializeField]
    public int BasePower { get; private set; }
    [field: SerializeField]
    public float BaseSpeed { get; private set; }
    [field: SerializeField]
    public XPManager.XPGemTypes GemDropped { get; private set; }
    
    [field: Header("Visuals")]
    [field: SerializeField]
    public Sprite Sprite { get; private set; }
    [field: SerializeField]
    public bool IsBoss { get; private set; }

}
