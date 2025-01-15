using UnityEngine;

public class HeroMover : MonoBehaviour
{
    [field:SerializeField]
    public float Speed { get; private set; } = 6;

    [SerializeField] private Rigidbody2D _rb2d;

    public void Move(Vector2 direction)
    {
        _rb2d.linearVelocity = direction * Speed;
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
}
