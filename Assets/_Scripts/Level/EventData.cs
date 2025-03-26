using UnityEngine;


public abstract class EventData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }


    public abstract void Play();
}


