using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    public int speed = 200;
    public Rigidbody2D rb;
    private void Update()
    {
        Move();
    }

    void Move()
    {
        Vector3 moveVec = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        rb.linearVelocity = moveVec * speed * Time.deltaTime;
    }
}
