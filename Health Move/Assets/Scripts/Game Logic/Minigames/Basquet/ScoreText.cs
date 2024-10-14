using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    public void EndTurn()
    {
        Debug.Log("End turn scored");
        GameManager.gm.CurrMinigameManager.OnTurnEnded();
    }
}
