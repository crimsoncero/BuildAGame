using UnityEngine;

// BaseAbilitySO
// AbilityInstance


/// <summary>
/// Read only base stats of the hero units.
/// </summary>
[CreateAssetMenu(fileName = "HeroData", menuName = "Scriptable Objects/HeroData")]
public class HeroData : ScriptableObject
{
    [field:Header("General")]
    [field: SerializeField]
    public string Name { get; private set; }

   

    [field:Header("Base Stats")]
    [field:SerializeField]
    public int BaseMaxHealth { get; private set; }
    [field: SerializeField]
    public float BaseMoveSpeed { get; private set; } = 6f;
    [field: SerializeField]
    public int BasePower { get; private set; }
    [field: SerializeField]
    public float BaseSpeed { get; private set; }
    [field: SerializeField]
    public float BaseCooldown { get; private set; }
    [field: SerializeField]
    public int BaseRecovery { get; private set; }

    [field: Header("Visuals")]
    [field: SerializeField]
    public Sprite Sprite { get; private set; }

}
