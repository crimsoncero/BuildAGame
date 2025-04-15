using UnityEngine;

[CreateAssetMenu(fileName = "Hero Base Stats", menuName = "Scriptable Objects/Hero Base Stats")]
public class HeroBaseStats : ScriptableObject
{
    [SerializeField] public int MaxHealth = 0;
    [SerializeField] public int Damage = 0;
    [SerializeField] public int Count = 0;
    [SerializeField] public float Speed = 0;
    [SerializeField] public float Cooldown = 0;
    [SerializeField] public int Pierce = 0;
    [SerializeField] public float RespawnTime = 15f;
    [SerializeField] public float MovementSpeed = 6f;
    [SerializeField] public int UpgradeCount = 3;
}
