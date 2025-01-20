using UnityEngine;

public class HeroMover : MonoBehaviour
{
    [field:SerializeField]
    public float Speed { get; private set; } = 6;

    [SerializeField] private Rigidbody2D _rb2d;

    private Vector2 inputVelocity;

    private void Start()
    {
        GameManager.Instance.OnGamePaused += PauseMover;
    }

    public void Move(Vector2 direction)
    {
       
        inputVelocity = direction * Speed;
     
    }
    public void SetSpeed(float speed)
    {
        Speed = speed;
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
