using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Missile Data", menuName = "Scriptable Objects/Abilities/Missile Data")]
public class MissileAbilityData : BaseAbilityData
{
    [field: Header("Stats")]
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public float Range {  get; private set; }
    [field: SerializeField] public float Cooldown {  get; private set; }
    [field: SerializeField] public float Speed {  get; private set; }
    [field: SerializeField] public int Count {  get; private set; }
    [field: SerializeField] public int Pierce {  get; private set; }
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
