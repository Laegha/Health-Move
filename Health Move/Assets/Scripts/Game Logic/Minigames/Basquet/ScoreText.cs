using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    public void EndTurn()
    {
        GameManager.gm.CurrMinigameManager.OnTurnEnded();
    }
}
