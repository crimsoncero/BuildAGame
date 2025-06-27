using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MessagesData", menuName = "Scriptable Objects/Messages Data")]
public class MessagesData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    
    [SerializeField, Multiline] private List<string> _messages;
    public List<string> Messages { get { return _messages; } }

    public string GetRandomMessage()
    {
        return _messages.GetRandom();
    }
}
