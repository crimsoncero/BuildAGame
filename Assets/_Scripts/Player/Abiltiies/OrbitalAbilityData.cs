using UnityEngine;

[CreateAssetMenu(fileName = "Orbital", menuName = "Scriptable Objects/Abilities/Orbital")]
public class OrbitalAbilityData : ScriptableObject
{
    [SerializeField] private float _radius;
    [SerializeField] private float _cooldown;
    [SerializeField] private float _lifeTime;

    private BoolTimer _spawnTimer;




}
