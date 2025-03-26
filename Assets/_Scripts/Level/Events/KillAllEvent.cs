using UnityEngine;

[CreateAssetMenu(fileName = "Kill All Event", menuName = "Scriptable Objects/Level/Events/Kill All")]
public class KillAllEvent : EventData
{
    public override void Play()
    {
        foreach (var enemy in EnemySpawner.Instance.EnemyList)
        {
            enemy.KillUnit(false);
        }
    }
}
