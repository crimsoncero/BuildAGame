using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Orbital Data", menuName = "Scriptable Objects/Abilities/Orbital Data")]
public class OrbitalAbilityData : BaseAbilityData
{
    [field: Header("Behavior Stats")]
    [field: SerializeField] public float Duration {  get; private set; }
    [field: SerializeField] public float Radius {  get; private set; }
    [field: SerializeField] public float SpeedMultipliar {  get; private set; }
    
    [field: Header("Pooling")]
    [field: SerializeField] public OrbitalProjectile ProjectilePrefab {  get; private set; }
    [field: SerializeField] public int InitCount {  get; private set; }


    public override BaseAbility CreateAbilityComponent(Transform abilityObject)
    {
        OrbitalAbility ability = abilityObject.AddComponent<OrbitalAbility>();
        return ability;
    }
}
