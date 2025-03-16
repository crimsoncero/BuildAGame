using UnityEngine;

public class HeroMover : MonoBehaviour
{
    [field:SerializeField]
    public float Speed { get; private set; } = 6;

    [SerializeField] private float _flipTolerance = 0.1f;
    [SerializeField] private float _flipAhead = 0.1f;
    [SerializeField] private Rigidbody2D _rb2d;

    private Vector2 inputVelocity;

    private void Start()
    {
        GameManager.Instance.OnGamePaused += PauseMover;
    }

    public void Move(Vector2 direction)
    {
        HandleDirectionChange(direction);
        
        inputVelocity = direction * Speed;
     
    }
    public void SetSpeed(float speed)
    {
        Speed = speed;
    }

    private void HandleDirectionChange(Vector2 direction)
    {
        if (direction == Vector2.zero) return;
        
        Vector2 centerPos = PlayerController.Instance.CenterPosition;
        Vector2 centerVec = (Vector2)transform.position - centerPos;

        
        // Handle X flip
        var checkToleranceX =  Mathf.Abs(centerVec.x) > _flipTolerance;
        var isOppositeSignX = (centerVec.x < 0) != (direction.x < 0);
        var isDirXZero = Mathf.Approximately(direction.x, 0);
        if (!isDirXZero && checkToleranceX && isOppositeSignX)
        {
            var vector3 = transform.position;
            vector3.x = centerPos.x + (_flipAhead * (direction.x > 0 ? 1 : -1));
            transform.position = vector3;
        }
        
        // Handle Y flip
        var checkToleranceY = Mathf.Abs(centerVec.y) > _flipTolerance;
        var isOppositeSignY = (centerVec.y < 0) != (direction.y < 0);
        var isDirYZero = Mathf.Approximately(direction.y, 0);

        if (!isDirYZero && checkToleranceY && isOppositeSignY)
        {
            var vector3 = transform.position;
            vector3.y = centerPos.y + (_flipAhead * (direction.y > 0 ? 1 : -1));
            transform.position = vector3;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }

    #region Pause & Resume

    private void PauseMover()
    {
        // Set speed to zero
        _rb2d.linearVelocity = Vector2.zero;
    }

    private void Update()
    {
        if (_rb2d != null)
        {
            _rb2d.linearVelocity = GameManager.Instance.IsPaused ? Vector2.zero : inputVelocity;
        }
    }

    #endregion
}
