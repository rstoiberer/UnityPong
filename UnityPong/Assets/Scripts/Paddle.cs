using UnityEngine;

public class Paddle : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public int id;
    public float moveSpeed = 2f;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();  
    }

    private void Update()
    {
        float movement = ProcessInput();
        Move(movement);
    }

    private float ProcessInput() 
    {
        float movement = 0f;
        switch (id)
        {
            case 1:
                movement = Input.GetAxis("MovePlayer1");
                break;
            case 2:
                movement = Input.GetAxis("MovePlayer2");
                break;
        }
        return movement;
    }

    private void Move(float value)
    {
        Vector2 velo = rb2d.linearVelocity;
        velo.y = moveSpeed * value;
        rb2d.linearVelocity = velo;
    }
}
