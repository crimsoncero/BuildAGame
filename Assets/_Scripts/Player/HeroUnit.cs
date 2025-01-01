using Unity.VisualScripting;
using UnityEngine;
using System;

public class HeroUnit : MonoBehaviour
{
    [field: SerializeField] public HeroData Data { get; private set; }


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
    public float AttackSpeed { get { return Data.BaseAttackSpeed; } }




    private void Awake()
    {
        // Init the unit automatically if starting with data. (for testing mainly)
        if (!Data.IsUnityNull())
            Init(Data);
    }

    public void Init(HeroData data)
    {
        if (data.IsUnityNull())
            throw new ArgumentNullException("data", "Can't initilize a unit with null data");

        Data = data;
        gameObject.name = $"Hero - {Data.name}";

        PathfindingModule.SetMaxSpeed(MaxSpeed);
        PathfindingModule.SetMaxAcceleration(MaxAcceleration);

    }




    #region Control Methods

    /// <summary>
    /// Sets the velocity of the unit in the direction given using its speed.
    /// </summary>
    /// <param name="direction"> The direction of the movement. </param>
    public void SetVelocity(Vector2 direction)
    {
        if (PathfindingModule.IsEnabled)
            throw new System.Exception("Unit can't be controlled, using pathfinder.");

        direction.Normalize();
        _rb2d.linearVelocity = MaxSpeed * direction;
    }

    #endregion
}
