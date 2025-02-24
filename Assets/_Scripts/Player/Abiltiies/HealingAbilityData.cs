using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Healing Data", menuName = "Scriptable Objects/Abilities/Healing Data")]
public class HealingAbilityData : BaseAbilityData
{
    

    public override BaseAbility CreateAbilityComponent(Transform abilityObject)
    {
        HealingAbility ability = abilityObject.AddComponent<HealingAbility>();
        return ability;
    }
}
