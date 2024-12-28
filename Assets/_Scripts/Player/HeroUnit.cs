using Pathfinding;
using UnityEngine;

public class HeroUnit : MonoBehaviour
{

    [SerializeField] private Rigidbody2D _rb2d;

    public PolygonCollider2D Collider;
    public PathfindingModule PathfindingModule;

    // Change into stat SO later

    /// <summary>
    /// Speed in world units per seconds.
    /// </summary>
    [SerializeField] private float MaxSpeed = 2;
    [SerializeField] private float MaxAcceleration = -10f;

    private void Awake()
    {
        PathfindingModule.SetMaxSpeed(MaxSpeed * 2f);
        PathfindingModule.SetMaxAcceleration(MaxAcceleration);
    }


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

}
