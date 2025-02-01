using System;
using UnityEngine;
using UnityEngine.Pool;

public class XPManager : Singleton<XPManager>
{
    // The type of gems in the game, spaced to allow more to be added if needed.
    public enum XPGemTypes { Common = 0, Rare = 25, Epic = 50, Unique = 99,}

    // XP Management


    // XP Gems

    [Header("XP Gem Creation Params")]
    [SerializeField] private XPGem _gemPrefab;
    [SerializeField] private Transform _gemParent;
    [SerializeField] private int _maxInstanceAmount = 500;
    [Space]
    [SerializeField] private XPGemData _commonGemData;
    [SerializeField] private XPGemData _rareGemData;
    [SerializeField] private XPGemData _uniqueGemData;
    [SerializeField] private XPGemData _epicGemData;

    private ObjectPool<XPGem> _gemPool;
    private XPGem _uniqueGem;
    private int _storedXP;

    private void Start()
    {
        _gemPool = new ObjectPool<XPGem>(CreateGem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, false, _maxInstanceAmount, _maxInstanceAmount);
        _gemPool.PreWarm(_maxInstanceAmount);
        _uniqueGem = null;
        _storedXP = 0;
    }

    #region Pool Methods

    private XPGem CreateGem()
    {
        XPGem gem = Instantiate(_gemPrefab, _gemParent);

        gem.gameObject.SetActive(false);

        return gem;
    }

    private void OnTakeFromPool(XPGem gem)
    {
        gem.gameObject.SetActive(true);
    }
    private void OnReturnedToPool(XPGem gem)
    {
        gem.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(XPGem gem)
    {
        Destroy(gem.gameObject);
    }
    #endregion

    public void SpawnGem(XPGemTypes gemType, Vector3 position)
    {
        // Check if max capacity reached (minus one because the unique gem is also part of the max capacity)
        if(_gemPool.CountActive >= _maxInstanceAmount - 1)
        {
            // Don't spawn the gem, rather add its xp equivelent to the storage.
            _storedXP += GemToXP(gemType);

            // Check whether a unique gem needs to be spawned
            if (_uniqueGem == null)
            {
                XPGem unique = _gemPool.Get();
                unique.Init(_uniqueGemData, position);
                _uniqueGem = unique;
            }
        }
        else
        {

            // Spawn a gem according to the type given.
            XPGemData gemData;
            switch (gemType)
            {
                case XPGemTypes.Common:
                    gemData = _commonGemData;
                    break;
                case XPGemTypes.Rare:
                    gemData = _rareGemData;
                    break;
                case XPGemTypes.Epic:
                    gemData = _epicGemData;
                    break;
                case XPGemTypes.Unique:
                    Debug.LogError("Can't spawn a unique gem directly");
                    return;
                default:
                    Debug.LogError("Not a valid gem type");
                    return;
            }

            XPGem gem = _gemPool.Get();
            gem.Init(gemData, position);
        }
    }

    public int GemToXP (XPGemTypes gemType)
    {
        throw new NotImplementedException();
    }
}
