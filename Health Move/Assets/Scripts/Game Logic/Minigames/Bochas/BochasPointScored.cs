using TMPro;
using UnityEngine;

public class BochasPointScored : PointScoreReciever
{
    [SerializeField] TextMeshPro text;
    [SerializeField] string team;
    public override void OnScored(MinigameManager minigameManager)
    {
        base.OnScored(minigameManager);

        BochasMinigameManager bochasMinigameManager = minigameManager as BochasMinigameManager;

        text.text = bochasMinigameManager.Scored[team].ToString();

        Debug.Log("Scored");
    }
}
