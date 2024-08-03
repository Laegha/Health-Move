using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasquetMinigameManager : MinigameManager
{
    int _scored = 0;
    int _neededScore = 3;

    public override void OnScored()
    {
        base.OnScored();
        Debug.Log("Score = " + _scored);
        _scored++;

        if(_scored == _neededScore)
            GameManager.gm.EndMinigame();
    }
}
