using UnityEngine;
using System;  

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int maxScore = 5;

    [Header("UI References")]
    public GameUI gameUI;

    [Header("Score Data")]
    public int scorePlayer1, scorePlayer2;
    public ScoreText scoreTextLeft, scoreTextRight;

    
    public event Action onReset;     
    public event Action onGameOver;  

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
            onGameOver?.Invoke();     
        }
        else
        {
            onReset?.Invoke();        
        }
    }

    
    public void ResetMatch()
    {
        gameOver = false;
        scorePlayer1 = 0;
        scorePlayer2 = 0;
        UpdateScores();
    }
}

