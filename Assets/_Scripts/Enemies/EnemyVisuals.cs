using UnityEngine;

public class EnemyVisuals : MonoBehaviour
{
    private static readonly int GetHit = Animator.StringToHash("GetHit");
    [SerializeField] private Animator _animator;
    
    private EnemyUnit _enemy;
    private Vector3 _position;
    public void Initialize(EnemyUnit enemy)
    {
        _enemy = enemy;
        _animator.speed = 0;
        _position = transform.position;
    }

    private void Update()
    {
        HandleDirection();
        SetSpeed();
    }

    private void HandleDirection()
    {
        var scale = transform.localScale;
        if (_enemy.PathfindingModule.AIPath.velocity.x > 0.5f)
        {
            scale.x = 1;
        }
        else if (_enemy.PathfindingModule.AIPath.velocity.x < 0.5f)
        {
            scale.x = -1;
        }

        transform.localScale = scale;

    }

    private void SetSpeed()
    {
        if (_enemy.PathfindingModule.AIPath.velocity.magnitude > 0.1f)
        {
            _animator.speed = 1;
        }
        else
        {
            _animator.speed = 0;
        }
    }

    public void OnHit()
    {
        _animator.SetTrigger(GetHit);
    }
}
