using UnityEngine;


/// <summary>
/// Read only base stats of the hero units.
/// </summary>
[CreateAssetMenu(fileName = "HeroData", menuName = "Scriptable Objects/HeroData")]
public class HeroData : ScriptableObject
{
    [field:Header("General")]
    public string Name { get; private set; }


    [field:Header("Base Stats")]
    [field:SerializeField]
    public int BaseMaxHealth { get; private set; }
    [field: SerializeField]
    public float BaseMaxSpeed { get; private set; } = 6f;
    [field: SerializeField]
    public int BasePower { get; private set; }
    [field: SerializeField]
    public float BaseAttackSpeed { get; private set; }



}
