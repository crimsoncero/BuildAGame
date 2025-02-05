using UnityEngine;

public class Projectile : MonoBehaviour
{
    [field: SerializeField] public XPGemData Data {  get; private set; }


    public void Init(XPGemData data, Vector3 position)
    {
        Data = data;
        transform.position = position;
    }

}
