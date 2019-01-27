using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    public Text player1Score;
    public Text player2Score;

    public static ScoreKeeper instance;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateScore(int whichPlayer, int score)
    {
        if (whichPlayer == 1)
            player1Score.text = $"Player 1: {score}";
        else
            player2Score.text = $"Player 2: {score}";
    }
}
