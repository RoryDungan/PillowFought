﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FinishWindow : MonoBehaviour
{
    public Text winningPlayerText;

    /// <summary>
    /// Populate the wining window with which player won, and hand assigning events for starting again / finishing early
    /// </summary>
    /// <param name="winningPlayer">Winning player.</param>
    /// <param name="rematchEvent">Rematch event.</param>
    /// <param name="mainMenuEvent">Main menu event.</param>
    public void Populate(int winningPlayer)
    {
        winningPlayerText.text = $"Player {winningPlayer} Wins";
    }
}
