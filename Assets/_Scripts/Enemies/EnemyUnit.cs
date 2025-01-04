using System;
using Unity.VisualScripting;
using UnityEngine;


public class EnemyUnit : MonoBehaviour
{
    [field: SerializeField] public EnemyData Data { get; private set; }


    [Header("Components")]
    [SerializeField] private Rigidbody2D _rb2d;
    public PolygonCollider2D Collider;
    public PathfindingModule PathfindingModule;
    [SerializeField] private float MaxAcceleration = -100f;

    // STATS::
    public int CurrentHealth { get; private set; }

    // Stats computational properties (if complicated, use an intermediary method)
    public int MaxHealth { get { return Data.BaseMaxHealth; } }
    public float MaxSpeed { get { return Data.BaseMaxSpeed; } }
    public int Power { get { return Data.BasePower; } }


    private void Awake()
    {
        // Init the unit automatically if starting with data. (for testing mainly)
        if (!Data.IsUnityNull())
            Init(Data);
    }

    public void Init(EnemyData data)
    {
        if (data.IsUnityNull())
            throw new ArgumentNullException("data", "Can't initilize a unit with null data");

        Data = data;
        gameObject.name = $"Enemy - {Data.name}";

        PathfindingModule.SetMaxSpeed(MaxSpeed);
        PathfindingModule.SetMaxAcceleration(MaxAcceleration);
        

        // Set target stuff
    }

}
