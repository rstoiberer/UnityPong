using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public float maxInitialAngle = 0.67f;
    public float moveSpeed = 1f;
    public float maxStartY = 4f;

    private float startX = 0f;

    public AudioClip sfxPaddle;
    public AudioClip sfxWall;
    public AudioClip sfxScore;

    private AudioSource audioSource;

    private void Awake()
    {
        if (rb2d == null) rb2d = GetComponent<Rigidbody2D>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D sound
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
    if (!scoreZone) return;

    // play goal sfx once
    if (sfxScore) audioSource.PlayOneShot(sfxScore);

    // stop movement immediately
    rb2d.linearVelocity = Vector2.zero;
    rb2d.simulated = false;

    // score ONCE
    GameManager.instance.OnScoreZoneReached(scoreZone.id);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
    // Paddle hit?
    if (collision.collider.GetComponent<Paddle>())
    {
        if (sfxPaddle) audioSource.PlayOneShot(sfxPaddle);
    }
    else
    {
        // Likely a wall (top/bottom). Tag walls as "Wall" if you want to be explicit.
        if (sfxWall) audioSource.PlayOneShot(sfxWall);
    }
    }
}