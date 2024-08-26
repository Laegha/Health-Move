using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BasquetPointScored : PointScoreReciever
{
    [SerializeField] TextMeshPro text;
    public override void OnScored(MinigameManager minigameManager)
    {
        base.OnScored(minigameManager);

        BasquetMinigameManager basquetMinigameManager = minigameManager as BasquetMinigameManager;

        text.text = basquetMinigameManager.scored.ToString();

    }
}
