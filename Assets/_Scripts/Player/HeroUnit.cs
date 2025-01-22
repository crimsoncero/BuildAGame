using Unity.VisualScripting;
using UnityEngine;
using System;

public class HeroUnit : MonoBehaviour
{
    [field: SerializeField] public HeroData Data { get; private set; }


    [Header("Components")]
    [SerializeField] private Rigidbody2D _rb2d;
    public PathfindingModule PathfindingModule;

    // STATS::
    public int CurrentHealth { get; private set; }

    // Stats computational properties (if complicated, use an intermediary method)
    public int MaxHealth { get { return Data.BaseMaxHealth; } }
    public float MaxSpeed { get { return Data.BaseMaxSpeed; } }
    public int Power { get { return Data.BasePower; } }
    public float AttackSpeed { get { return Data.BaseAttackSpeed; } }



    private void Start()
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
        PathfindingModule.SetMaxAcceleration(1000);



        AddCallbacks();
    }

    private void AddCallbacks()
    {
        GameManager.Instance.OnGamePaused += PauseHero;
        GameManager.Instance.OnGameResumed += ResumeHero;
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

    #region Pause & Resume

    private void PauseHero()
    {
        // Set speed to zero
        _rb2d.linearVelocity = Vector2.zero;
        PathfindingModule.PausePathfinding();
    }

    private void ResumeHero()
    {
        PathfindingModule.ResumePathfinding();
    }


    #endregion
}
