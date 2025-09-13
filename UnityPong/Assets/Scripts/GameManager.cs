using UnityEngine;
using System;  // needed for Action

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int maxScore = 5;

    [Header("UI References")]
    public GameUI gameUI;

    [Header("Score Data")]
    public int scorePlayer1, scorePlayer2;
    public ScoreText scoreTextLeft, scoreTextRight;

    // ✅ Events used by Ball
    public event Action onReset;     // fired after a point (no winner yet)
    public event Action onGameOver;  // fired when someone wins

    private bool gameOver;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy(gameObject); return; }

        if (gameUI == null)
            gameUI = FindFirstObjectByType<GameUI>();
    }

    public void OnScoreZoneReached(int id)
    {
        if (gameOver) return;

        if (id == 1)      scorePlayer1++;
        else if (id == 2) scorePlayer2++;

        UpdateScores();
        CheckWinOrReset();
    }

    private void UpdateScores()
    {
        if (scoreTextLeft  != null) scoreTextLeft.SetScore(scorePlayer1);
        if (scoreTextRight != null) scoreTextRight.SetScore(scorePlayer2);
    }

    private void CheckWinOrReset()
    {
        int winnerId = (scorePlayer1 >= maxScore) ? 1 :
                       (scorePlayer2 >= maxScore) ? 2 : 0;

        if (winnerId != 0)
        {
            gameOver = true;
            gameUI?.OnGameEnds(winnerId);
            onGameOver?.Invoke();     // 🔔 tell Ball to stop
        }
        else
        {
            onReset?.Invoke();        // 🔔 tell Ball to reset & auto-serve
        }
    }

    // optional: called from Start button before first serve
    public void ResetMatch()
    {
        gameOver = false;
        scorePlayer1 = 0;
        scorePlayer2 = 0;
        UpdateScores();
    }
}

