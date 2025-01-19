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
    public float BaseMaxSpeed { get; private set; } = 4f;
    [field: SerializeField]
    public int BasePower { get; private set; }

}
