using UnityEngine;

// BaseAbilitySO
// AbilityInstance


public enum RoleEnum
{
    Developer,
    Designer,
    Artist,
    QA,
    Producer,
}


/// <summary>
/// Read only base stats of the hero units.
/// </summary>
[CreateAssetMenu(fileName = "HeroData", menuName = "Scriptable Objects/HeroData")]
public class HeroData : ScriptableObject
{
    [field:Header("General")]
    [field: SerializeField]
    public string Name { get; private set; }
    [field:SerializeField][TextArea]
    public string Description { get; private set; }
    [field: SerializeField]
    public RoleEnum Role { get; private set; }
    [field: SerializeField]
    public BaseAbilityData AbilityData { get; private set; }
    
    [field: SerializeField]
    public MessagesData Messages { get; private set; }

    [field:Header("Base Stats")]
    [field:SerializeField]
    public int BaseMaxHealth { get; private set; }

    [field: Header("Visuals")]
    [field: SerializeField]
    public Animator VisualsPrefab { get; private set; }
    [field: SerializeField]
    public Material MaterialPrefab  { get; private set; }
    [field: SerializeField]
    public Sprite CharacterSprite { get; private set; }
    
    [field: SerializeField]
    public Sprite MugshotSprite { get; private set; }
    [field: SerializeField]
    public Sprite IconSprite { get; private set; }

}
