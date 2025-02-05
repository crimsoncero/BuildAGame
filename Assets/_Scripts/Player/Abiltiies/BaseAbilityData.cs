using UnityEngine;

public abstract class BaseAbilityData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }

    public abstract BaseAbility CreateAbilityComponent(Transform abilityObject);
}
