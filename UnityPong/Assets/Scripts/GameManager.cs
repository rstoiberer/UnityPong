using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI References")]
    public GameUI gameUI;

    [Header("Score Data")]
    public int scorePlayer1, scorePlayer2;
    public ScoreText scoreTextLeft, scoreTextRight;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy(gameObject); return; }

        // Optional: persist across scenes
        // DontDestroyOnLoad(gameObject);

        // Auto-assign if not set in Inspector
        if (gameUI == null)
            gameUI = FindFirstObjectByType<GameUI>();   // ✅ no obsolete warning
    }

    public void OnScoreZoneReached(int id)
    {
        if (id == 1) scorePlayer1++;
        if (id == 2) scorePlayer2++;
        UpdateScores();
    }

    private void UpdateScores()
    {
        if (scoreTextLeft != null) scoreTextLeft.SetScore(scorePlayer1);
        if (scoreTextRight != null) scoreTextRight.SetScore(scorePlayer2);
    }
}

