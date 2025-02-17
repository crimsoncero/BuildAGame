using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class XPManager : Singleton<XPManager>
{
    public event Action OnLevelUp;
    
    // The type of gems in the game, spaced to allow more to be added if needed.
    public enum XPGemTypes { Common = 0, Rare = 25, Epic = 50, Unique = 99,}

    // XP Management
    [Header("XP Data")]
    [SerializeField] private XPData _xpData;

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
    private XPGem _uniqueGem = null;
    private int _storedXP = 0;

    private int _currentXP = 0;
    private int _targetXP = 0;
    private int _currentLevel = 1;
    
    private void Start()
    {
        _gemPool = new ObjectPool<XPGem>(CreateGem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, false, _maxInstanceAmount, _maxInstanceAmount);
        _gemPool.PreWarm(_maxInstanceAmount);
        _targetXP = _xpData.BaseXpToLevelUp;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsPaused)
        {
            if (_currentXP >= _targetXP)
            {
                _currentLevel++;
                _targetXP += _xpData.CalculateXpNeededToLevelUp(_currentLevel);
                OnLevelUp?.Invoke();
            }
        }
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
            // Don't spawn the gem, rather add its xp equivalent to the storage.
            _storedXP += _xpData.GetGemXpAmount(gemType);

            // Check whether a unique gem needs to be spawned
            if (_uniqueGem != null)
                return;

            XPGem unique = _gemPool.Get();
            unique.Init(_uniqueGemData, position, _gemPool);
            _uniqueGem = unique;
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
            gem.Init(gemData, position, _gemPool);
        }
    }

    public void AddXp(XPGemTypes gemType)
    {
        if (gemType == XPGemTypes.Epic)
            _currentXP += _storedXP;
        else
            _currentXP += _xpData.GetGemXpAmount(gemType);
    }

    private void OnGUI()
    {
        GUI.TextArea(new Rect(Screen.width - 400, 10, 300, 300), $"Current XP: {_currentXP}\n Target XP: {_targetXP}\n Current Level: {_currentLevel}" );
    }
}
