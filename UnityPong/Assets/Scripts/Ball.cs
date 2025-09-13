using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameManager gameManager;
    public Rigidbody2D rb2d;
    public float maxInitialAngle = 0.67f;
    public float moveSpeed = 1f;
    public float maxStartY = 4f;
   

    private float startX = 0f;

     private void Awake()
    {
        if (rb2d == null) rb2d = GetComponent<Rigidbody2D>();   // ensure assigned
    }

    private void Start()
    {
        GameManager.instance.gameUI.onStartGame += OnStartGame;
    }

     private void OnStartGame()
    {
        ResetBall();
        InitialPush();                                          // ← push when Start is clicked
    }
    private void InitialPush()
    {
        Vector2 dir = Vector2.left;

        if (Random.value < 0.5f)
            dir = Vector2.right;

        dir.y = Random.Range(-maxInitialAngle, maxInitialAngle);
        rb2d.linearVelocity = dir * moveSpeed;
    }

    private void ResetBall()
    {
        float posY = Random.Range(-maxStartY, maxStartY);
        Vector2 position = new Vector2(startX, posY);
        transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ScoreZone scoreZone = collision.GetComponent<ScoreZone>();
        if(scoreZone)
        {
            gameManager.OnScoreZoneReached(scoreZone.id);
            ResetBall();
            InitialPush();
        }
    }

    
}