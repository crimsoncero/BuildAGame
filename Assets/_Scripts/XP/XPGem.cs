using System;
using UnityEngine;
using UnityEngine.Pool;

public class XPGem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private LayerMask _heroLayer;
    public XPGemData Data {  get; private set; }
    private ObjectPool<XPGem> _pool;
    
    public void Init(XPGemData data, Vector3 position, ObjectPool<XPGem> pool)
    {
        Data = data;
        transform.position = position;
        _pool = pool;
        _spriteRenderer.sprite = Data.Sprite;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        XPManager.Instance.AddXp(Data.Type);
        _pool?.Release(this);
    }
}
