using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public float maxInitialAngle = 0.67f;
    public float moveSpeed = 1f;
    public float maxStartY = 4f;

    private float startX = 0f;

    private void Awake()
    {
        if (rb2d == null) rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (GameManager.instance == null || GameManager.instance.gameUI == null)
        {
            Debug.LogError("Ball: GameManager.instance or gameUI is null.");
            enabled = false;
            return;
        }

        GameManager.instance.gameUI.onStartGame += OnStartGame;
        GameManager.instance.onReset           += OnResetPoint;
        GameManager.instance.onGameOver        += OnGameOver;

        rb2d.simulated = false;
        rb2d.linearVelocity = Vector2.zero;
    }

    private void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            if (GameManager.instance.gameUI != null)
                GameManager.instance.gameUI.onStartGame -= OnStartGame;

            GameManager.instance.onReset    -= OnResetPoint;
            GameManager.instance.onGameOver -= OnGameOver;
        }
    }

    private void OnStartGame()
    {
        GameManager.instance.ResetMatch();
        ServeNewBall();
    }

    private void OnResetPoint()
    {
        ServeNewBall();
    }

    private void OnGameOver()
    {
        rb2d.linearVelocity = Vector2.zero;
        rb2d.simulated = false;
    }

    private void ServeNewBall()
    {
        rb2d.simulated = true;
        ResetBall();
        InitialPush();
    }

    private void InitialPush()
    {
        Vector2 dir = (Random.value < 0.5f ? Vector2.left : Vector2.right);
        dir.y = Random.Range(-maxInitialAngle, maxInitialAngle);
        rb2d.linearVelocity = dir.normalized * moveSpeed;   // ✅ use velocity
    }

    private void ResetBall()
    {
        float posY = Random.Range(-maxStartY, maxStartY);
        transform.position = new Vector2(startX, posY);
        rb2d.linearVelocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var scoreZone = collision.GetComponent<ScoreZone>();
        if (scoreZone)
        {
            // stop movement immediately on score
            rb2d.linearVelocity = Vector2.zero;
            rb2d.simulated = false;

            // notify manager (manager will fire onReset or onGameOver)
            GameManager.instance.OnScoreZoneReached(scoreZone.id);
        }
    }
}