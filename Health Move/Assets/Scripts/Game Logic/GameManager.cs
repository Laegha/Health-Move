using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager gm
    {
        get { return instance;}
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }
    List<PointScoredReciever> scoredRecievers = new List<PointScoredReciever>();
    MinigameManager currMinigameManager;

    void OnScored()
    {
        foreach (PointScoredReciever reciever in scoredRecievers)
            reciever.OnScored();

        //if(currMinigameManager.hasEnded)
        //end minigame
    }
}
