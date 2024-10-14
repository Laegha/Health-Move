using TMPro;
using UnityEngine;

public class BasquetPointScored : PointScoreReciever
{
    [SerializeField] TextMeshPro text;
    [SerializeField] string team;
    public override void OnScored(MinigameManager minigameManager)
    {
        base.OnScored(minigameManager);

        BasquetMinigameManager basquetMinigameManager = minigameManager as BasquetMinigameManager;

        text.text = basquetMinigameManager.Scored[team].ToString();

        Debug.Log("Scored");
    }
}
