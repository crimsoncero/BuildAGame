using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Missile Data", menuName = "Scriptable Objects/Abilities/Missile Data")]
public class MissileAbilityData : BaseAbilityData
{
    [field: Header("Behavioral Stats")]
    [field: SerializeField] public float Range {  get; private set; }
    [field: SerializeField] public float SpeedMultipliar {  get; private set; }
    [field: SerializeField] public LayerMask EnemyLayer {  get; private set; }
    
    [field: Header("Pooling")]
    [field: SerializeField] public MissileProjectile ProjectilePrefab {  get; private set; }
    [field: SerializeField] public int InitCount {  get; private set; }


    public override BaseAbility CreateAbilityComponent(Transform abilityObject)
    {
        MissileAbility ability = abilityObject.AddComponent<MissileAbility>();
        return ability;
    }
}
