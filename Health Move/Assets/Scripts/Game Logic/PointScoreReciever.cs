using UnityEngine;

//Handles score related events that don't affect the minigame logic
public class PointScoreReciever : MonoBehaviour
{
    public virtual void OnScored(MinigameManager minigameManager) //Is called by GM when scored
    {

    }
    public virtual void OnRecieved()
    {

    }
}
