using Pathfinding;
using UnityEngine;

public class PathfindingModule : MonoBehaviour
{
    public AIPath AIPath;
    public AIDestinationSetter DestinationSetter;
    public Seeker Seeker;

    [SerializeField]
    private bool _isEnabled;

    public bool IsEnabled
    {
        get { return _isEnabled; }
        set 
        { 
            _isEnabled = value;
            
            if (_isEnabled)
                Enable();
            else
                Disable();
        }
    }


    /// <summary>
    /// Sets the max speed of the pathfinding unit.
    /// </summary>
    /// <param name="maxSpeed"> Max speed in world units per second.</param>
    public void SetMaxSpeed(float maxSpeed)
    {
        AIPath.maxSpeed = maxSpeed;
    }

    /// <summary>
    /// Sets the max acceleration of the pathfinding unit.
    /// </summary>
    /// <param name="maxAcceleration">
    /// Positive values represent an acceleration in world units per second squared.
    /// Negative values are interpreted as an inverse time of how long it should take for the agent to reach its max speed.
    /// For example if it should take roughly 0.4 seconds for the agent to reach its max speed then this field should be set to -1/0.4 = -2.5.
    /// For a negative value the final acceleration will be: -acceleration*maxSpeed.
    /// This behaviour exists mostly for compatibility reasons.
    /// </param>
    public void SetMaxAcceleration(float maxAcceleration)
    {
        AIPath.maxAcceleration = maxAcceleration;
    }

    /// <summary>
    /// Sets the target of the pathfinding unit.
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(Transform target)
    {
        DestinationSetter.target = target;
    }

    private void Enable()
    {
        AIPath.enabled = true;
        DestinationSetter.enabled = true;
        Seeker.enabled = true;
    }

    private void Disable()
    {
        AIPath.enabled = false;
        DestinationSetter.enabled = false;
        Seeker.enabled = false;
    }
}
