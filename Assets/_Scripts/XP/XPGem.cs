using UnityEngine;
using SeraphUtil.ObjectPool;

public class XPGem : MonoBehaviour, IPoolable
{
    [Header("Components")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private LayerMask _heroLayer;
    [SerializeField] private CircleCollider2D _innerCollider;
    [SerializeField] private CircleCollider2D _outerCollider;
    [SerializeField] private PathfindingModule _pathfindingModule;
    
    [Header("General Settings")] 
    [SerializeField] private float _basePickupRange = 2f;
    [SerializeField] private float _speed = 10f;
        
    
    public XPGemData Data {  get; private set; }
    private ObjectPool<XPGem> _pool;
    private bool _isAbsorbed = false;
    public void Init(XPGemData data, Vector3 position, ObjectPool<XPGem> pool, float pickupRange = 0f)
    {
        if (this == null)
        {
            Debug.LogError("Gem is null at init");
            return;
        }

        
        Data = data;
        transform.position = position;
        _pool = pool;
        _spriteRenderer.sprite = Data.Sprite;
        _pathfindingModule.SetMaxSpeed(_speed);
        _pathfindingModule.IsEnabled = false;
        _innerCollider.enabled = false;
        _outerCollider.enabled = true;
        _isAbsorbed = false;
        _outerCollider.radius = pickupRange <= _basePickupRange ? _basePickupRange : pickupRange;
        gameObject.SetActive(true);
        GameManager.Instance.OnGamePaused += OnPause;
        GameManager.Instance.OnGameResumed += OnResume;
    }

    public void AbsorbGem()
    {
        HeroUnit closestHero = HeroManager.Instance.GetClosestHero(transform.position);
        if (!closestHero)
        {
            return;
        }
        _innerCollider.enabled = true;
        _outerCollider.enabled = false;
        _isAbsorbed = true;
        _pathfindingModule.SetTarget(closestHero.transform);
        _pathfindingModule.IsEnabled = true;

    }

    private void OnPause()
    {
        _pathfindingModule.AIPath.canMove = false;
    }
    
    private void OnResume()
    {
        _pathfindingModule.AIPath.canMove = true;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isAbsorbed)
        {
            AbsorbGem();
        }
        else
        {
            GameManager.Instance.OnGamePaused -= OnPause;
            GameManager.Instance.OnGameResumed -= OnResume;
            XPManager.Instance.AddXp(Data.Type);
            
            _pool.Return(this);
        }
    }


    public void OnTakeFromPool()
    {
        
    }

    public void OnReturnToPool()
    {
        gameObject.SetActive(false);
    }
}
