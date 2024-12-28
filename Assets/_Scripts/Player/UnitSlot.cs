using UnityEngine;

public class UnitSlot : MonoBehaviour
{
    [field: SerializeField]
    public HeroUnit Unit { get; private set; }

    public bool IsControlled { get; private set; } = false;

    public void SetVelocity(Vector2 direction)
    {
        if (!IsControlled)
            throw new System.Exception("Tried moving a non controlled unit!");
        
        Unit.SetVelocity(direction);
    }

    public void SetControl(bool isControlled, Transform target = null)
    {
        Unit.PathfindingModule.SetTarget(target);

        if (isControlled)
        {
            IsControlled = true;
            Unit.PathfindingModule.IsEnabled = false;
            Unit.Collider.forceReceiveLayers &= ~(1 << Unit.gameObject.layer); 
        }
        else
        {
            IsControlled = false;
            Unit.PathfindingModule.IsEnabled = true;
            Unit.Collider.forceReceiveLayers |= (1 << Unit.gameObject.layer);
        }
    }

}
